using LibraryLoanManagement.Models;
using LibraryLoanManagement.Repositories;

namespace LibraryLoanManagement.Services;

public class BranchService(BranchRepository repository)
{
    public async Task AddAsync(string name,string? location) { if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Branch name is required."); await repository.AddAsync(new Branch { BranchName=name,Location=location }); }
    public Task<List<Branch>> ListAsync(string? search=null)=>repository.GetAllAsync(search);
    public Task UpdateAsync(Branch branch)=>repository.UpdateAsync(branch);
    public Task DeactivateAsync(int id)=>repository.DeactivateAsync(id);
}
