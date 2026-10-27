using Microsoft.Data.SqlClient;
using Sample_Example.Helpers;
using Sample_Example.Models;

namespace Sample_Example.Repositories;

public class DepartmentRepository
{
    public void Add(Department department)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();
        using var command = new SqlCommand(
            "INSERT INTO Departments (DepartmentName, Location) VALUES (@Name, @Location)", connection);
        command.Parameters.AddWithValue("@Name", department.DepartmentName);
        command.Parameters.AddWithValue("@Location", (object?)department.Location ?? DBNull.Value);
        command.ExecuteNonQuery();
    }

    public List<Department> GetAll(bool activeOnly = false)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();
        using var command = new SqlCommand(
            "SELECT DepartmentId, DepartmentName, Location, IsActive FROM Departments " +
            (activeOnly ? "WHERE IsActive = 1 " : "") + "ORDER BY DepartmentName", connection);
        using var reader = command.ExecuteReader();
        var result = new List<Department>();
        while (reader.Read())
        {
            result.Add(new Department
            {
                DepartmentId = reader.GetInt32(0),
                DepartmentName = reader.GetString(1),
                Location = reader.IsDBNull(2) ? null : reader.GetString(2),
                IsActive = reader.GetBoolean(3)
            });
        }
        return result;
    }

    public void Deactivate(int id) => ExecuteStatusUpdate("Departments", "DepartmentId", id);

    private static void ExecuteStatusUpdate(string table, string key, int id)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();
        using var command = new SqlCommand($"UPDATE {table} SET IsActive = 0 WHERE {key} = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);
        command.ExecuteNonQuery();
    }
}
