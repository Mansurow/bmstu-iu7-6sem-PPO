using System.ComponentModel.DataAnnotations;

namespace Portal.Common.Dto.Booking;

/// <summary>
/// Модель для подтверждении брони
/// </summary>
public class ConfirmBooking
{
    /// <summary>
    /// Идентификатор брони
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid Id { get; set; }

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
    
    public ConfirmBooking(Guid id, Guid packageId, int amountPeople, DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        Id = id;
        PackageId = packageId;
        AmountPeople = amountPeople;
        Date = date;
        StartTime = startTime;
        EndTime = endTime;
    }
}