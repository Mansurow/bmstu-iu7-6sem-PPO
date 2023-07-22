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

    /// <summary>
    /// Изменить статус 
    /// </summary>
    /// <param name="status">Новый статус</param>
    public void ChangeStatus(BookingStatus status)
    {
        Status = status;
    }

    /// <summary>
    /// Проверка на актуальность брони
    /// </summary>
    /// <returns>Значение о акутальности брони</returns>
    public bool IsBookingExpired()
    {
        return Date == DateOnly.FromDateTime(DateTime.UtcNow) 
               && EndTime <= TimeOnly.FromDateTime(DateTime.UtcNow);
    }

    /// <summary>
    /// Проверка на смену статуса на подходящий статус
    /// </summary>
    /// <param name="status">новый статус</param>
    /// <returns>Значние о правильной смене статуса</returns>
    public bool IsSuitableStatus(BookingStatus status)
    {
        return status >= Status;
    }

    /// <summary>
    /// Изменились ли даты и время брони
    /// </summary>
    /// <param name="booking">Обновленные данные</param>
    /// <returns>Значение о смене даты и время</returns>
    public bool IsChangeDateTime(Booking booking)
    {
        return Date != booking.Date 
               || StartTime != booking.StartTime
               || EndTime != booking.EndTime;
    }
}