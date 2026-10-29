using LibraryLoanManagement.Models;
using LibraryLoanManagement.Repositories;

namespace LibraryLoanManagement.Services;

public class MemberService(MemberRepository repository)
{
    public async Task RegisterAsync(Member member)
    {
        if(string.IsNullOrWhiteSpace(member.Name)||string.IsNullOrWhiteSpace(member.Email)) throw new ArgumentException("Name and email are required.");
        if(member.Age is < 5 or > 100) throw new ArgumentOutOfRangeException(nameof(member.Age),"Age must be between 5 and 100.");
        if(member.BranchId<=0) throw new ArgumentException("Select a valid branch.");
        await repository.AddAsync(member);
    }
    public Task<List<Member>> ListAsync(string? search=null,int? branchId=null)=>repository.GetAllAsync(search,branchId);
    public Task UpdateAsync(Member member)=>repository.UpdateAsync(member);
    public Task DeactivateAsync(int id)=>repository.DeactivateAsync(id);
}
