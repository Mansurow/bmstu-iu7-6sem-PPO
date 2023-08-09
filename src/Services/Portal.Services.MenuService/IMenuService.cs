using Portal.Common.Models;
using Portal.Common.Models.Enums;

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
    Task<Dish> GetDishByIdAsync(Guid dishId);
    
    /// <summary>
    /// Добавить блюдо
    /// </summary>
    /// <param name="name">Название блюда</param>
    /// <param name="type">Тип блюда</param>
    /// <param name="price">Цена блюда</param>
    /// <param name="description">Описание блюда</param>
    /// <returns>Идентификатор нового блюда</returns>
    Task<Guid> AddDishAsync(string name, DishType type, double price, string description);
    
    /// <summary>
    /// Обновить блюдо
    /// </summary>
    /// <param name="updateDish">Данные блюда для обновления</param>
    /// <returns></returns>
    Task UpdateDishAsync(Dish updateDish);
    
    /// <summary>
    /// Удалить блюдо
    /// </summary>
    /// <param name="dishId">Идентификатор блюда</param>
    /// <returns></returns>
    Task RemoveDishAsync(Guid dishId);
}
