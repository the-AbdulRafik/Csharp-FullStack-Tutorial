namespace ExpenseTracker.Api.DTOs.Auth;

public sealed record RegisterResponse(
    string Id,
    string Email,
    string Message);
