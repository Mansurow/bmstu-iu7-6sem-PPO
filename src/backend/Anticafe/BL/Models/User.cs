using Anticafe.BL.Enums;

namespace Anticafe.BL.Models;

public class User
{
    public int Id { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string FirstName { get; set; }
    public DateTime Birthday { get; set; }
    public string Gender { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string PasswordHash { get; set; }
    public int Salt { get; set; }
    public UserRole Role { get; set; }
   
    User(int id, string surname, string name, string firstName, DateTime birthday, string gender, string email, string phone, string passwordHash, int salt, UserRole role)
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
        Salt = salt;
        Role = role;
    }
}
