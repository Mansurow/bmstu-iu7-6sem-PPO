namespace Portal.Common.Models;

public class FreeTime
{
    public TimeOnly StartTime;
    public TimeOnly EndTime;

    public FreeTime(TimeOnly startTime, TimeOnly endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }
}