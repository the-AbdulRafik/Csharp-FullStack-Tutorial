using System.ComponentModel.DataAnnotations;
using ExpenseTracker.Api.Enums;

namespace ExpenseTracker.Api.DTOs.Transactions;

public class CreateTransactionRequest
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
    public decimal Amount { get; set; }

    public TransactionType Type { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }
}

public class UpdateTransactionRequest : CreateTransactionRequest
{
}
