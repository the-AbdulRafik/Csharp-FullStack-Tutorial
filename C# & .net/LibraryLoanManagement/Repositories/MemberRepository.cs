using LibraryLoanManagement.Data;
using LibraryLoanManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryLoanManagement.Repositories;

public class MemberRepository(LibraryDbContext db)
{
    public async Task AddAsync(Member member) { db.Members.Add(member); await db.SaveChangesAsync(); }

    public Task<List<Member>> GetAllAsync(string? search = null, int? branchId = null) =>
        db.Members.AsNoTracking()
            .Where(x => (search == null || x.Name.Contains(search) ||
                         x.MemberId.ToString() == search) &&
                        (branchId == null || x.BranchId == branchId))
            .OrderBy(x => x.Name)
            .Select(x => new Member
            {
                MemberId = x.MemberId, Name = x.Name, Email = x.Email, Age = x.Age,
                BranchId = x.BranchId, BranchName = x.Branch!.BranchName,
                JoinDate = x.JoinDate, IsActive = x.IsActive
            }).ToListAsync();

    public async Task UpdateAsync(Member member)
    {
        db.Members.Update(member);
        await db.SaveChangesAsync();
    }

    public async Task DeactivateAsync(int id)
    {
        var member = await db.Members.FindAsync(id) ?? throw new InvalidOperationException("Member was not found.");
        member.IsActive = false;
        await db.SaveChangesAsync();
    }
}
