using System.Runtime.Serialization;

namespace Portal.Common.Models.Enums;

/// <summary>
/// Пол пользователя
/// </summary>
public enum Gender
{
    /// <summary>
    /// Нейзвестный
    /// </summary>
    [EnumMember(Value = "Неизвестный")]
    Unknown = 0,
    
    /// <summary>
    /// Мужчина
    /// </summary>
    [EnumMember(Value = "Мужской")]
    Male = 1,
    
    /// <summary>
    /// Женщина
    /// </summary>
    [EnumMember(Value = "Женский")]
    Female = 2
}
