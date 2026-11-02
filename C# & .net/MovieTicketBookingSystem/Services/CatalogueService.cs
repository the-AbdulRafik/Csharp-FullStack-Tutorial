using MovieTicketBookingSystem.DTOs.Screens;
using MovieTicketBookingSystem.DTOs.Seats;
using MovieTicketBookingSystem.DTOs.Theatres;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Services;

public class CatalogueService(
    ITheatreRepository theatres,
    IScreenRepository screens,
    ISeatRepository seats) : ITheatreService, IScreenService, ISeatService
{
    public async Task<List<TheatreResponse>> GetTheatresAsync(int pageNumber, int pageSize) =>
        (await theatres.GetAllAsync(pageNumber, pageSize)).Select(ToTheatreResponse).ToList();

    public async Task<TheatreResponse?> GetTheatreAsync(int id) =>
        (await theatres.GetByIdAsync(id)) is { } theatre ? ToTheatreResponse(theatre) : null;

    public async Task<TheatreResponse> CreateTheatreAsync(CreateTheatreRequest request)
    {
        var theatre = new Theatre { Name = request.Name.Trim(), Location = request.Location.Trim() };
        await theatres.AddAsync(theatre);
        await theatres.SaveChangesAsync();
        return ToTheatreResponse(theatre);
    }

    public async Task<TheatreResponse?> UpdateTheatreAsync(int id, UpdateTheatreRequest request)
    {
        var theatre = await theatres.GetByIdAsync(id);
        if (theatre is null) return null;
        theatre.Name = request.Name.Trim();
        theatre.Location = request.Location.Trim();
        await theatres.SaveChangesAsync();
        return ToTheatreResponse(theatre);
    }

    public async Task<bool> DeleteTheatreAsync(int id)
    {
        var theatre = await theatres.GetByIdAsync(id);
        if (theatre is null) return false;
        theatres.Remove(theatre);
        await theatres.SaveChangesAsync();
        return true;
    }

    public async Task<List<ScreenResponse>> GetScreensAsync(int? theatreId) =>
        (await screens.GetAllAsync(theatreId)).Select(ToScreenResponse).ToList();

    public async Task<ScreenResponse?> GetScreenAsync(int id) =>
        (await screens.GetByIdAsync(id)) is { } screen ? ToScreenResponse(screen) : null;

    public async Task<ScreenResponse> CreateScreenAsync(CreateScreenRequest request)
    {
        if (!await screens.TheatreExistsAsync(request.TheatreId))
            throw new KeyNotFoundException("Theatre was not found.");
        var screen = new Screen { TheatreId = request.TheatreId, ScreenName = request.ScreenName.Trim() };
        await screens.AddAsync(screen);
        await screens.SaveChangesAsync();
        return ToScreenResponse(screen);
    }

    public async Task<ScreenResponse?> UpdateScreenAsync(int id, UpdateScreenRequest request)
    {
        var screen = await screens.GetByIdAsync(id);
        if (screen is null) return null;
        if (!await screens.TheatreExistsAsync(request.TheatreId))
            throw new KeyNotFoundException("Theatre was not found.");
        screen.TheatreId = request.TheatreId;
        screen.ScreenName = request.ScreenName.Trim();
        await screens.SaveChangesAsync();
        return ToScreenResponse(screen);
    }

    public async Task<bool> DeleteScreenAsync(int id)
    {
        var screen = await screens.GetByIdAsync(id);
        if (screen is null) return false;
        screens.Remove(screen);
        await screens.SaveChangesAsync();
        return true;
    }

    public async Task<List<SeatResponse>> GetSeatsAsync(int screenId) =>
        (await seats.GetByScreenAsync(screenId)).Select(ToSeatResponse).ToList();

    public async Task<SeatResponse?> GetSeatAsync(int id) =>
        (await seats.GetByIdAsync(id)) is { } seat ? ToSeatResponse(seat) : null;

    public async Task<SeatResponse> CreateSeatAsync(CreateSeatRequest request)
    {
        var rowName = request.RowName.Trim().ToUpperInvariant();
        if (!await seats.ScreenExistsAsync(request.ScreenId))
            throw new KeyNotFoundException("Screen was not found.");
        if (await seats.CoordinateExistsAsync(request.ScreenId, rowName, request.SeatNumber))
            throw new InvalidOperationException("A seat with the same coordinate already exists.");
        var seat = new Seat { ScreenId = request.ScreenId, RowName = rowName, SeatNumber = request.SeatNumber, SeatType = request.SeatType.Trim() };
        await seats.AddAsync(seat);
        await seats.SaveChangesAsync();
        return ToSeatResponse(seat);
    }

    public async Task<List<SeatResponse>> CreateSeatsAsync(BulkCreateSeatsRequest request)
    {
        if (!await seats.ScreenExistsAsync(request.ScreenId))
            throw new KeyNotFoundException("Screen was not found.");
        var definitions = request.Seats.Select(x => new
        {
            Row = x.RowName.Trim().ToUpperInvariant(),
            x.SeatNumber,
            Type = x.SeatType.Trim()
        }).ToList();
        if (definitions.GroupBy(x => new { x.Row, x.SeatNumber }).Any(x => x.Count() > 1))
            throw new InvalidOperationException("Duplicate seat coordinates are not allowed.");
        foreach (var definition in definitions)
        {
            if (await seats.CoordinateExistsAsync(request.ScreenId, definition.Row, definition.SeatNumber))
                throw new InvalidOperationException("Duplicate seat coordinates are not allowed.");
        }
        var newSeats = definitions.Select(x => new Seat
        {
            ScreenId = request.ScreenId,
            RowName = x.Row,
            SeatNumber = x.SeatNumber,
            SeatType = x.Type
        }).ToList();
        await seats.AddRangeAsync(newSeats);
        await seats.SaveChangesAsync();
        return newSeats.Select(ToSeatResponse).ToList();
    }

    public async Task<SeatResponse?> UpdateSeatAsync(int id, UpdateSeatRequest request)
    {
        var seat = await seats.GetByIdAsync(id);
        if (seat is null) return null;
        var rowName = request.RowName.Trim().ToUpperInvariant();
        if (!await seats.ScreenExistsAsync(request.ScreenId))
            throw new KeyNotFoundException("Screen was not found.");
        if (await seats.CoordinateExistsAsync(request.ScreenId, rowName, request.SeatNumber, id))
            throw new InvalidOperationException("Duplicate seat coordinates are not allowed.");
        seat.ScreenId = request.ScreenId;
        seat.RowName = rowName;
        seat.SeatNumber = request.SeatNumber;
        seat.SeatType = request.SeatType.Trim();
        await seats.SaveChangesAsync();
        return ToSeatResponse(seat);
    }

    public async Task<bool> DeleteSeatAsync(int id)
    {
        var seat = await seats.GetByIdAsync(id);
        if (seat is null) return false;
        seats.Remove(seat);
        await seats.SaveChangesAsync();
        return true;
    }

    private static TheatreResponse ToTheatreResponse(Theatre x) => new(x.TheatreId, x.Name, x.Location);
    private static ScreenResponse ToScreenResponse(Screen x) => new(x.ScreenId, x.TheatreId, x.ScreenName);
    private static SeatResponse ToSeatResponse(Seat x) => new(x.SeatId, x.ScreenId, x.RowName, x.SeatNumber, x.SeatType, x.RowName + x.SeatNumber);
}
