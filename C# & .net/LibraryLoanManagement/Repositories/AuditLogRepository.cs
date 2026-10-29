using LibraryLoanManagement.Data;
using LibraryLoanManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryLoanManagement.Repositories;

public class AuditLogRepository(LibraryDbContext db)
{
    public Task<List<AuditLog>> GetRecentAsync(int count = 20) =>
        db.AuditLogs.AsNoTracking().OrderByDescending(x => x.CreatedAt).Take(count).ToListAsync();
}
