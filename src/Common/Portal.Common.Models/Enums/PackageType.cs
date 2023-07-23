namespace Portal.Common.Models.Enums;

/// <summary>
/// Тип пакетов-акций 
/// </summary>
public enum PackageType
{
    /// <summary>
    /// Обычный - по часовой оплаты
    /// </summary>
    Usual = 0,

    /// <summary>
    /// Простые акции
    /// </summary>
    Simple = 1,

    /// <summary>
    /// Интерактивный режим 
    /// </summary>
    Interactive = 2,

    /// <summary>
    /// Праздники
    /// </summary>
    Holidays = 3
}
