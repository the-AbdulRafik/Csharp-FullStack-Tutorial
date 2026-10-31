using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.DTOs.Shows;
using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Services;

public class ShowService(ApplicationDbContext db)
{
    public async Task<List<ShowResponse>> GetAsync(DateTime? date, int? movieId, int? theatreId)
    {
        var query = db.Shows.AsNoTracking().Include(x => x.Movie).Include(x => x.Screen).AsQueryable();
        if (date.HasValue) query = query.Where(x => x.StartTime.Date == date.Value.Date);
        if (movieId.HasValue) query = query.Where(x => x.MovieId == movieId);
        if (theatreId.HasValue) query = query.Where(x => x.Screen.TheatreId == theatreId);
        return await query.OrderBy(x => x.StartTime).Select(ToResponse).ToListAsync();
    }

    public async Task<ShowResponse?> GetByIdAsync(int id) =>
        await db.Shows.AsNoTracking().Include(x => x.Movie).Include(x => x.Screen)
            .Where(x => x.ShowId == id).Select(ToResponse).FirstOrDefaultAsync();

    public async Task<ShowResponse> CreateAsync(CreateShowRequest request)
    {
        ValidateTime(request.StartTime, request.EndTime);
        if (!await db.Movies.AnyAsync(x => x.MovieId == request.MovieId)) throw new KeyNotFoundException("Movie was not found.");
        if (!await db.Screens.Include(x => x.Theatre).AnyAsync(x => x.ScreenId == request.ScreenId)) throw new KeyNotFoundException("Screen was not found.");
        await EnsureNoOverlapAsync(request.ScreenId, request.StartTime, request.EndTime, null);
        var show = new Show { MovieId = request.MovieId, ScreenId = request.ScreenId, StartTime = request.StartTime, EndTime = request.EndTime, TicketPrice = request.TicketPrice };
        var seats = await db.Seats.Where(x => x.ScreenId == request.ScreenId).ToListAsync();
        if (seats.Count == 0) throw new InvalidOperationException("The selected screen has no configured seats.");
        show.ShowSeats = seats.Select(x => new ShowSeat { SeatId = x.SeatId, Status = SeatStatus.Available }).ToList();
        db.Shows.Add(show); await db.SaveChangesAsync();
        return (await GetByIdAsync(show.ShowId))!;
    }

    public async Task<ShowResponse?> UpdateAsync(int id, UpdateShowRequest request)
    {
        ValidateTime(request.StartTime, request.EndTime);
        var show = await db.Shows.FindAsync(id); if (show is null) return null;
        if (!await db.Movies.AnyAsync(x => x.MovieId == request.MovieId)) throw new KeyNotFoundException("Movie was not found.");
        if (!await db.Screens.AnyAsync(x => x.ScreenId == request.ScreenId)) throw new KeyNotFoundException("Screen was not found.");
        await EnsureNoOverlapAsync(request.ScreenId, request.StartTime, request.EndTime, id);
        show.MovieId = request.MovieId; show.ScreenId = request.ScreenId; show.StartTime = request.StartTime; show.EndTime = request.EndTime; show.TicketPrice = request.TicketPrice;
        await db.SaveChangesAsync(); return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var show = await db.Shows.FindAsync(id); if (show is null) return false;
        if (await db.Bookings.AnyAsync(x => x.ShowId == id)) throw new InvalidOperationException("A show with booking history cannot be deleted.");
        db.Shows.Remove(show); await db.SaveChangesAsync(); return true;
    }

    public async Task<List<ShowSeatResponse>?> GetSeatsAsync(int showId, SeatStatus? status = null)
    {
        if (!await db.Shows.AnyAsync(x => x.ShowId == showId)) return null;
        var query = db.ShowSeats.AsNoTracking().Where(x => x.ShowId == showId).Include(x => x.Seat).Include(x => x.Show).AsQueryable();
        if (status.HasValue) query = query.Where(x => x.Status == status);
        return await query.OrderBy(x => x.Seat.RowName).ThenBy(x => x.Seat.SeatNumber)
            .Select(x => new ShowSeatResponse(x.SeatId, x.Seat.RowName, x.Seat.SeatNumber, x.Seat.RowName + x.Seat.SeatNumber, x.Seat.SeatType, x.Show.TicketPrice, x.Status.ToString())).ToListAsync();
    }

    private async Task EnsureNoOverlapAsync(int screenId, DateTime start, DateTime end, int? excludedId) =>
        _ = await db.Shows.AnyAsync(x => x.ScreenId == screenId && (!excludedId.HasValue || x.ShowId != excludedId) && start < x.EndTime && end > x.StartTime)
            ? throw new InvalidOperationException("The screen already has an overlapping show.")
            : false;

    private static void ValidateTime(DateTime start, DateTime end)
    {
        if (end <= start) throw new ArgumentException("EndTime must be later than StartTime.");
    }

    private static readonly Expression<Func<Show, ShowResponse>> ToResponse = x =>
        new(x.ShowId, x.MovieId, x.Movie.Title, x.ScreenId, x.Screen.ScreenName, x.Screen.TheatreId, x.StartTime, x.EndTime, x.TicketPrice);
}
