using System.Runtime.Serialization;

namespace Portal.Common.Models.Enums;

/// <summary>
/// Тип пакетов-акций 
/// </summary>
public enum PackageType
{
    /// <summary>
    /// Обычный - по часовой оплаты
    /// </summary>
    [EnumMember(Value = "Обычный")]
    Usual = 0,

    /// <summary>
    /// Простые акции
    /// </summary>
    [EnumMember(Value = "Простые акции")]
    Simple = 1,

    /// <summary>
    /// Интерактивный режим 
    /// </summary>
    [EnumMember(Value = "Интерактивный режим")]
    Interactive = 2,

    /// <summary>
    /// Праздники
    /// </summary>
    [EnumMember(Value = "Праздничный")]
    Holidays = 3
}
