using Portal.Common.Models;

namespace Portal.Database.Repositories.Interfaces;

public interface IMenuRepository
{
    Task<List<Dish>> GetAllDishesAsync();
    Task<Dish> GetDishByIdAsync(Guid dishId);
    Task InsertDishAsync(Dish menu);
    Task UpdateDishAsync(Dish menu);
    Task DeleteDishAsync(Guid dishId);
}
