using Portal.Common.Core;

namespace Portal.Database.Core.Repositories;

/// <summary>
/// Идентификатор репозитория меню блюд
/// </summary>
public interface IMenuRepository
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
    /// <param name="dish">Данные нового блюда</param>
    /// <returns></returns>
    Task InsertDishAsync(Dish dish);
    
    /// <summary>
    /// Обновить блюдо
    /// </summary>
    /// <param name="dish">Данные блюда для обновление</param>
    /// <returns></returns>
    Task UpdateDishAsync(Dish dish);
    
    /// <summary>
    /// Удалить блюдо
    /// </summary>
    /// <param name="dishId">Идентификатор блюда</param>
    /// <returns></returns>
    Task DeleteDishAsync(Guid dishId);
}
