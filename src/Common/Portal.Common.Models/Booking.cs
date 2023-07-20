using Portal.Common.Models.Enums;

namespace Portal.Common.Models;

/// <summary>
/// Бронь на зону
/// </summary>
public class Booking
{
    /// <summary>
    /// Идентификатор брони
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Идентификатор зоны
    /// </summary>
    public Guid ZoneId { get; set; }
    
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Идентификатор пакета 
    /// </summary>
    public Guid PackageId { get; set; }
    
    /// <summary>
    /// Количество людей в брони
    /// </summary>
    public int AmountPeople { get; set; }
    
    /// <summary>
    /// Статус брони ????
    /// </summary>
    public BookingStatus Status { get; set; }
    
    /// <summary>
    /// Дата бронирования
    /// </summary>
    public DateOnly Date { get; set; }
    
    /// <summary>
    /// Начало времени брони
    /// </summary>
    public TimeOnly StartTime { get; set; }
    
    /// <summary>
    /// Конец времени брони
    /// </summary>
    public TimeOnly EndTime { get; set; }

    public Booking(Guid id, Guid zoneId, Guid userId, Guid packageId,
        int amountPeople, BookingStatus status, DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        Id = id;
        ZoneId = zoneId;
        UserId = userId;
        PackageId = packageId;
        AmountPeople = amountPeople;
        Status = status;
        Date = date;
        StartTime = startTime;
        EndTime = endTime;
    }

    public void ChangeStatus(BookingStatus status)
    {
        Status = status;
    }

    public bool IsBookingExpired()
    {
        return EndTime <= TimeOnly.FromDateTime(DateTime.UtcNow);
    }
}
