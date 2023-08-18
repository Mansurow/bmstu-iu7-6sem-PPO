namespace Portal.Services.BookingService.Configuration;

/// <summary>
/// Конфигурация сервиса бронирования
/// </summary>
public class BookingServiceConfiguration
{
    public BookingServiceConfiguration(string startTimeWorking, string endTimeWorking)
    {
        StartTimeWorking = startTimeWorking;
        EndTimeWorking = endTimeWorking;
    }

    /// <summary>
    /// Время начало рабочего дня
    /// </summary>
    public string StartTimeWorking { get; set; }
    
    /// <summary>
    /// Время конца рабочего дня
    /// </summary>
    public string EndTimeWorking { get; set; }
    
    // public List<string> Holidays { get; set; }
}