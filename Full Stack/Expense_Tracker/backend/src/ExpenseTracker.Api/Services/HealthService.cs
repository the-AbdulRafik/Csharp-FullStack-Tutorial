using ExpenseTracker.Api.DTOs;

namespace ExpenseTracker.Api.Services;

public sealed class HealthService
{
    public HealthResponse GetStatus()
    {
        return new HealthResponse(
            Status: "Healthy",
            Service: "ExpenseTracker API",
            UtcTime: DateTimeOffset.UtcNow);
    }
}
