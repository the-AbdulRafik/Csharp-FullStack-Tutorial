# API Contract

## GET `/api/health`

Returns HTTP 200 when the API is running.

Example response:

```json
{
  "status": "Healthy",
  "service": "ExpenseTracker API",
  "utcTime": "2026-01-01T00:00:00+00:00"
}
```

The exact `utcTime` value is generated at request time.

## POST `/api/auth/register`

Creates an ASP.NET Core Identity user.

Request:

```json
{
  "email": "user@example.com",
  "password": "Strong123!"
}
```

Success status: `201 Created`.

Duplicate email: `409 Conflict`.

Invalid or weak password: `400 Bad Request`.

## POST `/api/auth/login`

Verifies Identity credentials and returns a short-lived JWT.

Success status: `200 OK`.

Invalid credentials: `401 Unauthorized`.

The response includes `token`, `expiresAt`, and the authenticated `user` with `id` and `email`.

## GET `/api/auth/me`

Requires a valid `Authorization: Bearer <token>` header.

The response identifies the user from JWT claims; it does not accept a user ID from the request body or query string.
