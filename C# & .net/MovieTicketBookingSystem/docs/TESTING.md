# Verification workflow

The API-focused verification sequence follows the developer guide:

1. Create a movie, theatre, screen, and seats.
2. Schedule a show and confirm its seat layout count matches the physical screen.
3. Create a booking for multiple available seats and confirm the server-calculated total.
4. Query booked seats.
5. Cancel the booking and confirm the booking remains `Cancelled` while seats return to `Available`.

The project is validated with:

```powershell
dotnet build
dotnet ef migrations list
dotnet run --urls http://localhost:5169
```

Version 1 intentionally does not claim simultaneous-booking concurrency safety. A future version must add conditional updates or an equivalent reservation strategy.
