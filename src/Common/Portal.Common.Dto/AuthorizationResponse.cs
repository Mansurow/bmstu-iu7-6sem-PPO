using System.ComponentModel.DataAnnotations;

namespace Portal.Common.Dto;

/// <summary>
/// Ответ после регистрации или авторизации
/// </summary>
public class AuthorizationResponse
{
    public AuthorizationResponse(Guid userId, string accessToken)
    {
        UserId = userId;
        AccessToken = accessToken;
    }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    [Required]
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Токен для авторизации на сервере
    /// </summary>
    /// <example>"Bearer {token}</example>
    [Required]
    public string AccessToken { get; set; }
}