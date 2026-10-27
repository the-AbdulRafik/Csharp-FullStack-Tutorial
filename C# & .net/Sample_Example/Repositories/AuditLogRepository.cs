using Microsoft.Data.SqlClient;
using Sample_Example.Helpers;
using Sample_Example.Models;

namespace Sample_Example.Repositories;

public class AuditLogRepository
{
    public List<AuditLog> GetRecent(int count = 20)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();
        using var command = new SqlCommand("""
            SELECT TOP (@Count) AuditId, EntityName, EntityId, Action, Details, CreatedAt
            FROM AuditLogs ORDER BY CreatedAt DESC
            """, connection);
        command.Parameters.AddWithValue("@Count", count);
        using var reader = command.ExecuteReader();
        var result = new List<AuditLog>();
        while (reader.Read())
        {
            result.Add(new AuditLog
            {
                AuditId = reader.GetInt64(0), EntityName = reader.GetString(1),
                EntityId = reader.GetInt32(2), Action = reader.GetString(3),
                Details = reader.IsDBNull(4) ? null : reader.GetString(4),
                CreatedAt = reader.GetDateTime(5)
            });
        }
        return result;
    }
}
