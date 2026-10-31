using MovieTicketBookingSystem.DTOs.Movies;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Services;

public class MovieService(IMovieRepository repository) : IMovieService
{
    public async Task<IReadOnlyList<MovieResponse>> GetAllAsync(int pageNumber, int pageSize, string? title)
    {
        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Clamp(pageSize, 1, 100);
        var movies = await repository.GetAllAsync(pageNumber, pageSize, title);
        return movies.Select(ToResponse).ToList();
    }

    public async Task<MovieResponse?> GetByIdAsync(int id) =>
        (await repository.GetByIdAsync(id)) is { } movie ? ToResponse(movie) : null;

    public async Task<MovieResponse> CreateAsync(CreateMovieRequest request)
    {
        var movie = new Movie
        {
            Title = request.Title.Trim(),
            Description = request.Description?.Trim(),
            DurationMinutes = request.DurationMinutes,
            ReleaseDate = request.ReleaseDate,
            Language = request.Language.Trim(),
            Genre = request.Genre.Trim()
        };
        await repository.AddAsync(movie);
        await repository.SaveChangesAsync();
        return ToResponse(movie);
    }

    public async Task<MovieResponse?> UpdateAsync(int id, UpdateMovieRequest request)
    {
        var movie = await repository.GetByIdAsync(id);
        if (movie is null) return null;
        movie.Title = request.Title.Trim();
        movie.Description = request.Description?.Trim();
        movie.DurationMinutes = request.DurationMinutes;
        movie.ReleaseDate = request.ReleaseDate;
        movie.Language = request.Language.Trim();
        movie.Genre = request.Genre.Trim();
        await repository.SaveChangesAsync();
        return ToResponse(movie);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var movie = await repository.GetByIdAsync(id);
        if (movie is null) return false;
        repository.Remove(movie);
        await repository.SaveChangesAsync();
        return true;
    }

    private static MovieResponse ToResponse(Movie movie) =>
        new(movie.MovieId, movie.Title, movie.Description, movie.DurationMinutes, movie.ReleaseDate, movie.Language, movie.Genre);
}
