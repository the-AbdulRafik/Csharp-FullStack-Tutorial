using BankManagement_API.Models;

namespace BankManagement_API.Data;

public class AccountStore
{
    private readonly List<BankAccount> _accounts = [];
    private int _nextId = 1;

    public AccountStore()
    {
        _accounts.Add(new BankAccount
        {
            Id = _nextId++,
            AccountHolderName = "Sample Customer",
            AccountNumber = "AC10000",
            Balance = 500,
            AccountType = "Savings"
        });
    }

    public List<BankAccount> GetAll() => _accounts.ToList();

    public BankAccount? GetById(int id) => _accounts.FirstOrDefault(account => account.Id == id);

    public BankAccount Add(CreateAccountRequest request)
    {
        var account = new BankAccount
        {
            Id = _nextId++,
            AccountHolderName = request.AccountHolderName.Trim(),
            AccountNumber = request.AccountNumber.Trim(),
            Balance = request.Balance,
            AccountType = request.AccountType.Trim()
        };

        _accounts.Add(account);
        return account;
    }

    public bool AccountNumberExists(string accountNumber, int? excludedId = null) =>
        _accounts.Any(account =>
            account.AccountNumber.Equals(accountNumber, StringComparison.OrdinalIgnoreCase) &&
            account.Id != excludedId);

    public List<BankAccount> Search(string? accountType, string? accountHolderName)
    {
        return _accounts
            .Where(account =>
                (string.IsNullOrWhiteSpace(accountType) ||
                 account.AccountType.Equals(accountType.Trim(), StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(accountHolderName) ||
                 account.AccountHolderName.Contains(accountHolderName.Trim(), StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }

    public bool Delete(int id)
    {
        var account = GetById(id);
        return account is not null && _accounts.Remove(account);
    }
}
