using ExpenseTracker.Api.Data;
using ExpenseTracker.Api.DTOs.Dashboard;
using ExpenseTracker.Api.Enums;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Services;

public sealed class DashboardService(ExpenseTrackerDbContext db)
{
    public async Task<DashboardSummaryResponse> GetSummaryAsync(CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var monthStart = new DateOnly(today.Year, today.Month, 1);
        var nextMonth = monthStart.AddMonths(1);

        var income = await db.Transactions
            .Where(transaction => transaction.Type == TransactionType.Income)
            .SumAsync(transaction => (decimal?)transaction.Amount, cancellationToken) ?? 0;
        var expenses = await db.Transactions
            .Where(transaction => transaction.Type == TransactionType.Expense)
            .SumAsync(transaction => (decimal?)transaction.Amount, cancellationToken) ?? 0;
        var currentMonthIncome = await db.Transactions
            .Where(transaction => transaction.Type == TransactionType.Income &&
                transaction.Date >= monthStart && transaction.Date < nextMonth)
            .SumAsync(transaction => (decimal?)transaction.Amount, cancellationToken) ?? 0;
        var currentMonthExpenses = await db.Transactions
            .Where(transaction => transaction.Type == TransactionType.Expense &&
                transaction.Date >= monthStart && transaction.Date < nextMonth)
            .SumAsync(transaction => (decimal?)transaction.Amount, cancellationToken) ?? 0;

        var recent = await db.Transactions
            .AsNoTracking()
            .Include(transaction => transaction.Category)
            .OrderByDescending(transaction => transaction.Date)
            .ThenByDescending(transaction => transaction.Id)
            .Take(5)
            .Select(transaction => new RecentTransactionResponse(
                transaction.Id,
                transaction.Title,
                transaction.Amount,
                transaction.Type,
                transaction.Date,
                transaction.Category.Name))
            .ToListAsync(cancellationToken);

        var expenseRows = await db.Transactions
            .AsNoTracking()
            .Include(transaction => transaction.Category)
            .Where(transaction => transaction.Type == TransactionType.Expense)
            .ToListAsync(cancellationToken);

        var categoryExpenses = expenseRows
            .GroupBy(transaction => new
            {
                transaction.CategoryId,
                transaction.Category.Name
            })
            .Select(group => new CategoryExpenseResponse(
                group.Key.CategoryId,
                group.Key.Name,
                group.Sum(transaction => transaction.Amount)))
            .OrderByDescending(item => item.Amount)
            .ToList();

        return new DashboardSummaryResponse(
            income,
            expenses,
            income - expenses,
            currentMonthIncome,
            currentMonthExpenses,
            recent,
            categoryExpenses);
    }
}
