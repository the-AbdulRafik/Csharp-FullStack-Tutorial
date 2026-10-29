namespace LibraryLoanManagement.Models;

public class Member
{
    public int MemberId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public DateTime JoinDate { get; set; }
    public bool IsActive { get; set; }
    public Branch? Branch { get; set; }
    public ICollection<Loan> Loans { get; set; } = [];
}
