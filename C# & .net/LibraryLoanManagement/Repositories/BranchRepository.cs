using LibraryLoanManagement.Data;
using LibraryLoanManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryLoanManagement.Repositories;

public class BranchRepository(LibraryDbContext db)
{
    public async Task AddAsync(Branch branch) { db.Branches.Add(branch); await db.SaveChangesAsync(); }

    public Task<List<Branch>> GetAllAsync(string? search = null) =>
        db.Branches.AsNoTracking()
            .Where(x => search == null || x.BranchName.Contains(search))
            .OrderBy(x => x.BranchName).ToListAsync();

    public async Task UpdateAsync(Branch branch)
    {
        db.Branches.Update(branch);
        await db.SaveChangesAsync();
    }

    public async Task DeactivateAsync(int id)
    {
        var branch = await db.Branches.FindAsync(id) ?? throw new InvalidOperationException("Branch was not found.");
        branch.IsActive = false;
        await db.SaveChangesAsync();
    }
}
