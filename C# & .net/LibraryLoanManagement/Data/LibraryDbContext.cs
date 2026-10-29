using LibraryLoanManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryLoanManagement.Data;

public class LibraryDbContext(DbContextOptions<LibraryDbContext> options) : DbContext(options)
{
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(x => x.BranchId);
            entity.Property(x => x.BranchName).HasMaxLength(100).IsRequired();
            entity.HasIndex(x => x.BranchName).IsUnique();
            entity.Property(x => x.Location).HasMaxLength(100);
            entity.Property(x => x.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(x => x.MemberId);
            entity.Ignore(x => x.BranchName);
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(255).IsRequired();
            entity.HasIndex(x => x.Email).IsUnique();
            entity.Property(x => x.JoinDate).HasDefaultValueSql("SYSDATETIME()");
            entity.Property(x => x.IsActive).HasDefaultValue(true);
            entity.ToTable("Members", table => table.HasCheckConstraint("CK_Members_Age", "[Age] BETWEEN 5 AND 100"));
            entity.HasOne(x => x.Branch).WithMany(x => x.Members)
                .HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(x => x.BookId);
            entity.Ignore(x => x.BranchName);
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Author).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Isbn).HasMaxLength(20).IsRequired();
            entity.HasIndex(x => x.Isbn).IsUnique();
            entity.Property(x => x.LateFeePerDay).HasPrecision(10, 2);
            entity.Property(x => x.IsActive).HasDefaultValue(true);
            entity.ToTable("Books", table =>
            {
                table.HasCheckConstraint("CK_Books_LoanDays", "[LoanDays] > 0");
                table.HasCheckConstraint("CK_Books_LateFee", "[LateFeePerDay] >= 0");
            });
            entity.HasOne(x => x.Branch).WithMany(x => x.Books)
                .HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(x => x.LoanId);
            entity.Ignore(x => x.MemberName);
            entity.Ignore(x => x.BookTitle);
            entity.Property(x => x.Status).HasMaxLength(20).HasDefaultValue("Borrowed");
            entity.Property(x => x.LoanedAt).HasDefaultValueSql("SYSDATETIME()");
            entity.ToTable("Loans", table => table.HasCheckConstraint(
                "CK_Loans_Status", "[Status] IN ('Borrowed', 'Returned', 'Lost')"));
            entity.HasIndex(x => new { x.MemberId, x.BookId })
                .HasDatabaseName("UX_Loans_Member_Book_Borrowed")
                .HasFilter("[Status] = 'Borrowed'")
                .IsUnique();
            entity.HasOne(x => x.Member).WithMany(x => x.Loans)
                .HasForeignKey(x => x.MemberId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Book).WithMany(x => x.Loans)
                .HasForeignKey(x => x.BookId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(x => x.AuditId);
            entity.Property(x => x.EntityName).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Action).HasMaxLength(20).IsRequired();
            entity.Property(x => x.Details).HasMaxLength(500);
            entity.Property(x => x.CreatedAt).HasDefaultValueSql("SYSDATETIME()");
        });
    }
}
