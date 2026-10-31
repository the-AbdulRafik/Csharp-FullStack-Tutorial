using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.DTOs.Screens;
using MovieTicketBookingSystem.DTOs.Seats;
using MovieTicketBookingSystem.DTOs.Theatres;
using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Services;

public class CatalogueService(ApplicationDbContext db)
{
    public async Task<List<TheatreResponse>> GetTheatresAsync(int pageNumber, int pageSize) =>
        await db.Theatres.AsNoTracking().OrderBy(x => x.Name)
            .Skip((Math.Max(1, pageNumber) - 1) * Math.Clamp(pageSize, 1, 100))
            .Take(Math.Clamp(pageSize, 1, 100)).Select(x => new TheatreResponse(x.TheatreId, x.Name, x.Location)).ToListAsync();

    public async Task<TheatreResponse?> GetTheatreAsync(int id) =>
        await db.Theatres.AsNoTracking().Where(x => x.TheatreId == id)
            .Select(x => new TheatreResponse(x.TheatreId, x.Name, x.Location)).FirstOrDefaultAsync();

    public async Task<TheatreResponse> CreateTheatreAsync(CreateTheatreRequest request)
    {
        var theatre = new Theatre { Name = request.Name.Trim(), Location = request.Location.Trim() };
        db.Theatres.Add(theatre); await db.SaveChangesAsync();
        return new(theatre.TheatreId, theatre.Name, theatre.Location);
    }

    public async Task<TheatreResponse?> UpdateTheatreAsync(int id, UpdateTheatreRequest request)
    {
        var theatre = await db.Theatres.FindAsync(id); if (theatre is null) return null;
        theatre.Name = request.Name.Trim(); theatre.Location = request.Location.Trim(); await db.SaveChangesAsync();
        return new(theatre.TheatreId, theatre.Name, theatre.Location);
    }

    public async Task<bool> DeleteTheatreAsync(int id)
    {
        var theatre = await db.Theatres.FindAsync(id); if (theatre is null) return false;
        db.Theatres.Remove(theatre); await db.SaveChangesAsync(); return true;
    }

    public async Task<List<ScreenResponse>> GetScreensAsync(int? theatreId) =>
        await db.Screens.AsNoTracking().Where(x => theatreId == null || x.TheatreId == theatreId).OrderBy(x => x.ScreenName)
            .Select(x => new ScreenResponse(x.ScreenId, x.TheatreId, x.ScreenName)).ToListAsync();

    public async Task<ScreenResponse?> GetScreenAsync(int id) =>
        await db.Screens.AsNoTracking().Where(x => x.ScreenId == id)
            .Select(x => new ScreenResponse(x.ScreenId, x.TheatreId, x.ScreenName)).FirstOrDefaultAsync();

    public async Task<ScreenResponse> CreateScreenAsync(CreateScreenRequest request)
    {
        if (!await db.Theatres.AnyAsync(x => x.TheatreId == request.TheatreId)) throw new KeyNotFoundException("Theatre was not found.");
        var screen = new Screen { TheatreId = request.TheatreId, ScreenName = request.ScreenName.Trim() };
        db.Screens.Add(screen); await db.SaveChangesAsync();
        return new(screen.ScreenId, screen.TheatreId, screen.ScreenName);
    }

    public async Task<ScreenResponse?> UpdateScreenAsync(int id, UpdateScreenRequest request)
    {
        var screen = await db.Screens.FindAsync(id); if (screen is null) return null;
        if (!await db.Theatres.AnyAsync(x => x.TheatreId == request.TheatreId)) throw new KeyNotFoundException("Theatre was not found.");
        screen.TheatreId = request.TheatreId; screen.ScreenName = request.ScreenName.Trim(); await db.SaveChangesAsync();
        return new(screen.ScreenId, screen.TheatreId, screen.ScreenName);
    }

    public async Task<bool> DeleteScreenAsync(int id)
    {
        var screen = await db.Screens.FindAsync(id); if (screen is null) return false;
        db.Screens.Remove(screen); await db.SaveChangesAsync(); return true;
    }

    public async Task<List<SeatResponse>> GetSeatsAsync(int screenId) =>
        await db.Seats.AsNoTracking().Where(x => x.ScreenId == screenId).OrderBy(x => x.RowName).ThenBy(x => x.SeatNumber)
            .Select(x => new SeatResponse(x.SeatId, x.ScreenId, x.RowName, x.SeatNumber, x.SeatType, x.RowName + x.SeatNumber)).ToListAsync();

    public async Task<SeatResponse?> GetSeatAsync(int id) =>
        await db.Seats.AsNoTracking().Where(x => x.SeatId == id)
            .Select(x => new SeatResponse(x.SeatId, x.ScreenId, x.RowName, x.SeatNumber, x.SeatType, x.RowName + x.SeatNumber)).FirstOrDefaultAsync();

    public async Task<SeatResponse> CreateSeatAsync(CreateSeatRequest request)
    {
        if (!await db.Screens.AnyAsync(x => x.ScreenId == request.ScreenId)) throw new KeyNotFoundException("Screen was not found.");
        if (await db.Seats.AnyAsync(x => x.ScreenId == request.ScreenId && x.RowName == request.RowName && x.SeatNumber == request.SeatNumber))
            throw new InvalidOperationException("A seat with the same coordinate already exists.");
        var seat = new Seat { ScreenId = request.ScreenId, RowName = request.RowName.Trim().ToUpperInvariant(), SeatNumber = request.SeatNumber, SeatType = request.SeatType.Trim() };
        db.Seats.Add(seat); await db.SaveChangesAsync();
        return new(seat.SeatId, seat.ScreenId, seat.RowName, seat.SeatNumber, seat.SeatType, seat.RowName + seat.SeatNumber);
    }

    public async Task<List<SeatResponse>> CreateSeatsAsync(BulkCreateSeatsRequest request)
    {
        if (!await db.Screens.AnyAsync(x => x.ScreenId == request.ScreenId)) throw new KeyNotFoundException("Screen was not found.");
        var definitions = request.Seats.Select(x => new { Row = x.RowName.Trim().ToUpperInvariant(), x.SeatNumber, Type = x.SeatType.Trim() }).ToList();
        var existingCoordinates = await db.Seats.Where(x => x.ScreenId == request.ScreenId)
            .Select(x => new { x.RowName, x.SeatNumber }).ToListAsync();
        if (definitions.GroupBy(x => new { x.Row, x.SeatNumber }).Any(x => x.Count() > 1) ||
            definitions.Any(d => existingCoordinates.Any(x => x.RowName == d.Row && x.SeatNumber == d.SeatNumber)))
            throw new InvalidOperationException("Duplicate seat coordinates are not allowed.");
        var seats = definitions.Select(x => new Seat { ScreenId = request.ScreenId, RowName = x.Row, SeatNumber = x.SeatNumber, SeatType = x.Type }).ToList();
        db.Seats.AddRange(seats); await db.SaveChangesAsync();
        return seats.Select(x => new SeatResponse(x.SeatId, x.ScreenId, x.RowName, x.SeatNumber, x.SeatType, x.RowName + x.SeatNumber)).ToList();
    }

    public async Task<SeatResponse?> UpdateSeatAsync(int id, UpdateSeatRequest request)
    {
        var seat = await db.Seats.FindAsync(id); if (seat is null) return null;
        if (seat.ScreenId != request.ScreenId && !await db.Screens.AnyAsync(x => x.ScreenId == request.ScreenId)) throw new KeyNotFoundException("Screen was not found.");
        if (await db.Seats.AnyAsync(x => x.SeatId != id && x.ScreenId == request.ScreenId && x.RowName == request.RowName && x.SeatNumber == request.SeatNumber))
            throw new InvalidOperationException("Duplicate seat coordinates are not allowed.");
        seat.ScreenId = request.ScreenId; seat.RowName = request.RowName.Trim().ToUpperInvariant(); seat.SeatNumber = request.SeatNumber; seat.SeatType = request.SeatType.Trim();
        await db.SaveChangesAsync(); return new(seat.SeatId, seat.ScreenId, seat.RowName, seat.SeatNumber, seat.SeatType, seat.RowName + seat.SeatNumber);
    }

    public async Task<bool> DeleteSeatAsync(int id)
    {
        var seat = await db.Seats.FindAsync(id); if (seat is null) return false;
        db.Seats.Remove(seat); await db.SaveChangesAsync(); return true;
    }
}
