using Portal.Common.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Portal.Common.Models;

public class User
{
    public Guid Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public DateTime Birthday { get; set; }
    public Gender Gender { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    [Phone]
    public string Phone { get; set; }
    public string? PasswordHash { get; set; }
    public Role Role { get; set; }

    public User(Guid id, string lastName, string firstName, string middleName, DateTime birthday, Gender gender, string email, string phone, string? passwordHash = null, Role role = Role.User)
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
}
