namespace SessionDurationModel;

public struct SessionDuration
{
    public SessionDuration(int days)
    {
        if (days <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(days), "Duration must be greater than zero.");
        }

        Days = days;
    }

    public int Days { get; }

    public override string ToString()
    {
        return $"{Days} day{(Days == 1 ? string.Empty : "s")}";
    }
}
