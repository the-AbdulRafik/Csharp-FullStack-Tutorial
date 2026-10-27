using SessionDurationModel;

namespace CourseSessionPkg;

public sealed class OnlineSession : CourseSession
{
    private const decimal OnlineSurcharge = 200m;

    public OnlineSession(string courseCode, DateTime startDate, SessionDuration duration)
        : base(courseCode, startDate, duration)
    {
    }

    public override decimal CalculateSessionCharges()
    {
        return OnlineSurcharge;
    }
}
