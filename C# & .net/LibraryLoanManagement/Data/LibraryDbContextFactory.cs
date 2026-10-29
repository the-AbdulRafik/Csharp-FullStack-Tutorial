using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LibraryLoanManagement.Data;

public class LibraryDbContextFactory : IDesignTimeDbContextFactory<LibraryDbContext>
{
    public LibraryDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseSqlServer(
                @"Server=.\SQLEXPRESS;Database=LibraryLoanDB;Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        return new LibraryDbContext(options);
    }
}
