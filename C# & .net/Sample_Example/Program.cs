using Microsoft.Data.SqlClient;
using Sample_Example.Repositories;
using Sample_Example.Services;

var departments = new DepartmentService(new DepartmentRepository());
var students = new StudentService(new StudentRepository());
var courses = new CourseService(new CourseRepository());
var enrollments = new EnrollmentService(new EnrollmentRepository());
var auditLogs = new AuditLogService(new AuditLogRepository());

while (true)
{
    Console.WriteLine("\n=== Student Course Management ===");
    Console.WriteLine("1. Add department");
    Console.WriteLine("2. List departments");
    Console.WriteLine("3. Add student");
    Console.WriteLine("4. List/search students");
    Console.WriteLine("5. Deactivate student");
    Console.WriteLine("6. Add course");
    Console.WriteLine("7. List courses");
    Console.WriteLine("8. Deactivate course");
    Console.WriteLine("9. Enroll student");
    Console.WriteLine("10. List enrollments");
    Console.WriteLine("11. Complete enrollment");
    Console.WriteLine("12. Cancel enrollment");
    Console.WriteLine("13. View audit logs");
    Console.WriteLine("0. Exit");
    Console.Write("Choose: ");

    try
    {
        switch (Console.ReadLine())
        {
            case "1":
                Console.Write("Department name: ");
                var departmentName = Console.ReadLine() ?? string.Empty;
                Console.Write("Location: ");
                departments.Add(departmentName, Console.ReadLine());
                Console.WriteLine("Department added.");
                break;
            case "2":
                foreach (var department in departments.GetAll())
                    Console.WriteLine($"{department.DepartmentId}: {department.DepartmentName} ({department.Location})");
                break;
            case "3":
                Console.Write("Name: ");
                var name = Console.ReadLine() ?? string.Empty;
                Console.Write("Email: ");
                var email = Console.ReadLine() ?? string.Empty;
                Console.Write("Age: ");
                var age = int.Parse(Console.ReadLine() ?? string.Empty);
                Console.Write("Department ID: ");
                students.Add(name, email, age, int.Parse(Console.ReadLine() ?? string.Empty));
                Console.WriteLine("Student added.");
                break;
            case "4":
                Console.Write("Search text, or press Enter for all: ");
                foreach (var student in students.Search(Console.ReadLine()))
                    Console.WriteLine($"{student.StudentId}: {student.Name} | {student.Email} | {student.DepartmentName}");
                break;
            case "5":
                students.Deactivate(ReadInt("Student ID: "));
                Console.WriteLine("Student deactivated.");
                break;
            case "6":
                Console.Write("Course name: ");
                var courseName = Console.ReadLine() ?? string.Empty;
                var duration = ReadInt("Duration in months: ");
                Console.Write("Fee: ");
                var fee = decimal.Parse(Console.ReadLine() ?? string.Empty);
                courses.Add(courseName, duration, fee, ReadInt("Department ID: "));
                Console.WriteLine("Course added.");
                break;
            case "7":
                foreach (var course in courses.GetAll())
                    Console.WriteLine($"{course.CourseId}: {course.CourseName} | {course.DurationMonths} months | {course.Fee:C}");
                break;
            case "8":
                courses.Deactivate(ReadInt("Course ID: "));
                Console.WriteLine("Course deactivated.");
                break;
            case "9":
                enrollments.Enroll(ReadInt("Student ID: "), ReadInt("Course ID: "));
                Console.WriteLine("Student enrolled.");
                break;
            case "10":
                foreach (var enrollment in enrollments.GetAll())
                    Console.WriteLine($"{enrollment.EnrollmentId}: {enrollment.StudentName} - {enrollment.CourseName} ({enrollment.Status})");
                break;
            case "11":
                enrollments.Complete(ReadInt("Enrollment ID: "));
                Console.WriteLine("Enrollment completed.");
                break;
            case "12":
                enrollments.Cancel(ReadInt("Enrollment ID: "));
                Console.WriteLine("Enrollment cancelled.");
                break;
            case "13":
                foreach (var log in auditLogs.GetRecent())
                    Console.WriteLine($"{log.CreatedAt}: {log.EntityName} {log.Action} - {log.Details}");
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }
    catch (FormatException)
    {
        Console.WriteLine("Please enter a valid number.");
    }
    catch (SqlException exception)
    {
        Console.WriteLine($"Database error: {exception.Message}");
    }
}

static int ReadInt(string message)
{
    Console.Write(message);
    return int.Parse(Console.ReadLine() ?? string.Empty);
}
