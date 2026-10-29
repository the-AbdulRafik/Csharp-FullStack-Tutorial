using LibraryLoanManagement.Data;
using LibraryLoanManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryLoanManagement.Repositories;

public class BookRepository(LibraryDbContext db)
{
    public async Task AddAsync(Book book) { db.Books.Add(book); await db.SaveChangesAsync(); }

    public Task<List<Book>> GetAllAsync(string? search = null, int? branchId = null) =>
        db.Books.AsNoTracking()
            .Where(x => (search == null || x.Title.Contains(search) ||
                         x.Author.Contains(search) || x.Isbn.Contains(search)) &&
                        (branchId == null || x.BranchId == branchId))
            .OrderBy(x => x.Title)
            .Select(x => new Book
            {
                BookId = x.BookId, Title = x.Title, Author = x.Author, Isbn = x.Isbn,
                LoanDays = x.LoanDays, LateFeePerDay = x.LateFeePerDay,
                BranchId = x.BranchId, BranchName = x.Branch!.BranchName,
                IsActive = x.IsActive
            }).ToListAsync();

    public async Task UpdateAsync(Book book)
    {
        db.Books.Update(book);
        await db.SaveChangesAsync();
    }

    public async Task DeactivateAsync(int id)
    {
        var book = await db.Books.FindAsync(id) ?? throw new InvalidOperationException("Book was not found.");
        book.IsActive = false;
        await db.SaveChangesAsync();
    }
}
