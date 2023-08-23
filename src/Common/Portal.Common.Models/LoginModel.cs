namespace Portal.Common.Models;

/// <summary>
/// Модель для авторизации
/// </summary>
public class LoginModel
{
    public LoginModel(string login, string password)
    {
        Login = login;
        Password = password;
    }

    /// <summary>
    /// Логин
    /// </summary>
    /// <example>user@gmail.com</example>
    public string Login { get; set; }

    /// <summary>
    /// Пароль 
    /// </summary>
    /// <example>password123</example>
    public string Password { get; set; }
}