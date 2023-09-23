using Portal.Common.Models;
using Portal.Common.Models.Dto;
using Portal.Database.Models;

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
    public static User ConvertDtoModelToAppModel(CreateUserDto user) 
    {
        return new User(id: Guid.NewGuid(), 
            firstName: user.FirstName,
            middleName: user.MiddleName,
            lastName: user.LastName,
            birthday: user.Birthday,
            gender: user.Gender,
            email: user.Email,
            phone: user.Phone);
    }
    
    public static UserDto ConvertAppModelToUserDto(User user) 
    {
        return new UserDto(userId: user.Id, 
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

    /// <summary>
    /// Преобразовать из модели бизнес логики в модели базы данных приложения
    /// </summary>
    /// <param name="user">Модель бизнес логики</param>
    /// <returns>Модель базы данных </returns>
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
