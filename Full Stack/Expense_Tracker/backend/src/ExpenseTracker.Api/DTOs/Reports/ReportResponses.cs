using ExpenseTracker.Api.Enums;

namespace ExpenseTracker.Api.DTOs.Reports;

public sealed record ReportSummaryResponse(
    decimal TotalIncome,
    decimal TotalExpenses,
    decimal Balance,
    IReadOnlyList<MonthlyReportResponse> Monthly,
    IReadOnlyList<CategoryReportResponse> Categories);

public sealed record MonthlyReportResponse(
    string Month,
    decimal Income,
    decimal Expenses);

public sealed record CategoryReportResponse(
    int CategoryId,
    string CategoryName,
    decimal Amount,
    TransactionType Type);
