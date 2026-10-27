using SessionDurationModel;

namespace CourseSessionPkg;

public abstract class CourseSession
{
    protected CourseSession(string courseCode, DateTime startDate, SessionDuration duration)
    {
        if (string.IsNullOrWhiteSpace(courseCode))
        {
            throw new ArgumentException("Course code is required.", nameof(courseCode));
        }

        CourseCode = courseCode;
        StartDate = startDate;
        Duration = duration;
    }

    public string CourseCode { get; }
    public DateTime StartDate { get; }
    public SessionDuration Duration { get; }

    public abstract decimal CalculateSessionCharges();
}
