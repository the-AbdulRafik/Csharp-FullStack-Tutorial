using MovieTicketBookingSystem.DTOs.Movies;

namespace MovieTicketBookingSystem.Services.Interfaces;

public interface IMovieService
{
    Task<IReadOnlyList<MovieResponse>> GetAllAsync(int pageNumber, int pageSize, string? title);
    Task<MovieResponse?> GetByIdAsync(int id);
    Task<MovieResponse> CreateAsync(CreateMovieRequest request);
    Task<MovieResponse?> UpdateAsync(int id, UpdateMovieRequest request);
    Task<bool> DeleteAsync(int id);
}
