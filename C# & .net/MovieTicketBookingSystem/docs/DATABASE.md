# Database setup

The application targets `MovieTicketBookingDB` on local SQL Server Express:

```text
Server=.\SQLEXPRESS;Database=MovieTicketBookingDB;Trusted_Connection=True;TrustServerCertificate=True
```

The eight tables are created by the `InitialCreate` EF Core migration. Startup calls `Database.Migrate()` so a fresh local database is created automatically. For explicit migration operations:

```powershell
dotnet ef database update
dotnet ef migrations script
```

Foreign keys use restrictive deletion for catalogue and booking history. A show owns its generated `ShowSeat` rows. Unique indexes protect screen names within a theatre, physical seat coordinates within a screen, show-seat pairs, and booking references.
