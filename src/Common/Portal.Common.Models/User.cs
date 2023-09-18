using Portal.Common.Models.Enums;

namespace Portal.Common.Models;

/// <summary>
/// Пользователь
/// </summary>
public class User
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    /// <example>f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6</example>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Фамилия пользователя
    /// </summary>
    /// <example>Иванов</example>
    public string LastName { get; set; }
    
    /// <summary>
    /// Имя пользовтеля
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
    /// <example>05.12.1999</example>
    public DateOnly Birthday { get; set; }
    
    /// <summary>
    /// Пол пользователя
    /// </summary>
    /// <example>Male</example>
    public Gender Gender { get; set; }
    
    /// <summary>
    /// Email пользовтеля
    /// </summary>
    /// <example>user.portal@gmail.com</example>
    public string Email { get; set; }
    
    /// <summary>
    /// Номер телефона пользователя
    /// </summary>
    /// <example>89268899772</example>
    public string? Phone { get; set; }
    
    /// <summary>
    /// Зашифрованный пароль 
    /// </summary>
    public string? PasswordHash { get; private set; }
    
    /// <summary>
    /// Права доступа
    /// </summary>
    public Role Role { get; private set; }

    public User(Guid id, string lastName, string firstName, string middleName, DateOnly birthday, Gender gender, string email, string? phone = null, string? passwordHash = null, Role role = Role.User)
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

    public void ChangePermission(Role role)
    {
        Role = role;
    }

    public void CreateHash(string password)
    {
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        var other = (User) obj;
        return Id == other.Id
               && LastName == other.LastName
               && FirstName == other.FirstName
               && LastName == other.LastName
               && Birthday == other.Birthday
               && Gender == other.Gender
               && Email == other.Email
               && Phone == other.Phone
               && PasswordHash == other.PasswordHash
               && Role == other.Role;
    }
}
