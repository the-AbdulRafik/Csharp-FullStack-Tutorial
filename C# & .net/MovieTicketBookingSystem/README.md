# Movie Ticket Booking System

Version 1 is a backend-first ASP.NET Core Web API built with .NET 10, EF Core 10, and SQL Server. It follows the guide's controller -> service -> repository/data-access separation and exposes DTO-based REST contracts for a future Angular client.

## Run locally

1. Ensure SQL Server Express is running as `.\SQLEXPRESS`.
2. From this directory run `dotnet run`.
3. The application applies the included `InitialCreate` migration on startup.
4. Open `http://localhost:5169/openapi/v1.json` in development or use the included `.http` requests.

The development launch profile intentionally uses HTTP on port 5169. HTTPS redirection is not enabled because the profile does not define an HTTPS endpoint or certificate.

The connection string is in `appsettings.json`. Override it with the `ConnectionStrings__MovieTicketBooking` environment variable when using another SQL Server instance.

## Architecture

The application uses controller -> service interface -> service -> repository interface -> repository -> `ApplicationDbContext`. Each functional area has its own repository abstraction: movies, theatres, screens, seats, shows, and bookings. The [database ER diagram](./docs/database-er-diagram.eraser) can be pasted into Eraser, and the [requirements compliance checklist](./docs/requirements-compliance.md) maps the implementation to Version 1 of the developer guide.

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
