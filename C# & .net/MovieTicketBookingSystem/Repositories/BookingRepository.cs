using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Repositories;

public class BookingRepository(ApplicationDbContext db) : IBookingRepository
{
    public Task<List<Booking>> GetAsync(int? showId, BookingStatus? status) =>
        db.Bookings.AsNoTracking().Include(x => x.BookingSeats).ThenInclude(x => x.Seat).Where(x => (!showId.HasValue || x.ShowId == showId) && (!status.HasValue || x.Status == status)).OrderByDescending(x => x.BookingDate).ToListAsync();
    public Task<Booking?> GetByIdAsync(int id) => db.Bookings.Include(x => x.BookingSeats).ThenInclude(x => x.Seat).SingleOrDefaultAsync(x => x.BookingId == id);
    public Task<Show?> GetShowWithSeatsAsync(int showId) => db.Shows.Include(x => x.ShowSeats).ThenInclude(x => x.Seat).SingleOrDefaultAsync(x => x.ShowId == showId);
    public Task<List<ShowSeat>> GetShowSeatsAsync(int showId, IReadOnlyCollection<int> seatIds) => db.ShowSeats.Where(x => x.ShowId == showId && seatIds.Contains(x.SeatId)).ToListAsync();
    public Task AddAsync(Booking booking) => db.Bookings.AddAsync(booking).AsTask();
    public Task SaveChangesAsync() => db.SaveChangesAsync();
    public Task<IDbContextTransaction> BeginTransactionAsync() => db.Database.BeginTransactionAsync();
}
