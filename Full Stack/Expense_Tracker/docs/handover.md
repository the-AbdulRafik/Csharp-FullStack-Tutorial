# Technical Handover

## Architecture

Angular is a standalone-component browser application. ASP.NET Core is a single Web API application with controllers, services, DTOs, EF Core, and SQL Server. Angular calls ASP.NET Core over HTTP/JSON, and the API stores data through `ExpenseTrackerDbContext`.

## Angular routing and components

`app.routes.ts` contains lazy routes for Dashboard, Transactions, Categories, and Reports. `app.ts` is the application shell and owns navigation and API health display. Feature components are standalone and loaded only when their route is opened.

## Angular services and HttpClient

Feature services in `src/app/core/services` inject Angular `HttpClient` and isolate API URLs and request shapes. `app.config.ts` registers `provideHttpClient()` and `provideRouter()`. Components subscribe to service observables and manage loading, success, and error states.

## ASP.NET Core controllers

Controllers in `Controllers` translate HTTP routes into service calls. Categories, transactions, dashboard, and reports use `GET`, `POST`, `PUT`, and `DELETE` appropriately. DTO records/classes define request and response data instead of exposing EF entities.

## Services and dependency injection

`CategoryService`, `TransactionService`, `DashboardService`, and `ReportService` contain business logic and database queries. `Program.cs` registers them with the built-in ASP.NET Core service container. Controllers receive services through constructor injection.

## EF Core and SQL Server

`ExpenseTrackerDbContext` maps `Category` and `Transaction` sets and applies entity configurations. SQL Server Express is addressed as `localhost\SQLEXPRESS` with database `ExpenseTrackerDb`. `DateOnly` is used for transaction dates and `decimal(18,2)` for money.

## Migrations and relationships

The existing `InitialCreate` migration creates Categories and Transactions. `Category` is one side and `Transaction` is the many side through `CategoryId`. Deleting a category with transactions is blocked because the relationship uses `DeleteBehavior.Restrict`.

## Validation and errors

DTO data annotations validate required fields and lengths. Service logic checks duplicate names, amounts, category existence, category/type matching, date ranges, and referenced categories. `GlobalExceptionHandler` converts domain-style exceptions and EF errors to Problem Details with 400, 404, 409, or 500 status codes.

## Dashboard and reports

Dashboard totals and category summaries are calculated in `DashboardService` from SQL Server rows. Report totals, monthly summaries, and category summaries are calculated by `ReportService`. The frontend does not calculate financial totals from partial or filtered data.

## Why this architecture

The design is intentionally simple: controllers handle HTTP, services own logic, DTOs isolate API contracts, and EF Core owns persistence. It avoids generic repositories and extra framework abstractions that would obscure the learning path.

## Limitations

Authentication is not included. The MVP has no server-side pagination metadata, export, recurring transactions, or automated test suite. Financial totals use the complete or requested date range in the backend.
