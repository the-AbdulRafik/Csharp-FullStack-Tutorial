using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Repositories.Interfaces;

public interface IShowRepository
{
    Task<List<Show>> GetAsync(DateTime? date, int? movieId, int? theatreId);
    Task<Show?> GetByIdAsync(int id);
    Task<bool> MovieExistsAsync(int movieId);
    Task<bool> ScreenExistsAsync(int screenId);
    Task<bool> HasOverlapAsync(int screenId, DateTime start, DateTime end, int? excludedShowId = null);
    Task<List<Seat>> GetScreenSeatsAsync(int screenId);
    Task<List<ShowSeat>> GetShowSeatsAsync(int showId, SeatStatus? status = null);
    Task<bool> ShowExistsAsync(int showId);
    Task<bool> HasBookingsAsync(int showId);
    Task AddAsync(Show show);
    void Remove(Show show);
    Task SaveChangesAsync();
}
