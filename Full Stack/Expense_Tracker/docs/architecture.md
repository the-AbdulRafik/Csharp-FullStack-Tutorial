# Architecture

The project has one Angular frontend and one ASP.NET Core Web API backend.

## Frontend

Angular uses standalone components, `provideRouter`, and `provideHttpClient`. The application shell owns navigation and the API connection indicator. Dashboard, Transactions, Categories, and Reports are feature folders with lazy-loaded placeholder components.

## Backend

Controllers expose HTTP endpoints. Services contain application logic, such as the health status calculation. DTOs define response shapes. `ExpenseTrackerDbContext` is the EF Core boundary for future database work. The context is registered with dependency injection but has no entities or migrations in Phase 0.

Development requests from Angular at `http://localhost:4200` are allowed by the development CORS policy. The frontend keeps the API base URL in `src/environments/environment.ts`.

OpenAPI is mapped at `/openapi/v1.json`; Scalar provides an interactive view at `/scalar`. Global exceptions are converted to Problem Details and logged through the built-in structured logging system.
