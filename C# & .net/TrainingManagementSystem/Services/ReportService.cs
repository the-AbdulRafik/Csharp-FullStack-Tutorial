using CourseModel;
using EnrollmentModel;
using PaymentModel;

namespace ReportServicePkg;

public class ReportService
{
    public void DisplayCourseOccupancyReport(Course[] courses)
    {
        Console.WriteLine("Course Occupancy Report");
        foreach (Course course in courses)
        {
            Console.WriteLine($"{course.CourseCode}: {course.EnrolledCount}/{course.TotalSeats}");
        }
    }

    public void DisplayExpectedFeesReport(Course[] courses, List<Enrollment> enrollments)
    {
        Console.WriteLine("Expected Fees Report");
        foreach (Course course in courses)
        {
            decimal total = 0m;
            foreach (Enrollment enrollment in enrollments)
            {
                if (enrollment.CourseCode == course.CourseCode && enrollment.Status == Enrollment.EnrollmentStatus.Enrolled)
                {
                    total += enrollment.FinalFee;
                }
            }

            Console.WriteLine($"{course.CourseCode}: {total:C}");
        }
    }

    public void DisplayPaymentSummaryReport(List<Payment> payments)
    {
        decimal successfulTotal = 0m;
        int successfulCount = 0;
        foreach (Payment payment in payments)
        {
            if (payment.Status == PaymentStatus.Successful)
            {
                successfulTotal += payment.Amount;
                successfulCount++;
            }
        }

        Console.WriteLine($"Successful payments: {successfulCount}");
        Console.WriteLine($"Successful amount: {successfulTotal:C}");
    }

    public void DisplayCourseEnrollmentSummaryReport(Course[] courses, List<Enrollment> enrollments)
    {
        Console.WriteLine("Course Enrollment Summary");
        foreach (Course course in courses)
        {
            int count = 0;
            foreach (Enrollment enrollment in enrollments)
            {
                if (enrollment.CourseCode == course.CourseCode && enrollment.Status == Enrollment.EnrollmentStatus.Enrolled)
                {
                    count++;
                }
            }

            Console.WriteLine($"{course.CourseCode} - {course.Title}: {count} active enrollment(s)");
        }
    }
}
