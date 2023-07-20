using Portal.Common.Models;
using Portal.Database.Models;

namespace Portal.Common.Converter;

public static class UserConverter
{
    public static User ConvertDbModelToAppModel(UserDbModel user) 
    {
        return new User(id: user.Id,
            firstName: user.FirstName,
            middleName: user.MiddleName,
            lastName: user.LastName,
            birthday: user.Birthday,
            gender: user.Gender,
            email: user.Email,
            phone: user.Phone,
            passwordHash: user.PasswordHash,
            role: user.Role);
    }

    public static UserDbModel ConvertAppModelToDbModel(User user)
    {
        return new UserDbModel(id: user.Id,
            firstName: user.FirstName,
            middleName: user.MiddleName,
            lastName: user.LastName,
            birthday: user.Birthday,
            gender: user.Gender,
            email: user.Email,
            phone: user.Phone,
            passwordHash: user.PasswordHash,
            role: user.Role);
    }
}
