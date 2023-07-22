using Portal.Common.Models;
using Portal.Database.Models;

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
    public static Dish ConvertDbModelToAppModel(DishDbModel dish) 
    {
        return new Dish(id: dish.Id,
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
    public static DishDbModel ConvertAppModelToDbModel(Dish dish)
    {
        return new DishDbModel(id: dish.Id,
                        name: dish.Name,
                        type: dish.Type,
                        price: dish.Price,
                        description: dish.Description);
    }
}
