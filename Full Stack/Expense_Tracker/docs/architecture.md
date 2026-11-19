# Architecture

The project has one Angular frontend and one ASP.NET Core Web API backend.

## Frontend

Angular uses standalone components, `provideRouter`, and `provideHttpClient`. The application shell owns navigation and the API connection indicator. Dashboard, Transactions, Categories, and Reports are feature folders with lazy-loaded placeholder components.

## Backend

Controllers expose HTTP endpoints. Services contain application logic, such as health and authentication. DTOs define request and response shapes. `ExpenseTrackerDbContext` derives from `IdentityDbContext<ApplicationUser>` and is the EF Core boundary for both Identity and application data.

Development requests from Angular at `http://localhost:4200` are allowed by the development CORS policy. The frontend keeps the API base URL in `src/environments/environment.ts`.

OpenAPI is mapped at `/openapi/v1.json`; Scalar provides an interactive view at `/scalar`. Global exceptions are converted to Problem Details and logged through the built-in structured logging system.

## Authentication flow

`ApplicationUser` is the Identity user type. `AuthService` uses `UserManager` for password hashing and credentials, then creates a short-lived JWT with user ID and email claims. `AuthController` maps registration, login, and the authenticated `/api/auth/me` request.

Angular uses `AuthService`, `authInterceptor`, and `authGuard`. The interceptor attaches the token, the guard protects routes, and 401 responses clear the local session and redirect to login.
