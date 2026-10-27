using SessionDurationModel;

namespace CourseSessionPkg;

public sealed class ClassroomSession : CourseSession
{
    private const decimal ClassroomSurcharge = 800m;

    public ClassroomSession(string courseCode, DateTime startDate, SessionDuration duration)
        : base(courseCode, startDate, duration)
    {
    }

    public override decimal CalculateSessionCharges()
    {
        return ClassroomSurcharge;
    }
}
