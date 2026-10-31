using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Repositories.Interfaces;

public interface IMovieRepository
{
    Task<List<Movie>> GetAllAsync(int pageNumber, int pageSize, string? title);
    Task<Movie?> GetByIdAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task AddAsync(Movie movie);
    void Remove(Movie movie);
    Task SaveChangesAsync();
}
