using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Portal.Common.Models.Enums;

namespace Portal.Database.Models;

public class UserDbModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    [Column("last_name", TypeName = "varchar(64)")]
    public string LastName { get; set; }
    [Column("first_name", TypeName = "varchar(64)")]
    public string FirstName { get; set; }
    [Column("middle_name", TypeName = "varchar(64)")]
    public string MiddleName { get; set; }
    [Column("birthday", TypeName = "varchar(64)")]
    public DateTime Birthday { get; set; }
    [Column("gender", TypeName = "varchar(64)")]
    public Gender Gender { get; set; }
    [Column("email", TypeName = "varchar(64)")]
    public string Email { get; set; }
    [Column("phone", TypeName = "varchar(64)")]
    public string Phone { get; set; }
    [Column("password", TypeName = "varchar(128)")]
    public string? PasswordHash { get; set; }
    [Column("role", TypeName = "varchar(64)")]
    public Role Role { get; set; }

    public UserDbModel(Guid id, string lastName, string firstName, string middleName, 
        DateTime birthday, Gender gender, string email, string phone, string? passwordHash, 
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
