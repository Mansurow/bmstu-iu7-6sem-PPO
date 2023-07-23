namespace Portal.Common.Models.Enums;

/// <summary>
/// Права доступа
/// </summary>
public enum Role
{
    /// <summary>
    /// Неавторизованный пользователь
    /// </summary>
    UnAuthorized = 0,
    
    /// <summary>
    /// Администратор
    /// </summary>
    Administrator = 1,
    
    /// <summary>
    /// Пользователь с простым правами доступа
    /// </summary>
    User = 2,
    
    /// <summary>
    /// Сотрудник
    /// </summary>
    Employee = 3,
}
