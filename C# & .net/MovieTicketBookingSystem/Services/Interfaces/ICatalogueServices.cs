using MovieTicketBookingSystem.DTOs.Screens;
using MovieTicketBookingSystem.DTOs.Seats;
using MovieTicketBookingSystem.DTOs.Theatres;

namespace MovieTicketBookingSystem.Services.Interfaces;

public interface ITheatreService
{
    Task<List<TheatreResponse>> GetTheatresAsync(int pageNumber, int pageSize);
    Task<TheatreResponse?> GetTheatreAsync(int id);
    Task<TheatreResponse> CreateTheatreAsync(CreateTheatreRequest request);
    Task<TheatreResponse?> UpdateTheatreAsync(int id, UpdateTheatreRequest request);
    Task<bool> DeleteTheatreAsync(int id);
}

public interface IScreenService
{
    Task<List<ScreenResponse>> GetScreensAsync(int? theatreId);
    Task<ScreenResponse?> GetScreenAsync(int id);
    Task<ScreenResponse> CreateScreenAsync(CreateScreenRequest request);
    Task<ScreenResponse?> UpdateScreenAsync(int id, UpdateScreenRequest request);
    Task<bool> DeleteScreenAsync(int id);
}

public interface ISeatService
{
    Task<List<SeatResponse>> GetSeatsAsync(int screenId);
    Task<SeatResponse?> GetSeatAsync(int id);
    Task<SeatResponse> CreateSeatAsync(CreateSeatRequest request);
    Task<List<SeatResponse>> CreateSeatsAsync(BulkCreateSeatsRequest request);
    Task<SeatResponse?> UpdateSeatAsync(int id, UpdateSeatRequest request);
    Task<bool> DeleteSeatAsync(int id);
}
