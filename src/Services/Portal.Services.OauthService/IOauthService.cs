using Portal.Common.Models;

namespace Portal.Services.OauthService;

/// <summary>
/// Интерфейс сервис авторизации 
/// </summary>
public interface IOauthService
{
    /// <summary>
    ///  Зарегестрировать пользователя
    /// </summary>
    /// <param name="user">Данные пользователя</param>
    /// <param name="password">Пароль в явном виде</param>
    /// <returns></returns>
    Task Registrate(User user, string password);
    
    /// <summary>
    /// Авторизовать пользователя
    /// </summary>
    /// <param name="login">логин - связан с email-ом</param>
    /// <param name="password">пароль</param>
    /// <returns>Данные пользователя</returns>
    Task<User> LogIn(string login, string password);
}
