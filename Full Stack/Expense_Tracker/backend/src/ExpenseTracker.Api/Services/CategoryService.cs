using ExpenseTracker.Api.Data;
using ExpenseTracker.Api.DTOs.Categories;
using ExpenseTracker.Api.Entities;
using ExpenseTracker.Api.Enums;
using ExpenseTracker.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Services;

public sealed class CategoryService(ExpenseTrackerDbContext db)
{
    public async Task<IReadOnlyList<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await db.Categories
            .AsNoTracking()
            .OrderBy(category => category.Type)
            .ThenBy(category => category.Name)
            .Select(category => new CategoryResponse(
                category.Id, category.Name, category.Type, category.CreatedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<CategoryResponse> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var category = await db.Categories.AsNoTracking()
            .Where(item => item.Id == id)
            .Select(item => new CategoryResponse(item.Id, item.Name, item.Type, item.CreatedAt))
            .FirstOrDefaultAsync(cancellationToken);

        return category ?? throw new ApiNotFoundException($"Category {id} was not found.");
    }

    public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        await ValidateNameAsync(request.Name, request.Type, null, cancellationToken);

        var category = new Category
        {
            Name = request.Name.Trim(),
            Type = request.Type,
            CreatedAt = DateTime.UtcNow
        };

        db.Categories.Add(category);
        await db.SaveChangesAsync(cancellationToken);
        return new CategoryResponse(category.Id, category.Name, category.Type, category.CreatedAt);
    }

    public async Task<CategoryResponse> UpdateAsync(int id, UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await db.Categories.FirstOrDefaultAsync(item => item.Id == id, cancellationToken)
            ?? throw new ApiNotFoundException($"Category {id} was not found.");

        await ValidateNameAsync(request.Name, request.Type, id, cancellationToken);
        category.Name = request.Name.Trim();
        category.Type = request.Type;

        await db.SaveChangesAsync(cancellationToken);
        return new CategoryResponse(category.Id, category.Name, category.Type, category.CreatedAt);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var category = await db.Categories
            .Include(item => item.Transactions)
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken)
            ?? throw new ApiNotFoundException($"Category {id} was not found.");

        if (category.Transactions.Count > 0)
            throw new ApiConflictException("A category referenced by transactions cannot be deleted.");

        db.Categories.Remove(category);
        await db.SaveChangesAsync(cancellationToken);
    }

    private async Task ValidateNameAsync(string name, TransactionType type, int? excludedId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ApiBadRequestException("Category name is required.");
        if (name.Trim().Length > 50)
            throw new ApiBadRequestException("Category name cannot exceed 50 characters.");

        var duplicate = await db.Categories.AnyAsync(category =>
            category.Id != excludedId &&
            category.Type == type &&
            category.Name.ToLower() == name.Trim().ToLower(), cancellationToken);

        if (duplicate)
            throw new ApiConflictException("A category with this name and type already exists.");
    }
}
