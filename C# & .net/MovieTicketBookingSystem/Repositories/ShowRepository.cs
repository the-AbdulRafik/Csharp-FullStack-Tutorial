using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Repositories;

public class ShowRepository(ApplicationDbContext db) : IShowRepository
{
    public Task<List<Show>> GetAsync(DateTime? date, int? movieId, int? theatreId) =>
        db.Shows.AsNoTracking().Include(x => x.Movie).Include(x => x.Screen).Where(x => (!date.HasValue || x.StartTime.Date == date.Value.Date) && (!movieId.HasValue || x.MovieId == movieId) && (!theatreId.HasValue || x.Screen.TheatreId == theatreId)).OrderBy(x => x.StartTime).ToListAsync();
    public Task<Show?> GetByIdAsync(int id) => db.Shows.Include(x => x.Movie).Include(x => x.Screen).Include(x => x.ShowSeats).ThenInclude(x => x.Seat).SingleOrDefaultAsync(x => x.ShowId == id);
    public Task<bool> MovieExistsAsync(int movieId) => db.Movies.AnyAsync(x => x.MovieId == movieId);
    public Task<bool> ScreenExistsAsync(int screenId) => db.Screens.AnyAsync(x => x.ScreenId == screenId);
    public Task<bool> HasOverlapAsync(int screenId, DateTime start, DateTime end, int? excludedShowId = null) =>
        db.Shows.AnyAsync(x => x.ScreenId == screenId && (!excludedShowId.HasValue || x.ShowId != excludedShowId) && start < x.EndTime && end > x.StartTime);
    public Task<List<Seat>> GetScreenSeatsAsync(int screenId) => db.Seats.Where(x => x.ScreenId == screenId).ToListAsync();
    public Task<List<ShowSeat>> GetShowSeatsAsync(int showId, SeatStatus? status = null) =>
        db.ShowSeats.AsNoTracking().Include(x => x.Seat).Include(x => x.Show).Where(x => x.ShowId == showId && (!status.HasValue || x.Status == status)).OrderBy(x => x.Seat.RowName).ThenBy(x => x.Seat.SeatNumber).ToListAsync();
    public Task<bool> ShowExistsAsync(int showId) => db.Shows.AnyAsync(x => x.ShowId == showId);
    public Task<bool> HasBookingsAsync(int showId) => db.Bookings.AnyAsync(x => x.ShowId == showId);
    public Task AddAsync(Show show) => db.Shows.AddAsync(show).AsTask();
    public void Remove(Show show) => db.Shows.Remove(show);
    public Task SaveChangesAsync() => db.SaveChangesAsync();
}
