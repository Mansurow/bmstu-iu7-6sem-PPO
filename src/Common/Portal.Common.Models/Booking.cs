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
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Идентификатор зоны
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    public Guid ZoneId { get; set; }
    
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Идентификатор пакета 
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    public Guid PackageId { get; set; }
    
    /// <summary>
    /// Количество людей в брони
    /// </summary>
    /// <example>10</example>
    public int AmountPeople { get; set; }
    
    /// <summary>
    /// Статус брони
    /// </summary>
    public BookingStatus Status { get; set; }
    
    /// <summary>
    /// Дата бронирования
    /// </summary>
    /// <example>10.08.2023</example>
    public DateOnly Date { get; set; }
    
    /// <summary>
    /// Начало времени брони
    /// </summary>
    /// <example>12:00:00</example>
    public TimeOnly StartTime { get; set; }
    
    /// <summary>
    /// Конец времени брони
    /// </summary>
    /// <example>18:00:00</example>
    public TimeOnly EndTime { get; set; }

    /// <summary>
    /// Время создания брони
    /// </summary>
    public DateTime CreateDateTime { get; set; }

    /// <summary>
    /// Оплачена ли бронь
    /// </summary>
    public bool IsPaid { get; set; }
    
    /// <summary>
    /// Общая стоимоcть брони
    /// </summary>
    public double TotalPrice { get; set; }
    
    public Booking(Guid id, Guid zoneId, Guid userId, Guid packageId,
        int amountPeople, BookingStatus status, DateOnly date, TimeOnly startTime, TimeOnly endTime, 
        DateTime createDateTime, bool isPaid, double totalPrice)
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
        CreateDateTime = createDateTime;
        IsPaid = isPaid;
        TotalPrice = totalPrice;
    }

    public bool IsActualStatus()
    {
        return Status != BookingStatus.Done 
               && Status != BookingStatus.Cancelled;
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
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        var other = (Booking) obj;
        return Id == other.Id
               && UserId == other.UserId
               && ZoneId == other.ZoneId
               && PackageId == other.PackageId
               && AmountPeople == other.AmountPeople
               // && Status == other.Status
               && StartTime.Equals(other.StartTime)
               && EndTime.Equals(other.EndTime)
               && Date.Equals(other.Date);
    }
}