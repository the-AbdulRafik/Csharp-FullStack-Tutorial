using BankManagement_API.Data;
using BankManagement_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankManagement_API.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountsController(AccountStore store) : ControllerBase
{
    [HttpPost]
    public ActionResult<BankAccount> Create(CreateAccountRequest request)
    {
        if (store.AccountNumberExists(request.AccountNumber.Trim()))
        {
            return Conflict("An account with this account number already exists.");
        }

        var account = store.Add(request);
        return CreatedAtAction(nameof(GetById), new { id = account.Id }, account);
    }

    [HttpGet]
    public ActionResult<List<BankAccount>> GetAll() => Ok(store.GetAll());

    [HttpGet("search")]
    public ActionResult<List<BankAccount>> Search(
        [FromQuery] string? accountType,
        [FromQuery] string? accountHolderName) =>
        Ok(store.Search(accountType, accountHolderName));

    [HttpGet("{id:int}")]
    public ActionResult<BankAccount> GetById(int id)
    {
        var account = store.GetById(id);
        return account is null ? NotFound() : Ok(account);
    }

    [HttpPut("{id:int}")]
    public ActionResult<BankAccount> Update(int id, UpdateAccountRequest request)
    {
        var account = store.GetById(id);
        if (account is null)
        {
            return NotFound();
        }

        account.AccountHolderName = request.AccountHolderName.Trim();
        account.AccountType = request.AccountType.Trim();
        return Ok(account);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id) =>
        store.Delete(id) ? NoContent() : NotFound();

    [HttpPost("{id:int}/deposit")]
    public ActionResult<BankAccount> Deposit(int id, TransactionRequest request)
    {
        var account = store.GetById(id);
        if (account is null)
        {
            return NotFound();
        }

        account.Balance += request.Amount;
        return Ok(account);
    }

    [HttpPost("{id:int}/withdraw")]
    public ActionResult<BankAccount> Withdraw(int id, TransactionRequest request)
    {
        var account = store.GetById(id);
        if (account is null)
        {
            return NotFound();
        }

        if (request.Amount > account.Balance)
        {
            return BadRequest("Withdrawal amount cannot be greater than the current balance.");
        }

        account.Balance -= request.Amount;
        return Ok(account);
    }
}
