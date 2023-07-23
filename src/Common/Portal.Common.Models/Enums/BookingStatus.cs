namespace Portal.Common.Models.Enums;

/// <summary>
/// Статус брони
/// </summary>
public enum BookingStatus
{
    /// <summary>
    /// Временная бронь зоны
    /// </summary>
    TemporaryReserved = 1,
    
    /// <summary>
    /// Забронировано
    /// </summary>
    Reserved = 2,
    
    /// <summary>
    /// Неактуальная бронь (время брони вышло)
    /// </summary>
    NoActual = 3,
    
    /// <summary>
    /// Отмененная бронь
    /// </summary>
    Cancelled = 4
}
