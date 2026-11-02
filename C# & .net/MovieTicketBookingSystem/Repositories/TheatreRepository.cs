using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Repositories;

public class TheatreRepository(ApplicationDbContext db) : ITheatreRepository
{
    public Task<List<Theatre>> GetAllAsync(int pageNumber, int pageSize) =>
        db.Theatres.AsNoTracking().OrderBy(x => x.Name).Skip((Math.Max(1, pageNumber) - 1) * Math.Clamp(pageSize, 1, 100)).Take(Math.Clamp(pageSize, 1, 100)).ToListAsync();
    public Task<Theatre?> GetByIdAsync(int id) => db.Theatres.FindAsync(id).AsTask();
    public Task AddAsync(Theatre theatre) => db.Theatres.AddAsync(theatre).AsTask();
    public void Remove(Theatre theatre) => db.Theatres.Remove(theatre);
    public Task SaveChangesAsync() => db.SaveChangesAsync();
}
