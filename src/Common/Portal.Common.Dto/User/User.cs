using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Portal.Common.Enums;
using Portal.Common.JsonConverter;

namespace Portal.Common.Dto.User;

/// <summary>
/// Модель для получения данных о пользователе
/// </summary>
public class User
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    [Required]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Фамилия пользователя
    /// </summary>
    /// <example>Иванов</example>
    [Required]
    public string LastName { get; set; }
    
    /// <summary>
    /// Имя пользователя
    /// </summary>
    /// <example>Иван</example>
    [Required]
    public string FirstName { get; set; }
    
    /// <summary>
    /// Отчество пользователя
    /// </summary>
    /// <example>Иванович</example>
    public string? MiddleName { get; set; }
    
    /// <summary>
    /// Дата рождения пользователя
    /// </summary>
    /// <example>08.12.2002</example>
    [JsonConverter(typeof(CustomDateOnlyConverter))]
    [Required]
    public DateOnly Birthday { get; set; }
    
    /// <summary>
    /// Пол пользователя
    /// </summary>
    /// <example>Male</example>
    [Required]
    public Gender Gender { get; set; }
    
    /// <summary>
    /// Email пользователя
    /// </summary>
    /// <example>user.portal@gmail.com</example>
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    
    /// <summary>
    /// Номер телефона пользователя
    /// </summary>
    /// <example>89268899772</example>
    [Phone]
    [Required]
    public string Phone { get; set; }
    
    /// <summary>
    /// Права доступа
    /// </summary>
    [Required]
    public Role Role { get; set; }
    
    public User(Guid id, string lastName, string firstName, string? middleName, 
        DateOnly birthday, Gender gender, string email, string phone, Role role)
    {
        Id = id;
        LastName = lastName;
        FirstName = firstName;
        MiddleName = middleName;
        Birthday = birthday;
        Gender = gender;
        Email = email;
        Phone = phone;
        Role = role;
    }
}