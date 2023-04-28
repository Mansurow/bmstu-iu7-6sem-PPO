using Anticafe.Common.Enums;

namespace Anticafe.BL.Models;

public class User
{
    public int Id { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string FirstName { get; set; }
    public DateTime Birthday { get; set; }
    public Gender Gender { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string? PasswordHash { get; set; }
    public UserRole Role { get; set; }
   
    public User (int id, string surname, string name, string firstName, DateTime birthday, Gender gender, string email, string phone, string? passwordHash, UserRole role = UserRole.User)
    {
        Id = id;
        Surname = surname;
        Name = name;
        FirstName = firstName;
        Birthday = birthday;
        Gender = gender;
        Email = email;
        Phone = phone;
        PasswordHash = passwordHash;
        Role = role;
    }

    public User(int id, string surname, string name, string firstName, DateTime birthday, Gender gender, string email, string phone, UserRole role = UserRole.User)
    {
        Id = id;
        Surname = surname;
        Name = name;
        FirstName = firstName;
        Birthday = birthday;
        Gender = gender;
        Email = email;
        Phone = phone;
        PasswordHash = null;
        Role = UserRole.User;
    }

    public void ChangePermission(UserRole role) 
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
