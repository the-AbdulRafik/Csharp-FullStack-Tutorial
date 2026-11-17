# ExpenseTracker

ExpenseTracker is a learning project foundation for building a full-stack expense application with Angular, ASP.NET Core, EF Core, and SQL Server.

## Technology stack

- Angular 20 standalone components, TypeScript, Router, HttpClient, and plain CSS
- ASP.NET Core 10 Web API with controllers
- Entity Framework Core 10 and SQL Server LocalDB configuration
- ASP.NET Core OpenAPI and Scalar.AspNetCore
- xUnit for backend tests

## Repository structure

```text
frontend/                 Angular application
backend/src/              ExpenseTracker.Api
backend/tests/            ExpenseTracker.Api.Tests
docs/                     Project documentation
```

## Prerequisites

- Node.js and npm
- .NET 10 SDK
- SQL Server LocalDB (or a compatible SQL Server instance)

## Run locally

Backend:

```powershell
cd backend
dotnet run --project src\ExpenseTracker.Api --launch-profile https
```

Frontend:

```powershell
cd frontend
npm start
```

Development URLs:

- API HTTPS: https://localhost:7045
- API HTTP: http://localhost:5090
- Scalar: https://localhost:7045/scalar
- OpenAPI: https://localhost:7045/openapi/v1.json
- Health: https://localhost:7045/api/health
- Angular: http://localhost:4200

## Tests

```powershell
cd backend
dotnet test
```

The Angular test script is available with `npm test`; it requires a local Chrome/Chromium installation for the default Karma launcher.

## Current status

Phase 0 is implemented: project structure, API health endpoint, EF Core configuration without business tables, Angular shell, API health integration, tests, and documentation. Expense and category features are intentionally not implemented.
