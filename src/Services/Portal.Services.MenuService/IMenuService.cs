using Portal.Common.Models;
using Portal.Common.Models.Enums;
using Portal.Services.MenuService.Exceptions;

namespace Portal.Services.MenuService;

/// <summary>
/// Интерфейс сервиса меню блюд
/// </summary>
public interface IMenuService
{
    /// <summary>
    /// Получить меню блюд
    /// </summary>
    /// <returns>Список блюд</returns>
    Task<List<Dish>> GetAllDishesAsync();
    
    /// <summary>
    /// Получить блюдо
    /// </summary>
    /// <param name="dishId">Идентификатор блюда</param>
    /// <returns>Блюдо</returns>
    /// <exception cref="DishNotFoundException">Блюдо не найдена</exception>
    Task<Dish> GetDishByIdAsync(Guid dishId);
    
    /// <summary>
    /// Добавить блюдо
    /// </summary>
    /// <param name="name">Название блюда</param>
    /// <param name="type">Тип блюда</param>
    /// <param name="price">Цена блюда</param>
    /// <param name="description">Описание блюда</param>
    /// <returns>Идентификатор нового блюда</returns>
    /// <exception cref="DishCreateException">При добавлении блюда</exception>
    Task<Guid> AddDishAsync(string name, DishType type, double price, string description);
    
    /// <summary>
    /// Обновить блюдо
    /// </summary>
    /// <param name="updateDish">Данные блюда для обновления</param>
    /// <exception cref="DishNotFoundException">Блюдо не найден</exception>
    /// <exception cref="DishUpdateException">При обновлении блюда</exception>
    Task UpdateDishAsync(Dish updateDish);
    
    /// <summary>
    /// Удалить блюдо
    /// </summary>
    /// <param name="dishId">Идентификатор блюда</param>
    /// <exception cref="DishNotFoundException">Блюдо не найден</exception>
    /// <exception cref="DishRemoveException">При удалении блюда</exception>
    Task RemoveDishAsync(Guid dishId);
}
