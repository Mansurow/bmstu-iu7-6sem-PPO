namespace Portal.Common.Models.Dto;

public class CreateBookingDto
{
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
}