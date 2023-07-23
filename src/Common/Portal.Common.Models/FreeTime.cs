namespace Portal.Common.Models;

/// <summary>
/// Пара свободного времени для определения брони
/// </summary>
public class FreeTime
{
    /// <summary>
    /// Начало времени
    /// </summary>
    public TimeOnly StartTime;
    
    /// <summary>
    /// Конец времени
    /// </summary>
    public TimeOnly EndTime;

    public FreeTime(TimeOnly startTime, TimeOnly endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }
    
    public FreeTime(string startTime, string endTime)
    {
        StartTime = TimeOnly.Parse(startTime);
        EndTime = TimeOnly.Parse(endTime);
    }
}