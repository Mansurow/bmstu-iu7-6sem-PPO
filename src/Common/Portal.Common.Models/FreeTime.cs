namespace Portal.Common.Models;

/// <summary>
/// Пара свободного времени для определения брони
/// </summary>
public class FreeTime
{
    /// <summary>
    /// Начало времени
    /// </summary>
    /// <example>12:00:00</example>
    public TimeOnly StartTime;
    
    /// <summary>
    /// Конец времени
    /// </summary>
    /// <example>18:00:00</example>
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
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        var other = (FreeTime) obj;
        return StartTime == other.StartTime
               && EndTime == other.EndTime;
    }
}