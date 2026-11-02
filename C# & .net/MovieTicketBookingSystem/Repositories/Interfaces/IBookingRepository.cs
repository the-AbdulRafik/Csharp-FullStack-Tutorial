using MovieTicketBookingSystem.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace MovieTicketBookingSystem.Repositories.Interfaces;

public interface IBookingRepository
{
    Task<List<Booking>> GetAsync(int? showId, BookingStatus? status);
    Task<Booking?> GetByIdAsync(int id);
    Task<Show?> GetShowWithSeatsAsync(int showId);
    Task<List<ShowSeat>> GetShowSeatsAsync(int showId, IReadOnlyCollection<int> seatIds);
    Task AddAsync(Booking booking);
    Task SaveChangesAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
}
