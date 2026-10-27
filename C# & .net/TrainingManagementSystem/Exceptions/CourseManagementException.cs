namespace CourseManagementExceptions;

public class CourseManagementException : Exception
{
    public CourseManagementException(string message)
        : base(message)
    {
    }

    public CourseManagementException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
