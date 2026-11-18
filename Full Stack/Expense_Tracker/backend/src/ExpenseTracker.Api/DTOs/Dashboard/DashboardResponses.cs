using ExpenseTracker.Api.Enums;

namespace ExpenseTracker.Api.DTOs.Dashboard;

public sealed record DashboardSummaryResponse(
    decimal TotalIncome,
    decimal TotalExpenses,
    decimal CurrentBalance,
    decimal CurrentMonthIncome,
    decimal CurrentMonthExpenses,
    IReadOnlyList<RecentTransactionResponse> RecentTransactions,
    IReadOnlyList<CategoryExpenseResponse> CategoryExpenses);

public sealed record RecentTransactionResponse(
    int Id,
    string Title,
    decimal Amount,
    TransactionType Type,
    DateOnly Date,
    string CategoryName);

public sealed record CategoryExpenseResponse(
    int CategoryId,
    string CategoryName,
    decimal Amount);
