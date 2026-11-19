# Setup Guide

## Backend

From the project root:

```powershell
cd backend
dotnet restore
dotnet build
dotnet test
dotnet run --project src\ExpenseTracker.Api --launch-profile https
```

The HTTPS API uses `https://localhost:7045` and the HTTP fallback uses `http://localhost:5090` with the generated launch settings. Open `https://localhost:7045/scalar` for Scalar.

## Frontend

```powershell
cd frontend
npm install
npm run build
npm start
```

Open `http://localhost:4200`. The development environment calls `https://localhost:7045/api/health`. Accept the local development certificate if the browser prompts for it.

## Database

Install SQL Server LocalDB separately if it is not already available. Phase 0 only configures the connection string and DbContext; it does not create the database or run migrations.

## Authentication setup

Development JWT settings remain outside source control:

```powershell
cd backend\src\ExpenseTracker.Api
dotnet user-secrets set "Jwt:SigningKey" "<development-signing-key>"
dotnet user-secrets set "Jwt:Issuer" "ExpenseTracker.Api"
dotnet user-secrets set "Jwt:Audience" "ExpenseTracker.Angular"
```

Apply the additive Identity migration without resetting the database:

```powershell
cd backend
dotnet ef database update --project src\ExpenseTracker.Api --startup-project src\ExpenseTracker.Api
```
