using Portal.Common.Models;

namespace Portal.Database.Core.Repositories;

/// <summary>
/// Интерфейс репозитория пользователя
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Получить всех пользователя
    /// </summary>
    /// <returns>Список пользователей</returns>
    Task<List<User>> GetAllUsersAsync();

    /// <summary>
    /// Получить всех админов
    /// </summary>
    /// <returns>Список админов</returns>
    Task<List<User>> GetAdmins();
    
    /// <summary>
    /// Получить пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns>Пользователь</returns>
    Task<User> GetUserByIdAsync(Guid userId);
    
    /// <summary>
    /// Получить пользователя по электронной почтой
    /// </summary>
    /// <param name="email">Электронная почта</param>
    /// <returns>Пользователь</returns>
    Task<User> GetUserByEmailAsync(string email);
    
    /// <summary>
    /// Добавить пользователя
    /// </summary>
    /// <param name="user">Данные новые пользователя</param>
    /// <returns></returns>
    Task InsertUserAsync(User user);
    
    /// <summary>
    /// Обновить пользователя
    /// </summary>
    /// <param name="user">Данные пользователя для обновления</param>
    /// <returns></returns>
    Task UpdateUserAsync(User user);
    
    /// <summary>
    /// Удалить пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns></returns>
    Task DeleteUserAsync(Guid userId);
}
