using CourseModel;
using CourseSummaryModel;

namespace TrainingServicePkg;

public class TrainingService
{
    private Course[] courses;

    public TrainingService(Course[] courses)
    {
        this.courses = courses ?? throw new ArgumentNullException(nameof(courses));
    }

    public void DisplayCourses()
    {
        Console.WriteLine("Code\tTitle\t\t\tFee\tAvailable");
        foreach (Course course in courses)
        {
            Console.WriteLine($"{course.CourseCode}\t{course.Title}\t{course.Fee:C}\t{GetAvailableSeats(course)}");
        }
    }

    public Course? FindByCode(string? courseCode)
    {
        if (string.IsNullOrWhiteSpace(courseCode))
        {
            return null;
        }

        foreach (Course course in courses)
        {
            if (string.Equals(course.CourseCode, courseCode.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return course;
            }
        }

        return null;
    }

    public List<Course> SearchByTitle(string? title)
    {
        List<Course> matches = new List<Course>();
        if (string.IsNullOrWhiteSpace(title))
        {
            return matches;
        }

        foreach (Course course in courses)
        {
            if (course.Title.Contains(title.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                matches.Add(course);
            }
        }

        return matches;
    }

    public List<Course> SortByFee(bool descending)
    {
        List<Course> sorted = new List<Course>(courses);
        sorted.Sort((left, right) => descending
            ? right.Fee.CompareTo(left.Fee)
            : left.Fee.CompareTo(right.Fee));
        return sorted;
    }

    public CourseSummary CreateSummary(Course course)
    {
        return new CourseSummary(course.CourseCode, course.Title, course.Fee, GetAvailableSeats(course));
    }

    public static int GetAvailableSeats(Course course)
    {
        return course.TotalSeats - course.EnrolledCount;
    }
}
