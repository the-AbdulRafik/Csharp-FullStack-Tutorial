# Database Design

The API is configured for SQL Server LocalDB using the `ExpenseTrackerDbContext` and the `ExpenseTracker` connection string.

## Planned Categories table

- `Id`: integer primary key
- `Name`: required category name
- `Description`: optional description
- `CreatedAtUtc`: creation timestamp

## Planned Transactions table

- `Id`: integer primary key
- `Amount`: required decimal amount
- `Description`: optional description
- `TransactionDate`: date of the transaction
- `CategoryId`: foreign key to Categories
- `CreatedAtUtc`: creation timestamp

The existing `InitialCreate` migration implements these tables and was already applied to SQL Server Express. The database is not recreated or deleted during normal application startup.
