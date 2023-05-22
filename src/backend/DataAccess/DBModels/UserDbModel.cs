using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Anticafe.Common.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Anticafe.DataAccess.DBModels;

public class UserDbModel
{
    [Key]
    [Column("id")]
    [BsonId]
    public int Id { get; set; }
    [Column("surname", TypeName = "varchar(64)")]
    public string Surname { get; set; }
    [Column("name", TypeName = "varchar(64)")]
    public string Name { get; set; }
    [Column("first_name", TypeName = "varchar(64)")]
    public string FirstName { get; set; }
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
    public UserRole Role { get; set; }

    public UserDbModel(int id, string surname, string name, string firstName, DateTime birthday, Gender gender, string email, string phone, string? passwordHash, UserRole role = UserRole.User)
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
}
