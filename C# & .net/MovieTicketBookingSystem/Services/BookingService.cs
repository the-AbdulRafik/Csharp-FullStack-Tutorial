using MovieTicketBookingSystem.DTOs.Bookings;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Services;

public class BookingService(IBookingRepository bookings) : IBookingService
{
    public async Task<BookingResponse> CreateAsync(CreateBookingRequest request)
    {
        var seatIds = request.SeatIds.Distinct().ToList();
        if (seatIds.Count != request.SeatIds.Count)
            throw new InvalidOperationException("SeatIds must be distinct.");
        var show = await bookings.GetShowWithSeatsAsync(request.ShowId)
            ?? throw new KeyNotFoundException("Show was not found.");
        var selected = show.ShowSeats.Where(x => seatIds.Contains(x.SeatId)).ToList();
        if (selected.Count != seatIds.Count)
            throw new InvalidOperationException("Every requested seat must belong to the show's screen.");
        if (selected.Any(x => x.Status != SeatStatus.Available))
            throw new InvalidOperationException("One or more requested seats are no longer available.");

        await using var transaction = await bookings.BeginTransactionAsync();
        var booking = new Booking
        {
            BookingReference = string.Empty,
            ShowId = show.ShowId,
            CustomerName = request.CustomerName.Trim(),
            CustomerEmail = request.CustomerEmail.Trim(),
            CustomerPhone = request.CustomerPhone.Trim(),
            BookingDate = DateTime.UtcNow,
            TotalAmount = show.TicketPrice * selected.Count,
            Status = BookingStatus.Confirmed,
            BookingSeats = selected.Select(x => new BookingSeat { SeatId = x.SeatId, Price = show.TicketPrice }).ToList()
        };
        selected.ForEach(x => x.Status = SeatStatus.Booked);
        await bookings.AddAsync(booking);
        await bookings.SaveChangesAsync();
        booking.BookingReference = $"MOV{booking.BookingId}";
        await bookings.SaveChangesAsync();
        await transaction.CommitAsync();
        return (await GetByIdAsync(booking.BookingId))!;
    }

    public async Task<List<BookingResponse>> GetAsync(int? showId, BookingStatus? status) =>
        (await bookings.GetAsync(showId, status)).Select(ToResponse).ToList();

    public async Task<BookingResponse?> GetByIdAsync(int id) =>
        (await bookings.GetByIdAsync(id)) is { } booking ? ToResponse(booking) : null;

    public Task<List<BookingResponse>> GetByShowAsync(int showId) => GetAsync(showId, null);

    public async Task<BookingResponse?> CancelAsync(int id)
    {
        var booking = await bookings.GetByIdAsync(id);
        if (booking is null) return null;
        if (booking.Status != BookingStatus.Confirmed)
            throw new InvalidOperationException("Only confirmed bookings can be cancelled.");

        await using var transaction = await bookings.BeginTransactionAsync();
        var seatIds = booking.BookingSeats.Select(x => x.SeatId).ToList();
        var showSeats = await bookings.GetShowSeatsAsync(booking.ShowId, seatIds);
        showSeats.ForEach(x => x.Status = SeatStatus.Available);
        booking.Status = BookingStatus.Cancelled;
        await bookings.SaveChangesAsync();
        await transaction.CommitAsync();
        return (await GetByIdAsync(booking.BookingId))!;
    }

    private static BookingResponse ToResponse(Booking booking) =>
        new(booking.BookingId, booking.BookingReference, booking.ShowId, booking.CustomerName,
            booking.CustomerEmail, booking.CustomerPhone, booking.BookingDate, booking.TotalAmount,
            booking.Status.ToString(), booking.BookingSeats.OrderBy(x => x.Seat.RowName)
                .ThenBy(x => x.Seat.SeatNumber).Select(x => x.Seat.RowName + x.Seat.SeatNumber).ToList());
}
