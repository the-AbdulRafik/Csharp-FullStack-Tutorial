namespace LibraryLoanManagement.Models;

public class Branch
{
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public string? Location { get; set; }
    public bool IsActive { get; set; }
    public ICollection<Member> Members { get; set; } = [];
    public ICollection<Book> Books { get; set; } = [];
}
