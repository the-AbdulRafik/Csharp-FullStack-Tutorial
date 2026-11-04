using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Constants;
using MovieTicketBookingSystem.Data;

namespace MovieTicketBookingSystem.Authorization;

public sealed class BookingCancellationHandler(
    ApplicationDbContext db) : AuthorizationHandler<BookingCancellationRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        BookingCancellationRequirement requirement)
    {
        if (context.User.IsInRole(UserRoles.Admin) ||
            context.User.IsInRole(UserRoles.TheatreManager))
        {
            context.Succeed(requirement);
            return;
        }

        if (!context.User.IsInRole(UserRoles.Customer) ||
            !int.TryParse(context.Resource as string, out var bookingId))
            return;

        var authenticatedEmail = context.User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(authenticatedEmail))
            return;

        var bookingExists = await db.Bookings
            .AsNoTracking()
            .AnyAsync(booking =>
                booking.BookingId == bookingId &&
                booking.CustomerEmail == authenticatedEmail);

        if (bookingExists)
            context.Succeed(requirement);
    }
}
