using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Repositories;

public class MovieRepository(ApplicationDbContext db) : IMovieRepository
{
    public Task<List<Movie>> GetAllAsync(int pageNumber, int pageSize, string? title) =>
        db.Movies.AsNoTracking()
            .Where(x => string.IsNullOrWhiteSpace(title) || x.Title.Contains(title))
            .OrderBy(x => x.Title)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public Task<Movie?> GetByIdAsync(int id) => db.Movies.FindAsync(id).AsTask();
    public Task<bool> ExistsAsync(int id) => db.Movies.AnyAsync(x => x.MovieId == id);
    public Task AddAsync(Movie movie) => db.Movies.AddAsync(movie).AsTask();
    public void Remove(Movie movie) => db.Movies.Remove(movie);
    public Task SaveChangesAsync() => db.SaveChangesAsync();
}
