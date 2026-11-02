using MovieTicketBookingSystem.DTOs.Shows;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Services;

public class ShowService(IShowRepository shows) : IShowService
{
    public async Task<List<ShowResponse>> GetAsync(DateTime? date, int? movieId, int? theatreId) =>
        (await shows.GetAsync(date, movieId, theatreId)).Select(ToResponse).ToList();

    public async Task<ShowResponse?> GetByIdAsync(int id) =>
        (await shows.GetByIdAsync(id)) is { } show ? ToResponse(show) : null;

    public async Task<ShowResponse> CreateAsync(CreateShowRequest request)
    {
        ValidateTime(request.StartTime, request.EndTime);
        if (!await shows.MovieExistsAsync(request.MovieId))
            throw new KeyNotFoundException("Movie was not found.");
        if (!await shows.ScreenExistsAsync(request.ScreenId))
            throw new KeyNotFoundException("Screen was not found.");
        if (await shows.HasOverlapAsync(request.ScreenId, request.StartTime, request.EndTime))
            throw new InvalidOperationException("The screen already has an overlapping show.");
        var physicalSeats = await shows.GetScreenSeatsAsync(request.ScreenId);
        if (physicalSeats.Count == 0)
            throw new InvalidOperationException("The selected screen has no configured seats.");
        var show = new Show
        {
            MovieId = request.MovieId,
            ScreenId = request.ScreenId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            TicketPrice = request.TicketPrice,
            ShowSeats = physicalSeats.Select(x => new ShowSeat { SeatId = x.SeatId }).ToList()
        };
        await shows.AddAsync(show);
        await shows.SaveChangesAsync();
        return (await GetByIdAsync(show.ShowId))!;
    }

    public async Task<ShowResponse?> UpdateAsync(int id, UpdateShowRequest request)
    {
        ValidateTime(request.StartTime, request.EndTime);
        var show = await shows.GetByIdAsync(id);
        if (show is null) return null;
        if (!await shows.MovieExistsAsync(request.MovieId))
            throw new KeyNotFoundException("Movie was not found.");
        if (!await shows.ScreenExistsAsync(request.ScreenId))
            throw new KeyNotFoundException("Screen was not found.");
        if (await shows.HasOverlapAsync(request.ScreenId, request.StartTime, request.EndTime, id))
            throw new InvalidOperationException("The screen already has an overlapping show.");
        show.MovieId = request.MovieId;
        show.ScreenId = request.ScreenId;
        show.StartTime = request.StartTime;
        show.EndTime = request.EndTime;
        show.TicketPrice = request.TicketPrice;
        await shows.SaveChangesAsync();
        return (await GetByIdAsync(id))!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var show = await shows.GetByIdAsync(id);
        if (show is null) return false;
        if (await shows.HasBookingsAsync(id))
            throw new InvalidOperationException("A show with booking history cannot be deleted.");
        shows.Remove(show);
        await shows.SaveChangesAsync();
        return true;
    }

    public async Task<List<ShowSeatResponse>?> GetSeatsAsync(int showId, SeatStatus? status = null)
    {
        if (!await shows.ShowExistsAsync(showId)) return null;
        return (await shows.GetShowSeatsAsync(showId, status))
            .Select(x => new ShowSeatResponse(x.SeatId, x.Seat.RowName, x.Seat.SeatNumber,
                x.Seat.RowName + x.Seat.SeatNumber, x.Seat.SeatType, x.Show.TicketPrice, x.Status.ToString()))
            .ToList();
    }

    private static void ValidateTime(DateTime start, DateTime end)
    {
        if (end <= start) throw new ArgumentException("EndTime must be later than StartTime.");
    }

    private static ShowResponse ToResponse(Show x) =>
        new(x.ShowId, x.MovieId, x.Movie.Title, x.ScreenId, x.Screen.ScreenName,
            x.Screen.TheatreId, x.StartTime, x.EndTime, x.TicketPrice);
}
