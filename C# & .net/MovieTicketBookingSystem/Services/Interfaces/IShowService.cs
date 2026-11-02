using MovieTicketBookingSystem.DTOs.Shows;
using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Services.Interfaces;

public interface IShowService
{
    Task<List<ShowResponse>> GetAsync(DateTime? date, int? movieId, int? theatreId);
    Task<ShowResponse?> GetByIdAsync(int id);
    Task<ShowResponse> CreateAsync(CreateShowRequest request);
    Task<ShowResponse?> UpdateAsync(int id, UpdateShowRequest request);
    Task<bool> DeleteAsync(int id);
    Task<List<ShowSeatResponse>?> GetSeatsAsync(int showId, SeatStatus? status = null);
}
