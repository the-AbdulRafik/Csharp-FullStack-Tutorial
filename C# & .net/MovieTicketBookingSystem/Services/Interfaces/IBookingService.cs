using MovieTicketBookingSystem.DTOs.Bookings;
using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Services.Interfaces;

public interface IBookingService
{
    Task<BookingResponse> CreateAsync(CreateBookingRequest request);
    Task<List<BookingResponse>> GetAsync(int? showId, BookingStatus? status);
    Task<BookingResponse?> GetByIdAsync(int id);
    Task<List<BookingResponse>> GetByShowAsync(int showId);
    Task<BookingResponse?> CancelAsync(int id);
}
