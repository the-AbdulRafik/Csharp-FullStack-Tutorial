using Microsoft.Data.SqlClient;
using Sample_Example.Helpers;
using Sample_Example.Models;

namespace Sample_Example.Repositories;

public class EnrollmentRepository
{
    public void Enroll(int studentId, int courseId)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();
        try
        {
            using var enrollment = new SqlCommand(
                "INSERT INTO Enrollments (StudentId, CourseId) VALUES (@StudentId, @CourseId)",
                connection, transaction);
            enrollment.Parameters.AddWithValue("@StudentId", studentId);
            enrollment.Parameters.AddWithValue("@CourseId", courseId);
            enrollment.ExecuteNonQuery();

            using var audit = new SqlCommand("""
                INSERT INTO AuditLogs (EntityName, EntityId, Action, Details)
                VALUES ('Enrollment', @Id, 'CREATE', 'Student enrolled in course')
                """, connection, transaction);
            audit.Parameters.AddWithValue("@Id", studentId);
            audit.ExecuteNonQuery();
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public List<Enrollment> GetAll()
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();
        using var command = new SqlCommand("""
            SELECT e.EnrollmentId, e.StudentId, e.CourseId, s.Name, c.CourseName,
                   e.Status, e.EnrolledAt
            FROM Enrollments e
            JOIN Students s ON s.StudentId = e.StudentId
            JOIN Courses c ON c.CourseId = e.CourseId
            ORDER BY e.EnrollmentId
            """, connection);
        using var reader = command.ExecuteReader();
        var result = new List<Enrollment>();
        while (reader.Read())
        {
            result.Add(new Enrollment
            {
                EnrollmentId = reader.GetInt32(0), StudentId = reader.GetInt32(1),
                CourseId = reader.GetInt32(2), StudentName = reader.GetString(3),
                CourseName = reader.GetString(4), Status = reader.GetString(5),
                EnrolledAt = reader.GetDateTime(6)
            });
        }
        return result;
    }

    public void UpdateStatus(int id, string status)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();
        using var command = new SqlCommand(
            "UPDATE Enrollments SET Status = @Status WHERE EnrollmentId = @Id", connection);
        command.Parameters.AddWithValue("@Status", status);
        command.Parameters.AddWithValue("@Id", id);
        command.ExecuteNonQuery();
    }
}
