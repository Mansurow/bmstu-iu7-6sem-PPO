using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Portal.Common.JsonConverter;
using Portal.Common.Models.Enums;

namespace Portal.Common.Models.Dto;

/// <summary>
/// Модель для регистрации пользователя
/// </summary>
public class CreateUserDto
{
    /// <summary>
    /// Фамилия пользователя
    /// </summary>
    /// <example>Иванов</example>
    public string LastName { get; set; }
    
    /// <summary>
    /// Имя пользователя
    /// </summary>
    /// <example>Иван</example>
    public string FirstName { get; set; }
    
    /// <summary>
    /// Отчество пользователя
    /// </summary>
    /// <example>Иванович</example>
    public string? MiddleName { get; set; }
    
    /// <summary>
    /// Дата рождения пользователя
    /// </summary>
    /// <example>12.08.2002</example>
    [JsonConverter(typeof(CustomDateOnlyConverter))]
    public DateOnly Birthday { get; set; }
    
    /// <summary>
    /// Пол пользователя
    /// </summary>
    /// <example>Male</example>
    public Gender Gender { get; set; }
    
    /// <summary>
    /// Email пользователя
    /// </summary>
    /// <example>user.portal@gmail.com</example>
    [EmailAddress]
    public string Email { get; set; }
    
    /// <summary>
    /// Номер телефона пользователя
    /// </summary>
    /// <example>89268899772</example>
    [Phone]
    public string Phone { get; set; }
    
    /// <summary>
    /// Пароль 
    /// </summary>
    /// <example>password123</example>
    public string Password { get; private set; }
    
    public CreateUserDto(string lastName, string firstName, string middleName, DateOnly birthday, Gender gender, string email, string phone, string password)
    {
        LastName = lastName;
        FirstName = firstName;
        MiddleName = middleName;
        Birthday = birthday;
        Gender = gender;
        Email = email;
        Phone = phone;
        Password = password;
    }
}