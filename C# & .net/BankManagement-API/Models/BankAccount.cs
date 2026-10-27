using System.ComponentModel.DataAnnotations;

namespace BankManagement_API.Models;

public class BankAccount
{
    public int Id { get; set; }

    [Required]
    public string AccountHolderName { get; set; } = string.Empty;

    [Required]
    public string AccountNumber { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Balance { get; set; }

    [Required]
    public string AccountType { get; set; } = string.Empty;
}

public class CreateAccountRequest
{
    [Required]
    public string AccountHolderName { get; set; } = string.Empty;

    [Required]
    public string AccountNumber { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Balance { get; set; }

    [Required]
    public string AccountType { get; set; } = string.Empty;
}

public class UpdateAccountRequest
{
    [Required]
    public string AccountHolderName { get; set; } = string.Empty;

    [Required]
    public string AccountType { get; set; } = string.Empty;
}

public class TransactionRequest
{
    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
    public decimal Amount { get; set; }
}
