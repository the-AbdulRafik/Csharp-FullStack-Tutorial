using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Repositories;

public class SeatRepository(ApplicationDbContext db) : ISeatRepository
{
    public Task<List<Seat>> GetByScreenAsync(int screenId) => db.Seats.AsNoTracking().Where(x => x.ScreenId == screenId).OrderBy(x => x.RowName).ThenBy(x => x.SeatNumber).ToListAsync();
    public Task<Seat?> GetByIdAsync(int id) => db.Seats.FindAsync(id).AsTask();
    public Task<bool> ScreenExistsAsync(int screenId) => db.Screens.AnyAsync(x => x.ScreenId == screenId);
    public Task<bool> CoordinateExistsAsync(int screenId, string rowName, int seatNumber, int? excludedSeatId = null) =>
        db.Seats.AnyAsync(x => x.ScreenId == screenId && x.RowName == rowName && x.SeatNumber == seatNumber && (!excludedSeatId.HasValue || x.SeatId != excludedSeatId));
    public Task AddAsync(Seat seat) => db.Seats.AddAsync(seat).AsTask();
    public Task AddRangeAsync(IEnumerable<Seat> seats) { db.Seats.AddRange(seats); return Task.CompletedTask; }
    public void Remove(Seat seat) => db.Seats.Remove(seat);
    public Task SaveChangesAsync() => db.SaveChangesAsync();
}
