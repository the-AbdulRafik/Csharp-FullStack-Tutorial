using ExpenseTracker.Api.Enums;

namespace ExpenseTracker.Api.DTOs.Transactions;

public sealed record CategorySummaryResponse(
    int Id,
    string Name,
    TransactionType Type);

public sealed record TransactionResponse(
    int Id,
    string Title,
    decimal Amount,
    TransactionType Type,
    DateOnly Date,
    string? Description,
    int CategoryId,
    CategorySummaryResponse Category,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
