namespace LibraryLoanManagement.Models;

public class Loan
{
    public int LoanId { get; set; }
    public int MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public DateTime LoanedAt { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedAt { get; set; }
    public string Status { get; set; } = "Borrowed";
    public Member? Member { get; set; }
    public Book? Book { get; set; }
}
