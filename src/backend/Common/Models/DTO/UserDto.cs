using Anticafe.Common.Enums;
using Newtonsoft.Json;

namespace Common.Models.DTO;

[JsonObject]
public class UserDto
{
    [JsonProperty("userId")]
    public int Id { get; set; }
    [JsonProperty("surname")]
    public string Surname { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("firstName")]
    public string FirstName { get; set; }
    public DateTime Birthday { get; set; }
    [JsonProperty("gender")]
    public Gender Gender { get; set; }
    [JsonProperty("email")]
    public string Email { get; set; }
    [JsonProperty("phone")]
    public string Phone { get; set; }
    [JsonProperty("password")]
    public string? PasswordHash { get; set; }
    [JsonProperty("role")]
    public UserRole Role { get; set; }

    public UserDto(int id, string surname, string name, string firstName, DateTime birthday, Gender gender, string email, string phone, string? passwordHash, UserRole role)
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
