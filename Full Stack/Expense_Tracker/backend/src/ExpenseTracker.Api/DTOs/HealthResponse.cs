namespace ExpenseTracker.Api.DTOs;

public sealed record HealthResponse(
    string Status,
    string Service,
    DateTimeOffset UtcTime);
