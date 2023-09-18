using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Portal.Common.JsonConverter;
using Portal.Common.Models.Enums;

namespace Portal.Common.Models.Dto;

/// <summary>
/// Модель для получения данных о пользователе
/// </summary>
public class UserDto
{
    public UserDto(Guid userId, string lastName, string firstName, string middleName, DateOnly birthday, Gender gender, string email, string phone, Role role)
    {
        UserId = userId;
        LastName = lastName;
        FirstName = firstName;
        MiddleName = middleName;
        Birthday = birthday;
        Gender = gender;
        Email = email;
        Phone = phone;
        Role = role;
    }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }
    
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
    public string MiddleName { get; set; }
    
    /// <summary>
    /// Дата рождения пользователя
    /// </summary>
    /// <example>08.12.2002</example>
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
    /// Права доступа
    /// </summary>
    public Role Role { get; set; }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        var other = (UserDto) obj;
        return UserId == other.UserId
               && LastName == other.LastName
               && FirstName == other.FirstName
               && LastName == other.LastName
               && Birthday == other.Birthday
               && Gender == other.Gender
               && Email == other.Email
               && Phone == other.Phone
               && Role == other.Role;
    }
}