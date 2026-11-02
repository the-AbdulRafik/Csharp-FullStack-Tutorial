using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Repositories;

public class ScreenRepository(ApplicationDbContext db) : IScreenRepository
{
    public Task<List<Screen>> GetAllAsync(int? theatreId) => db.Screens.AsNoTracking().Where(x => theatreId == null || x.TheatreId == theatreId).OrderBy(x => x.ScreenName).ToListAsync();
    public Task<Screen?> GetByIdAsync(int id) => db.Screens.FindAsync(id).AsTask();
    public Task<bool> TheatreExistsAsync(int theatreId) => db.Theatres.AnyAsync(x => x.TheatreId == theatreId);
    public Task AddAsync(Screen screen) => db.Screens.AddAsync(screen).AsTask();
    public void Remove(Screen screen) => db.Screens.Remove(screen);
    public Task SaveChangesAsync() => db.SaveChangesAsync();
}
