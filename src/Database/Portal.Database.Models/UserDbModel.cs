using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Portal.Common.Models.Enums;

namespace Portal.Database.Models;

/// <summary>
/// Модель базы данных пользователь
/// </summary>
[Table("users")]
public class UserDbModel
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Фамилия пользователя
    /// </summary>
    [Column("last_name", TypeName = "varchar(64)")]
    public string LastName { get; set; }
    
    /// <summary>
    /// Имя пользователя
    /// </summary>
    [Column("first_name", TypeName = "varchar(64)")]
    public string FirstName { get; set; }
    
    /// <summary>
    /// Отчество пользователя
    /// </summary>
    [Column("middle_name", TypeName = "varchar(64)")]
    public string? MiddleName { get; set; }
    
    /// <summary>
    /// Дата рождения
    /// </summary>
    [Column("birthday")]
    public DateOnly Birthday { get; set; }
    
    /// <summary>
    /// Пол (гендер) пользователя - может быть не указан
    /// </summary>
    [Column("gender")]
    public Gender Gender { get; set; }
    
    /// <summary>
    /// Электронная почта
    /// </summary>
    [Column("email")]
    public string Email { get; set; }
    
    /// <summary>
    /// Номер телефона
    /// </summary>
    [Column("phone")]
    public string? Phone { get; set; }
    
    /// <summary>
    /// Хеш-пароль 
    /// </summary>
    [Column("password", TypeName = "varchar(128)")]
    public string? PasswordHash { get; set; }
    
    /// <summary>
    /// Права доступа
    /// </summary>
    [Column("role", TypeName = "varchar(64)")]
    public Role Role { get; set; }

    public UserDbModel(Guid id, string lastName, string firstName, string? middleName, 
        DateOnly birthday, Gender gender, string email, string? phone, string? passwordHash, 
        Role role = Role.User)
    {
        Id = id;
        LastName = lastName;
        FirstName = firstName;
        MiddleName = middleName;
        Birthday = birthday;
        Gender = gender;
        Email = email;
        Phone = phone;
        PasswordHash = passwordHash;
        Role = role;
    }
}
