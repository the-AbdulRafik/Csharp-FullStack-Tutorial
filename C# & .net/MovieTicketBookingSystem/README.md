# Movie Ticket Booking System

Version 1 is a backend-first ASP.NET Core Web API built with .NET 10, EF Core 10, and SQL Server. It follows the guide's controller -> service -> repository/data-access separation and exposes DTO-based REST contracts for a future Angular client.

## Run locally

1. Ensure SQL Server Express is running as `.\SQLEXPRESS`.
2. From this directory run `dotnet run`.
3. The application applies the included `InitialCreate` migration on startup.
4. Open `/openapi/v1.json` in development or use the included `.http` requests.

The connection string is in `appsettings.json`. Override it with the `ConnectionStrings__MovieTicketBooking` environment variable when using another SQL Server instance.

## Endpoints

- `movies`: CRUD, pagination, and title search.
- `theatres`: CRUD and pagination.
- `screens`: CRUD and theatre filtering.
- `seats`: individual/bulk setup and screen filtering.
- `shows`: scheduling, overlap validation, filters, and per-show seat status.
- `bookings`: transactional booking, retrieval, and cancellation.

Creating a show creates one available `ShowSeat` record for every physical seat on its screen. Booking calculates the total from the server-side show price, marks those records booked, and preserves booking history when cancellation releases them.

## Version 1 boundaries

This learning implementation deliberately has no authentication, authorization, payment processing, temporary holds, email notifications, Angular frontend, or advanced concurrency protection. The availability check and transaction are not a complete solution for simultaneous requests booking the same seat.
