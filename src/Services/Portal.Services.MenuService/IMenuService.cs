using Portal.Common.Models;
using Portal.Common.Models.Enums;

namespace Portal.Services.MenuService;

public interface IMenuService
{
    Task<List<Dish>> GetAllDishesAsync();
    Task<Dish> GetDishByIdAsync(Guid dishId);
    Task AddDishAsync(string name, DishType type, double price, string description);
    Task UpdateDishAsync(Dish updateDish);
    Task DeleteDishAsync(Guid dishId);
}
