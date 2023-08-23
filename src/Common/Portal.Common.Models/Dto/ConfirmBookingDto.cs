namespace Portal.Common.Models.Dto;

/// <summary>
/// Модель для подтверждении брони
/// </summary>
public class ConfirmBookingDto
{
    /// <summary>
    /// Идентификатор брони
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    public Guid Id { get; set; }

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
}