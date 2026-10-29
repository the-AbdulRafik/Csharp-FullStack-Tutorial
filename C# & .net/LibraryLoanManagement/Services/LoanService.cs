using LibraryLoanManagement.Models;
using LibraryLoanManagement.Repositories;

namespace LibraryLoanManagement.Services;

public class LoanService(LoanRepository repository)
{
    public Task<int> IssueAsync(int memberId,int bookId) { if(memberId<=0||bookId<=0) throw new ArgumentException("Select a valid member and book."); return repository.IssueAsync(memberId,bookId); }
    public Task<List<Loan>> ListAsync(int? memberId=null,int? bookId=null)=>repository.GetAllAsync(memberId,bookId);
    public Task ReturnAsync(int id)=>repository.ChangeStatusAsync(id,"Returned");
    public Task MarkLostAsync(int id)=>repository.ChangeStatusAsync(id,"Lost");
    public Task<List<(string Name,int Count)>> MemberCountByBranchAsync()=>repository.MemberCountByBranchAsync();
    public Task<List<(string Title,int Count)>> LoanCountByBookAsync()=>repository.LoanCountByBookAsync();
    public Task<List<(int Id,string Name)>> MembersWithoutLoansAsync()=>repository.MembersWithoutLoansAsync();
    public Task<List<(int Id,string Title)>> BooksWithoutLoansAsync()=>repository.BooksWithoutLoansAsync();
}
