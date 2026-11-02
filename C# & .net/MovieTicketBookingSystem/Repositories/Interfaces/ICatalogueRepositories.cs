using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Repositories.Interfaces;

public interface ITheatreRepository
{
    Task<List<Theatre>> GetAllAsync(int pageNumber, int pageSize);
    Task<Theatre?> GetByIdAsync(int id);
    Task AddAsync(Theatre theatre);
    void Remove(Theatre theatre);
    Task SaveChangesAsync();
}

public interface IScreenRepository
{
    Task<List<Screen>> GetAllAsync(int? theatreId);
    Task<Screen?> GetByIdAsync(int id);
    Task<bool> TheatreExistsAsync(int theatreId);
    Task AddAsync(Screen screen);
    void Remove(Screen screen);
    Task SaveChangesAsync();
}

public interface ISeatRepository
{
    Task<List<Seat>> GetByScreenAsync(int screenId);
    Task<Seat?> GetByIdAsync(int id);
    Task<bool> ScreenExistsAsync(int screenId);
    Task<bool> CoordinateExistsAsync(int screenId, string rowName, int seatNumber, int? excludedSeatId = null);
    Task AddAsync(Seat seat);
    Task AddRangeAsync(IEnumerable<Seat> seats);
    void Remove(Seat seat);
    Task SaveChangesAsync();
}
