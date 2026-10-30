using LibraryLoanManagement.Data;
using LibraryLoanManagement.Models;
using LibraryLoanManagement.Repositories;
using LibraryLoanManagement.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;


var builder = Host.CreateApplicationBuilder(args);

var services = builder.Services;
// services.AddDbContext<LibraryDbContext>(options =>
//     options.UseSqlServer(
//         @"Server=.\SQLEXPRESS;Database=LibraryLoanDB;Trusted_Connection=True;TrustServerCertificate=True;"));

services.AddDbContext<LibraryDbContext>(options =>
{
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("app.settings.json")
        .Build();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);

    Console.WriteLine($"Using connection string: {connectionString}");
});

services.AddScoped<BranchRepository>();
services.AddScoped<MemberRepository>();
services.AddScoped<BookRepository>();
services.AddScoped<LoanRepository>();
services.AddScoped<AuditLogRepository>();
services.AddScoped<BranchService>();
services.AddScoped<MemberService>();
services.AddScoped<BookService>();
services.AddScoped<LoanService>();
services.AddScoped<AuditLogService>();

await using var provider = services.BuildServiceProvider();
await using var scope = provider.CreateAsyncScope();
var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
await db.Database.MigrateAsync();

var branches = scope.ServiceProvider.GetRequiredService<BranchService>();
var members = scope.ServiceProvider.GetRequiredService<MemberService>();
var books = scope.ServiceProvider.GetRequiredService<BookService>();
var loans = scope.ServiceProvider.GetRequiredService<LoanService>();
var audit = scope.ServiceProvider.GetRequiredService<AuditLogService>();

while (true)
{
    Console.WriteLine("\n========================================");
    Console.WriteLine("       LIBRARY LOAN MANAGEMENT SYSTEM");
    Console.WriteLine("========================================");
    Console.WriteLine("1. Branch Management");
    Console.WriteLine("2. Member Management");
    Console.WriteLine("3. Book Management");
    Console.WriteLine("4. Loan Management");
    Console.WriteLine("5. Reports");
    Console.WriteLine("6. View Audit Logs");
    Console.WriteLine("0. Exit");
    Console.Write("Enter your choice: ");

    try
    {
        switch (Console.ReadLine())
        {
            case "1": await BranchMenuAsync(); break;
            case "2": await MemberMenuAsync(); break;
            case "3": await BookMenuAsync(); break;
            case "4": await LoanMenuAsync(); break;
            case "5": await ReportMenuAsync(); break;
            case "6": await ShowAuditLogsAsync(); break;
            case "0": return;
            default: Console.WriteLine("Invalid choice."); break;
        }
    }
    catch (FormatException) { Console.WriteLine("Enter a valid number."); }
    catch (DbUpdateException ex) { Console.WriteLine($"Database error: {ex.InnerException?.Message ?? ex.Message}"); }
    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
    { Console.WriteLine($"Error: {ex.Message}"); }
}

async Task BranchMenuAsync()
{
    Console.WriteLine("\n1 Add  2 List/Search  3 Update  4 Deactivate");
    switch (Console.ReadLine())
    {
        case "1":
            await branches.AddAsync(ReadRequired("Branch name: "), ReadOptional("Location: "));
            Console.WriteLine("Branch added."); break;
        case "2":
            foreach (var b in await branches.ListAsync(ReadOptional("Search (blank for all): ")))
                Console.WriteLine($"{b.BranchId}: {b.BranchName} | {b.Location} | {(b.IsActive ? "Active" : "Inactive")}");
            break;
        case "3":
            await branches.UpdateAsync(new Branch { BranchId=ReadInt("Branch ID: "), BranchName=ReadRequired("New name: "), Location=ReadOptional("New location: ") });
            Console.WriteLine("Branch updated."); break;
        case "4": await branches.DeactivateAsync(ReadInt("Branch ID: ")); Console.WriteLine("Branch deactivated."); break;
    }
}

async Task MemberMenuAsync()
{
    Console.WriteLine("\n1 Register \n2 List/Search \n3 Filter Branch \n4 Update \n5 Deactivate");
    switch (Console.ReadLine())
    {
        case "1": await members.RegisterAsync(ReadMember()); Console.WriteLine("Member registered."); break;
        case "2": PrintMembers(await members.ListAsync(ReadOptional("Search name/ID (blank for all): "))); break;
        case "3": PrintMembers(await members.ListAsync(null, ReadInt("Branch ID: "))); break;
        case "4":
            var member = ReadMember(); member.MemberId=ReadInt("Member ID: "); await members.UpdateAsync(member); Console.WriteLine("Member updated."); break;
        case "5": await members.DeactivateAsync(ReadInt("Member ID: ")); Console.WriteLine("Member deactivated."); break;
    }
}

async Task BookMenuAsync()
{
    Console.WriteLine("\n1 Add  \n2 List/Search  \n3 Filter Branch  \n4 Update  \n5 Deactivate");
    switch (Console.ReadLine())
    {
        case "1": await books.AddAsync(ReadBook()); Console.WriteLine("Book added."); break;
        case "2": PrintBooks(await books.ListAsync(ReadOptional("Search title/author/ISBN (blank for all): "))); break;
        case "3": PrintBooks(await books.ListAsync(null, ReadInt("Branch ID: "))); break;
        case "4":
            var book = ReadBook(); book.BookId=ReadInt("Book ID: "); await books.UpdateAsync(book); Console.WriteLine("Book updated."); break;
        case "5": await books.DeactivateAsync(ReadInt("Book ID: ")); Console.WriteLine("Book deactivated."); break;
    }
}

async Task LoanMenuAsync()
{
    Console.WriteLine("\n1 Issue  \n2 List All  \n3 Member Loans  \n4 Book History  \n5 Return  \n6 Mark Lost");
    switch (Console.ReadLine())
    {
        case "1": Console.WriteLine($"Loan created with ID {await loans.IssueAsync(ReadInt("Member ID: "), ReadInt("Book ID: "))}."); break;
        case "2": PrintLoans(await loans.ListAsync()); break;
        case "3": PrintLoans(await loans.ListAsync(ReadInt("Member ID: "))); break;
        case "4": PrintLoans(await loans.ListAsync(null, ReadInt("Book ID: "))); break;
        case "5": await loans.ReturnAsync(ReadInt("Loan ID: ")); Console.WriteLine("Book returned."); break;
        case "6": await loans.MarkLostAsync(ReadInt("Loan ID: ")); Console.WriteLine("Loan marked lost."); break;
    }
}

async Task ReportMenuAsync()
{
    Console.WriteLine("\n1 Member count by branch  \n2 Loan count by book  \n3 Members without loans  \n4 Books without loans");
    switch (Console.ReadLine())
    {
        case "1": foreach (var x in await loans.MemberCountByBranchAsync()) Console.WriteLine($"{x.Name}: {x.Count} members"); break;
        case "2": foreach (var x in await loans.LoanCountByBookAsync()) Console.WriteLine($"{x.Title}: {x.Count} loans"); break;
        case "3": foreach (var x in await loans.MembersWithoutLoansAsync()) Console.WriteLine($"{x.Id}: {x.Name}"); break;
        case "4": foreach (var x in await loans.BooksWithoutLoansAsync()) Console.WriteLine($"{x.Id}: {x.Title}"); break;
    }
}

async Task ShowAuditLogsAsync()
{
    foreach (var log in await audit.RecentAsync())
        Console.WriteLine($"{log.CreatedAt:g} | {log.EntityName} {log.Action} | {log.Details}");
}

Member ReadMember() => new()
{
    Name=ReadRequired("Name: "), Email=ReadRequired("Email: "),
    Age=ReadInt("Age: "), BranchId=ReadInt("Branch ID: ")
};

Book ReadBook() => new()
{
    Title=ReadRequired("Title: "), Author=ReadRequired("Author: "),
    Isbn=ReadRequired("ISBN: "), LoanDays=ReadInt("Loan days: "),
    LateFeePerDay=ReadDecimal("Late fee per day: "), BranchId=ReadInt("Branch ID: ")
};

void PrintMembers(IEnumerable<Member> items)
{
    foreach (var m in items) Console.WriteLine($"{m.MemberId}: {m.Name} | {m.Email} | Age {m.Age} | {m.BranchName} | {(m.IsActive ? "Active" : "Inactive")}");
}

void PrintBooks(IEnumerable<Book> items)
{
    foreach (var b in items) Console.WriteLine($"{b.BookId}: {b.Title} by {b.Author} | ISBN {b.Isbn} | {b.BranchName} | {(b.IsActive ? "Active" : "Inactive")}");
}

void PrintLoans(IEnumerable<Loan> items)
{
    foreach (var l in items) Console.WriteLine($"{l.LoanId}: {l.MemberName} -> {l.BookTitle} | Due {l.DueDate:d} | {l.Status}");
}

string ReadRequired(string label) { Console.Write(label); var value=Console.ReadLine() ?? ""; if(string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Value is required."); return value.Trim(); }
string? ReadOptional(string label) { Console.Write(label); var value=Console.ReadLine(); return string.IsNullOrWhiteSpace(value) ? null : value.Trim(); }
int ReadInt(string label) { Console.Write(label); return int.Parse(Console.ReadLine() ?? ""); }
decimal ReadDecimal(string label) { Console.Write(label); return decimal.Parse(Console.ReadLine() ?? ""); }
