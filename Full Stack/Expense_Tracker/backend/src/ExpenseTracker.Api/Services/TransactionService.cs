using ExpenseTracker.Api.Data;
using ExpenseTracker.Api.DTOs.Transactions;
using ExpenseTracker.Api.Entities;
using ExpenseTracker.Api.Enums;
using ExpenseTracker.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Services;

public sealed class TransactionService(ExpenseTrackerDbContext db)
{
    public async Task<IReadOnlyList<TransactionResponse>> GetAllAsync(
        TransactionType? type,
        int? categoryId,
        DateOnly? from,
        DateOnly? to,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;
        if (pageSize > 100) pageSize = 100;
        if (from.HasValue && to.HasValue && from > to)
            throw new ApiBadRequestException("The start date cannot be after the end date.");

        var query = db.Transactions
            .AsNoTracking()
            .Include(transaction => transaction.Category)
            .AsQueryable();

        if (type.HasValue)
            query = query.Where(transaction => transaction.Type == type.Value);
        if (categoryId.HasValue)
            query = query.Where(transaction => transaction.CategoryId == categoryId.Value);
        if (from.HasValue)
            query = query.Where(transaction => transaction.Date >= from.Value);
        if (to.HasValue)
            query = query.Where(transaction => transaction.Date <= to.Value);

        var transactions = await query
            .OrderByDescending(transaction => transaction.Date)
            .ThenByDescending(transaction => transaction.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return transactions.Select(ToResponse).ToList();
    }

    public async Task<TransactionResponse> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var transaction = await db.Transactions
            .AsNoTracking()
            .Include(item => item.Category)
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken)
            ?? throw new ApiNotFoundException($"Transaction {id} was not found.");

        return ToResponse(transaction);
    }

    public async Task<TransactionResponse> CreateAsync(CreateTransactionRequest request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var transaction = new Transaction
        {
            Title = request.Title.Trim(),
            Amount = request.Amount,
            Type = request.Type,
            Date = request.Date,
            Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
            CategoryId = request.CategoryId,
            CreatedAt = DateTime.UtcNow
        };

        db.Transactions.Add(transaction);
        await db.SaveChangesAsync(cancellationToken);
        await db.Entry(transaction).Reference(item => item.Category).LoadAsync(cancellationToken);
        return ToResponse(transaction);
    }

    public async Task<TransactionResponse> UpdateAsync(int id, UpdateTransactionRequest request, CancellationToken cancellationToken)
    {
        var transaction = await db.Transactions
            .Include(item => item.Category)
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken)
            ?? throw new ApiNotFoundException($"Transaction {id} was not found.");

        await ValidateAsync(request, cancellationToken);
        transaction.Title = request.Title.Trim();
        transaction.Amount = request.Amount;
        transaction.Type = request.Type;
        transaction.Date = request.Date;
        transaction.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
        transaction.CategoryId = request.CategoryId;
        transaction.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(cancellationToken);
        await db.Entry(transaction).Reference(item => item.Category).LoadAsync(cancellationToken);
        return ToResponse(transaction);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var transaction = await db.Transactions.FirstOrDefaultAsync(item => item.Id == id, cancellationToken)
            ?? throw new ApiNotFoundException($"Transaction {id} was not found.");

        db.Transactions.Remove(transaction);
        await db.SaveChangesAsync(cancellationToken);
    }

    private async Task ValidateAsync(CreateTransactionRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new ApiBadRequestException("Transaction title is required.");
        if (request.Title.Trim().Length > 100)
            throw new ApiBadRequestException("Transaction title cannot exceed 100 characters.");
        if (request.Amount <= 0)
            throw new ApiBadRequestException("Transaction amount must be greater than zero.");
        if (request.Date == default)
            throw new ApiBadRequestException("Transaction date is required.");
        if (!string.IsNullOrEmpty(request.Description) && request.Description.Length > 500)
            throw new ApiBadRequestException("Description cannot exceed 500 characters.");

        var category = await db.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == request.CategoryId, cancellationToken)
            ?? throw new ApiBadRequestException("The selected category does not exist.");

        if (category.Type != request.Type)
            throw new ApiBadRequestException("The transaction type must match the selected category type.");
    }

    private static TransactionResponse ToResponse(Transaction transaction) =>
        new(
            transaction.Id,
            transaction.Title,
            transaction.Amount,
            transaction.Type,
            transaction.Date,
            transaction.Description,
            transaction.CategoryId,
            new CategorySummaryResponse(
                transaction.Category.Id,
                transaction.Category.Name,
                transaction.Category.Type),
            transaction.CreatedAt,
            transaction.UpdatedAt);
}
