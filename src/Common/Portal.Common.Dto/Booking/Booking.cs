using System.ComponentModel.DataAnnotations;
using Portal.Common.Enums;

namespace Portal.Common.Dto.Booking;

public class Booking
{
    /// <summary>
    /// Идентификатор брони
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Идентификатор зоны
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid ZoneId { get; set; }
    
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Идентификатор пакета 
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid PackageId { get; set; }
    
    /// <summary>
    /// Количество людей в брони
    /// </summary>
    /// <example>10</example>
    [Required]
    public int AmountPeople { get; set; }
    
    /// <summary>
    /// Статус брони
    /// </summary>
    [Required]
    public BookingStatus Status { get; set; }
    
    /// <summary>
    /// Дата бронирования
    /// </summary>
    /// <example>10.08.2023</example>
    [Required]
    public DateOnly Date { get; set; }
    
    /// <summary>
    /// Начало времени брони
    /// </summary>
    /// <example>12:00:00</example>
    [Required]
    public TimeOnly StartTime { get; set; }
    
    /// <summary>
    /// Конец времени брони
    /// </summary>
    /// <example>18:00:00</example>
    [Required]
    public TimeOnly EndTime { get; set; }

    /// <summary>
    /// Время создания брони
    /// </summary>
    [Required]
    public DateTime CreateDateTime { get; set; }

    /// <summary>
    /// Оплачена ли бронь
    /// </summary>
    [Required]
    public bool IsPaid { get; set; }
    
    /// <summary>
    /// Общая стоимоcть брони
    /// </summary>
    [Required]
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
}