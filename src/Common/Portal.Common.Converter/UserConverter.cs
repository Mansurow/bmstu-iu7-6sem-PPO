using Portal.Common.Dto.User;
using UserCore = Portal.Common.Core.User;
using UserDB = Portal.Database.Models.UserDbModel;
using UserDto = Portal.Common.Dto.User.User;

namespace Portal.Common.Converter;

/// <summary>
/// Конвертатор модели Dish
/// </summary>
public static class UserConverter
{
    /// <summary>
    /// Преобразовать из модели Dto в модель бизнес логики приложения
    /// </summary>
    /// <param name="user">Модель Dto</param>
    /// <returns>Модель бизнес логики</returns>
    public static UserCore ConvertDtoToCoreModel(CreateUser user) 
    {
        return new UserCore(id: Guid.NewGuid(), 
            firstName: user.FirstName,
            middleName: user.MiddleName,
            lastName: user.LastName,
            birthday: user.Birthday,
            gender: user.Gender,
            email: user.Email,
            phone: user.Phone);
    }
    
    public static UserDto ConvertCoreToDtoModel(UserCore user) 
    {
        return new UserDto(id: user.Id, 
            firstName: user.FirstName,
            middleName: user.MiddleName,
            lastName: user.LastName,
            birthday: user.Birthday,
            gender: user.Gender,
            email: user.Email,
            phone: user.Phone,
            role: user.Role);
    }
    
    /// <summary>
    /// Преобразовать из модели базы данных в модель бизнес логики приложения
    /// </summary>
    /// <param name="user">Модель базы данных</param>
    /// <returns>Модель бизнес логики</returns>
    public static UserCore ConvertDBToCoreModel(UserDB user) 
    {
        return new UserCore(id: user.Id,
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

    /// <summary>
    /// Преобразовать из модели бизнес логики в модели базы данных приложения
    /// </summary>
    /// <param name="user">Модель бизнес логики</param>
    /// <returns>Модель базы данных </returns>
    public static UserDB ConvertCoreToDBModel(UserCore user)
    {
        return new UserDB(id: user.Id,
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
