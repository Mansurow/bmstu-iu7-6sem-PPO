using System.Runtime.Serialization;

namespace Portal.Common.Models.Enums;

/// <summary>
/// Статус брони
/// </summary>
public enum BookingStatus
{
    /// <summary>
    /// Временная бронь зоны
    /// </summary>
    [EnumMember(Value = "Временная бронь")]
    TemporaryReserved = 1,
    
    /// <summary>
    /// Забронировано
    /// </summary>
    [EnumMember(Value = "Забронирована")]
    Reserved = 2,
    
    /// <summary>
    /// Неактуальная бронь (время брони вышло)
    /// </summary>
    [EnumMember(Value = "Готово")]
    Done = 3,
    
    /// <summary>
    /// Отмененная бронь
    /// </summary>
    [EnumMember(Value = "Отменена")]
    Cancelled = 4
}
