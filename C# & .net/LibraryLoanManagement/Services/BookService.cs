using LibraryLoanManagement.Models;
using LibraryLoanManagement.Repositories;

namespace LibraryLoanManagement.Services;

public class BookService(BookRepository repository)
{
    public async Task AddAsync(Book book)
    {
        if(string.IsNullOrWhiteSpace(book.Title)||string.IsNullOrWhiteSpace(book.Author)||string.IsNullOrWhiteSpace(book.Isbn)) throw new ArgumentException("Title, author and ISBN are required.");
        if(book.LoanDays<=0||book.LateFeePerDay<0) throw new ArgumentException("Loan days and fee must be valid.");
        await repository.AddAsync(book);
    }
    public Task<List<Book>> ListAsync(string? search=null,int? branchId=null)=>repository.GetAllAsync(search,branchId);
    public Task UpdateAsync(Book book)=>repository.UpdateAsync(book);
    public Task DeactivateAsync(int id)=>repository.DeactivateAsync(id);
}
