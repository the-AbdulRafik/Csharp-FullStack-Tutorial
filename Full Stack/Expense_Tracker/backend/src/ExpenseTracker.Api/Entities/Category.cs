using ExpenseTracker.Api.Enums;

namespace ExpenseTracker.Api.Entities;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public TransactionType Type { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<Transaction> Transactions { get; set; }
        = new List<Transaction>();
}