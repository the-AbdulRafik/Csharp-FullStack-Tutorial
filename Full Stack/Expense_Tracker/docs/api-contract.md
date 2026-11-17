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
