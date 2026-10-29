# Code Explanation

## Models

The files in `Models` are simple C# classes that represent database rows:

- `Branch` represents a library branch.
- `Member` represents a library member and their home branch.
- `Book` represents a library book and its lending rules.
- `Loan` represents a member borrowing a book.
- `AuditLog` represents an important action recorded by the application.

Models do not open database connections or execute SQL.

## Data and Fluent API

`Data/LibraryDbContext.cs` is the EF Core unit of work. It exposes `DbSet` properties for the five database tables.
`OnModelCreating` uses Fluent API to configure keys, lengths, required fields, defaults, precision, check constraints, foreign keys, delete behavior, and the filtered unique index for active loans.
`LibraryDbContextFactory` allows the EF CLI to create the context while generating migrations.

The old ADO.NET helper is no longer used for database operations. EF Core creates and manages connections through the configured SQL Server provider.

## Repositories

Repositories own EF Core queries and persistence operations:

- `BranchRepository` performs branch inserts, reads, updates, and deactivation.
- `MemberRepository` performs member inserts, searches, branch filtering, updates, and deactivation.
- `BookRepository` performs book inserts, searches, branch filtering, updates, and deactivation.
- `LoanRepository` performs issue, history, return/lost status changes, and report queries.
- `AuditLogRepository` reads recent audit records.

`AsNoTracking` is used for read-only results. `SaveChangesAsync` persists changes, and LINQ queries are translated into SQL by EF Core.

## Services

Services are the business-facing layer used by the console:

- `BranchService` validates branch input.
- `MemberService` validates names, email, age, and branch IDs.
- `BookService` validates title, author, ISBN, loan days, and fees.
- `LoanService` validates loan IDs and exposes loan/report operations.
- `AuditLogService` exposes recent audit records.

The services keep menu code from containing database rules.

## Program.cs

`Program.cs` handles only console navigation and input:

1. It creates repository and service objects.
2. It displays the main menu.
3. It opens a submenu for each feature area.
4. It reads values from the user.
5. It calls a service method.
6. It displays the result or a clear error message.

SQL statements do not belong in `Program.cs`.

## Dependency injection

`Program.cs` registers the `LibraryDbContext`, repositories, and services with `ServiceCollection`.
The context is scoped, so one console application scope owns the context used by the services.
The console resolves services from the DI scope instead of constructing repositories manually.

## Database design

The SQL script creates five tables:

- `Branches` has many members and books.
- `Members` and `Books` are connected through `Loans`.
- `AuditLogs` records important actions without a foreign key because it can refer to multiple entity types.
- The filtered unique index prevents duplicate active loans for the same member and book.
