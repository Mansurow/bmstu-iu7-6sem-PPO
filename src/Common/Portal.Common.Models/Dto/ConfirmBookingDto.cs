namespace Portal.Common.Models.Dto;

/// <summary>
/// Модель для подтверждении брони
/// </summary>
public class ConfirmBookingDto
{
    /// <summary>
    /// Идентификатор брони
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Идентификатор пакета 
    /// </summary>
    public Guid PackageId { get; set; }
    
    /// <summary>
    /// Количество людей в брони
    /// </summary>
    public int AmountPeople { get; set; }
    
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