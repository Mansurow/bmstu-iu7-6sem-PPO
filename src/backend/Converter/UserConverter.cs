using Anticafe.BL.Models;
using Anticafe.DataAccess.DBModels;

namespace Anticafe.DataAccess.Converter;

public static class UserConverter
{
    public static User ConvertDbModelToAppModel(UserDbModel user) 
    {
        return new User(id: user.Id,
                        surname: user.Surname,
                        name: user.Name,
                        firstName: user.FirstName,
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
                                surname: user.Surname,
                                name: user.Name,
                                firstName: user.FirstName,
                                birthday: user.Birthday,
                                gender: user.Gender,
                                email: user.Email,
                                phone: user.Phone,
                                passwordHash: user.PasswordHash,
                                role: user.Role);
    }
}
