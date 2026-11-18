# ExpenseTracker

ExpenseTracker is a personal expense tracking MVP built with Angular, ASP.NET Core, EF Core, and SQL Server. It tracks INR income and expenses, categories, dashboard totals, and reports.

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

The MVP includes category CRUD, transaction CRUD and filtering, real database dashboard summaries, monthly reports, category breakdowns, a responsive Angular interface, OpenAPI, Scalar, CORS, and SQL Server integration.
