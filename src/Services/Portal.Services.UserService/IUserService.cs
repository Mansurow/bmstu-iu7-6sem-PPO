using Portal.Common.Models;

namespace Portal.Services.UserService;

/// <summary>
/// Интерфейс сервиса пользователя
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Получить всех пользователей
    /// </summary>
    /// <returns>Список всех пользователей</returns>
    Task<List<User>> GetAllUsersAsync();
    
    /// <summary>
    /// Получить дынные пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns>Данные пользователя</returns>
    Task<User> GetUserByIdAsync(Guid userId);
    
    /// <summary>
    /// Изменить права доступа
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns></returns>
    Task ChangeUserPermissionsAsync(Guid userId);
}
