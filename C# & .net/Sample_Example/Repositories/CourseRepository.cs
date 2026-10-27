using Microsoft.Data.SqlClient;
using Sample_Example.Helpers;
using Sample_Example.Models;

namespace Sample_Example.Repositories;

public class CourseRepository
{
    public void Add(Course course)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();
        using var command = new SqlCommand("""
            INSERT INTO Courses (CourseName, DurationMonths, Fee, DepartmentId)
            VALUES (@Name, @Duration, @Fee, @DepartmentId)
            """, connection);
        command.Parameters.AddWithValue("@Name", course.CourseName);
        command.Parameters.AddWithValue("@Duration", course.DurationMonths);
        command.Parameters.AddWithValue("@Fee", course.Fee);
        command.Parameters.AddWithValue("@DepartmentId", course.DepartmentId);
        command.ExecuteNonQuery();
    }

    public List<Course> GetAll(bool activeOnly = false)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();
        using var command = new SqlCommand(
            "SELECT CourseId, CourseName, DurationMonths, Fee, DepartmentId, IsActive FROM Courses " +
            (activeOnly ? "WHERE IsActive = 1 " : "") + "ORDER BY CourseName", connection);
        using var reader = command.ExecuteReader();
        var result = new List<Course>();
        while (reader.Read())
        {
            result.Add(new Course
            {
                CourseId = reader.GetInt32(0), CourseName = reader.GetString(1),
                DurationMonths = reader.GetInt32(2), Fee = reader.GetDecimal(3),
                DepartmentId = reader.GetInt32(4), IsActive = reader.GetBoolean(5)
            });
        }
        return result;
    }

    public void Deactivate(int id)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();
        using var command = new SqlCommand(
            "UPDATE Courses SET IsActive = 0 WHERE CourseId = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);
        command.ExecuteNonQuery();
    }
}
