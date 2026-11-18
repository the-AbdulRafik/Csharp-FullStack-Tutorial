using ExpenseTracker.Api.Data;
using ExpenseTracker.Api.DTOs.Reports;
using ExpenseTracker.Api.Enums;
using ExpenseTracker.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Services;

public sealed class ReportService(ExpenseTrackerDbContext db)
{
    public async Task<ReportSummaryResponse> GetSummaryAsync(
        DateOnly? from,
        DateOnly? to,
        CancellationToken cancellationToken)
    {
        if (from.HasValue && to.HasValue && from > to)
            throw new ApiBadRequestException("The start date cannot be after the end date.");

        var query = db.Transactions.AsNoTracking().AsQueryable();
        if (from.HasValue)
            query = query.Where(transaction => transaction.Date >= from.Value);
        if (to.HasValue)
            query = query.Where(transaction => transaction.Date <= to.Value);

        var income = await query
            .Where(transaction => transaction.Type == TransactionType.Income)
            .SumAsync(transaction => (decimal?)transaction.Amount, cancellationToken) ?? 0;
        var expenses = await query
            .Where(transaction => transaction.Type == TransactionType.Expense)
            .SumAsync(transaction => (decimal?)transaction.Amount, cancellationToken) ?? 0;

        var reportRows = await query
            .Include(transaction => transaction.Category)
            .ToListAsync(cancellationToken);

        var monthly = reportRows
            .GroupBy(transaction => new
            {
                transaction.Date.Year,
                transaction.Date.Month
            })
            .OrderBy(group => group.Key.Year)
            .ThenBy(group => group.Key.Month)
            .Select(group => new MonthlyReportResponse(
                $"{group.Key.Year}-{group.Key.Month:00}",
                group.Where(transaction => transaction.Type == TransactionType.Income)
                    .Sum(transaction => transaction.Amount),
                group.Where(transaction => transaction.Type == TransactionType.Expense)
                    .Sum(transaction => transaction.Amount)))
            .ToList();

        var categories = reportRows
            .GroupBy(transaction => new
            {
                transaction.CategoryId,
                transaction.Category.Name,
                transaction.Type
            })
            .Select(group => new CategoryReportResponse(
                group.Key.CategoryId,
                group.Key.Name,
                group.Sum(transaction => transaction.Amount),
                group.Key.Type))
            .OrderBy(item => item.CategoryName)
            .ToList();

        return new ReportSummaryResponse(income, expenses, income - expenses, monthly, categories);
    }
}
