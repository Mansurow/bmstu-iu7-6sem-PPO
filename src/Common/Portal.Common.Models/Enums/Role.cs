using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Portal.Common.Models.Enums;

/// <summary>
/// Права доступа
/// </summary>
public enum Role
{
    /// <summary>
    /// Неавторизованный пользователь
    /// </summary>
    [EnumMember(Value = "Неавторизован")]
    UnAuthorized = 0,
    
    /// <summary>
    /// Администратор
    /// </summary>
    [EnumMember(Value = "Администратор")]
    Administrator = 1,
    
    /// <summary>
    /// Пользователь с простым правами доступа
    /// </summary>
    [EnumMember(Value = "Пользователь")]
    User = 2,
    
    /// <summary>
    /// Сотрудник
    /// </summary>
    [EnumMember(Value = "Сотрудник")]
    Employee = 3,
}
