using Microsoft.Data.SqlClient;
using Sample_Example.Helpers;
using Sample_Example.Models;

namespace Sample_Example.Repositories;

public class StudentRepository
{
    public void Add(Student student)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();
        using var command = new SqlCommand("""
            INSERT INTO Students (Name, Email, Age, DepartmentId)
            VALUES (@Name, @Email, @Age, @DepartmentId)
            """, connection);
        command.Parameters.AddWithValue("@Name", student.Name);
        command.Parameters.AddWithValue("@Email", student.Email);
        command.Parameters.AddWithValue("@Age", student.Age);
        command.Parameters.AddWithValue("@DepartmentId", student.DepartmentId);
        command.ExecuteNonQuery();
    }

    public List<Student> GetAll(bool activeOnly = false, string? search = null)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();
        using var command = new SqlCommand("""
            SELECT s.StudentId, s.Name, s.Email, s.Age, s.DepartmentId,
                   d.DepartmentName, s.AdmissionDate, s.IsActive
            FROM Students s JOIN Departments d ON d.DepartmentId = s.DepartmentId
            WHERE (@Search IS NULL OR s.Name LIKE '%' + @Search + '%' OR s.Email LIKE '%' + @Search + '%')
              AND (@ActiveOnly = 0 OR s.IsActive = 1)
            ORDER BY s.StudentId
            """, connection);
        command.Parameters.AddWithValue("@Search", (object?)search ?? DBNull.Value);
        command.Parameters.AddWithValue("@ActiveOnly", activeOnly);
        using var reader = command.ExecuteReader();
        var result = new List<Student>();
        while (reader.Read())
        {
            result.Add(new Student
            {
                StudentId = reader.GetInt32(0), Name = reader.GetString(1),
                Email = reader.GetString(2), Age = reader.GetInt32(3),
                DepartmentId = reader.GetInt32(4), DepartmentName = reader.GetString(5),
                AdmissionDate = reader.GetDateTime(6), IsActive = reader.GetBoolean(7)
            });
        }
        return result;
    }

    public void Deactivate(int id)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();
        using var command = new SqlCommand(
            "UPDATE Students SET IsActive = 0 WHERE StudentId = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);
        command.ExecuteNonQuery();
    }
}
