using System.Security.Cryptography;
using Npgsql.Replication;
using Portal.Common.Models;
using Portal.Common.Models.Enums;
using Portal.Services.UserService.Exceptions;

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
    /// <exception cref="UserNotFoundException">Пользователь не найден</exception>
    Task<User> GetUserByIdAsync(Guid userId);
    
    /// <summary>
    /// Изменить права доступа
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="permissions">Новые права доступа</param>
    /// <exception cref="UserNotFoundException">Пользователь не найден</exception>
    /// <exception cref="UserUpdateException">При обновлении прав доступа</exception>
    Task ChangeUserPermissionsAsync(Guid userId, Role permissions);

    /// <summary>
    /// Создать администратора
    /// </summary>
    /// <param name="login">Логин</param>
    /// <param name="password">Пароль</param>
    /// <exception cref="UserCreateException">При создании администратора</exception>
    Task CreateAdmin(string login, string password);
}
