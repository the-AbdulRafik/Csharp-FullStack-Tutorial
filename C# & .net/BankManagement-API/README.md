# Bank Management API

A small beginner-friendly ASP.NET Core Web API for practising bank account CRUD and balance operations.

This project uses an **in-memory store**, so no SQL Server or database setup is required. Data is reset whenever the application restarts. Swagger is available at `/swagger`.

## Run the project

```powershell
dotnet run
```

Open the URL printed by .NET and add `/swagger` to it, for example:

```text
http://localhost:5000/swagger
```

## Endpoints

| Method | URL | Description |
| --- | --- | --- |
| POST | `/api/accounts` | Create an account |
| GET | `/api/accounts` | List all accounts |
| GET | `/api/accounts/{id}` | Get one account |
| GET | `/api/accounts/search?accountType=Savings` | Filter by account type |
| GET | `/api/accounts/search?accountHolderName=Sample` | Search by holder name |
| PUT | `/api/accounts/{id}` | Update holder name and account type |
| DELETE | `/api/accounts/{id}` | Delete an account |
| POST | `/api/accounts/{id}/deposit` | Deposit money |
| POST | `/api/accounts/{id}/withdraw` | Withdraw money |

## Example requests

Create an account:

```json
{
  "accountHolderName": "Kavitha Rao",
  "accountNumber": "AC10001",
  "balance": 1000.00,
  "accountType": "Savings"
}
```

Deposit or withdraw:

```json
{
  "amount": 100.00
}
```

## Validation

- Account holder name, account number, and account type are required.
- Account numbers are unique, ignoring letter case. A duplicate returns `409 Conflict`.
- Opening balances cannot be negative.
- Deposit and withdrawal amounts must be greater than zero.
- A withdrawal greater than the current balance returns `400 Bad Request`.
- Updating an account changes only the holder name and account type; balance is changed only through deposit or withdrawal.
- Missing account IDs return `404 Not Found`.

The API includes one sample account (`AC10000`) at startup so the list, search, deposit, and withdrawal endpoints can be tried immediately.

## Sample frontend

The `Sample_Frontend` folder contains a dependency-free HTML/CSS/JavaScript client. It uses the browser `fetch` API and JSON requests for account listing, creation, deletion, deposits, and withdrawals.

1. Start the API with `dotnet run`.
2. Open `Sample_Frontend/index.html` directly or through a local static file server.
3. The client expects the API at `http://localhost:5031` by default. Change `API_BASE_URL` in `Sample_Frontend/script.js` if the API uses another port.
