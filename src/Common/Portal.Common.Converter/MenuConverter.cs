using DishCore = Portal.Common.Core.Dish;
using DishDB = Portal.Database.Models.DishDbModel;
using DishDto = Portal.Common.Dto.Dish.Dish;

namespace Portal.Common.Converter;

/// <summary>
/// Конвертатор модели Dish
/// </summary>
public static class MenuConverter
{
    /// <summary>
    /// Преобразовать из модели базы данных в модель бизнес логики приложения
    /// </summary>
    /// <param name="dish">Модель базы данных</param>
    /// <returns>Модель бизнес логики</returns>
    public static DishCore ConvertDBToCoreModel(DishDB dish) 
    {
        return new DishCore(id: dish.Id,
                        name: dish.Name,
                        type: dish.Type,
                        price: dish.Price,
                        description: dish.Description);
    }

    /// <summary>
    /// Преобразовать из модели бизнес логики в модели базы данных приложения
    /// </summary>
    /// <param name="dish">Модель бизнес логики</param>
    /// <returns>Модель базы данных </returns>
    public static DishDB ConvertCoreToDBModel(DishCore dish)
    {
        return new DishDB(id: dish.Id,
                        name: dish.Name,
                        type: dish.Type,
                        price: dish.Price,
                        description: dish.Description);
    }
    
    /// <summary>
    /// Преобразовать из модели бизнес логики в модели DTO
    /// </summary>
    /// <param name="dish">Модель бизнес логики</param>
    /// <returns>Модель DTO</returns>
    public static DishDto ConvertCoreToDtoModel(DishCore dish)
    {
        return new DishDto(id: dish.Id,
            name: dish.Name,
            type: dish.Type,
            price: dish.Price,
            description: dish.Description);
    }
    
    /// <summary>
    /// Преобразовать из модели DTO в модели бизнес логики
    /// </summary>
    /// <param name="dish">Модель DTO</param>
    /// <returns>Модель бизнес логики</returns>
    public static DishCore ConvertDtoToCoreModel(DishDto dish)
    {
        return new DishCore(id: dish.Id,
            name: dish.Name,
            type: dish.Type,
            price: dish.Price,
            description: dish.Description);
    }
}
