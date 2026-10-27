using Sample_Example.Models;
using Sample_Example.Repositories;

namespace Sample_Example.Services;

public class AuditLogService(AuditLogRepository repository)
{
    public List<AuditLog> GetRecent() => repository.GetRecent();
}
