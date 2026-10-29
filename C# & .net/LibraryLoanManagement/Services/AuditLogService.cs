using LibraryLoanManagement.Models;
using LibraryLoanManagement.Repositories;

namespace LibraryLoanManagement.Services;

public class AuditLogService(AuditLogRepository repository)
{
    public Task<List<AuditLog>> RecentAsync()=>repository.GetRecentAsync();
}
