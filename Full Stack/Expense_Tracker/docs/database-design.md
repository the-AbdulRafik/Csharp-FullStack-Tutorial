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

These are planned only. Categories and Transactions entities, tables, and business migrations have not been implemented in Phase 0. The database is not created by the application during startup.
