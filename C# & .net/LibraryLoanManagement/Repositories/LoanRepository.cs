using LibraryLoanManagement.Data;
using LibraryLoanManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryLoanManagement.Repositories;

public class LoanRepository(LibraryDbContext db)
{
    public async Task<int> IssueAsync(int memberId, int bookId)
    {
        await using var transaction = await db.Database.BeginTransactionAsync();
        try
        {
            var memberExists = await db.Members.AnyAsync(x => x.MemberId == memberId && x.IsActive);
            var book = await db.Books.SingleOrDefaultAsync(x => x.BookId == bookId && x.IsActive);
            if (!memberExists || book is null)
                throw new InvalidOperationException("Member or book was not found or is inactive.");

            var loan = new Loan
            {
                MemberId = memberId,
                BookId = bookId,
                DueDate = DateTime.Now.AddDays(book.LoanDays),
                Status = "Borrowed"
            };
            db.Loans.Add(loan);
            await db.SaveChangesAsync();

            db.AuditLogs.Add(new AuditLog
            {
                EntityName = "Loan",
                EntityId = loan.LoanId,
                Action = "ISSUE",
                Details = $"Member {memberId} borrowed book {bookId}."
            });
            await db.SaveChangesAsync();
            await transaction.CommitAsync();
            return loan.LoanId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public Task<List<Loan>> GetAllAsync(int? memberId = null, int? bookId = null) =>
        db.Loans.AsNoTracking()
            .Where(x => (memberId == null || x.MemberId == memberId) &&
                        (bookId == null || x.BookId == bookId))
            .OrderByDescending(x => x.LoanedAt)
            .Select(x => new Loan
            {
                LoanId = x.LoanId, MemberId = x.MemberId, MemberName = x.Member!.Name,
                BookId = x.BookId, BookTitle = x.Book!.Title, LoanedAt = x.LoanedAt,
                DueDate = x.DueDate, ReturnedAt = x.ReturnedAt, Status = x.Status
            }).ToListAsync();

    public async Task ChangeStatusAsync(int loanId, string status)
    {
        var loan = await db.Loans.FindAsync(loanId) ?? throw new InvalidOperationException("Loan was not found.");
        loan.Status = status;
        loan.ReturnedAt = status == "Borrowed" ? null : DateTime.Now;
        await db.SaveChangesAsync();
    }

    public async Task<List<(string Name, int Count)>> MemberCountByBranchAsync() =>
        await db.Branches.AsNoTracking()
            .Select(x => new { x.BranchName, Count = x.Members.Count })
            .OrderBy(x => x.BranchName)
            .Select(x => ValueTuple.Create(x.BranchName, x.Count)).ToListAsync();

    public async Task<List<(string Title, int Count)>> LoanCountByBookAsync() =>
        await db.Books.AsNoTracking()
            .Select(x => new { x.Title, Count = x.Loans.Count })
            .OrderByDescending(x => x.Count)
            .Select(x => ValueTuple.Create(x.Title, x.Count)).ToListAsync();

    public async Task<List<(int Id, string Name)>> MembersWithoutLoansAsync() =>
        await db.Members.AsNoTracking().Where(x => !x.Loans.Any())
            .Select(x => ValueTuple.Create(x.MemberId, x.Name)).ToListAsync();

    public async Task<List<(int Id, string Title)>> BooksWithoutLoansAsync() =>
        await db.Books.AsNoTracking().Where(x => !x.Loans.Any())
            .Select(x => ValueTuple.Create(x.BookId, x.Title)).ToListAsync();
}
