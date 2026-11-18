using ExpenseTracker.Api.Enums;

namespace ExpenseTracker.Api.DTOs.Categories;

public sealed record CategoryResponse(
    int Id,
    string Name,
    TransactionType Type,
    DateTime CreatedAt);
