namespace Portal.Common.Models.Dto;

/// <summary>
/// Модель для создания брони 
/// </summary>
public class CreateBookingDto
{
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