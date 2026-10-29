namespace LibraryLoanManagement.Models;

public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public int LoanDays { get; set; }
    public decimal LateFeePerDay { get; set; }
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Branch? Branch { get; set; }
    public ICollection<Loan> Loans { get; set; } = [];
}
