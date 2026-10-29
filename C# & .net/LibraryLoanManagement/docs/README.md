# Library Loan Management

This project is a .NET 10 console application using ADO.NET and SQL Server Express.

## Setup

1. Confirm SQL Server Express is available as `.\SQLEXPRESS`.
2. Run `dotnet restore`.
3. Run `dotnet ef database update`.
4. Run `dotnet run`.

The application also calls `Database.MigrateAsync()` at startup, so pending migrations are applied automatically.
The connection string is intentionally kept in the composition root because this is a learning project.

## EF Core workflow

- `Data/LibraryDbContext.cs` contains the Fluent API model configuration.
- `Migrations/` contains the generated database history.
- Create a new migration with `dotnet ef migrations add MigrationName`.
- Apply migrations with `dotnet ef database update`.

## Documentation

- [Console features](./console-features.md) records the user-facing menu.
- [Code explanation](./code-explanation.md) explains the role of each project layer.
