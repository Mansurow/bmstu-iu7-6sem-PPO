using Anticafe.BL.Enums;
using Anticafe.BL.Models;

namespace Anticafe.BL.Services.MenuService
{
    public interface IMenuService
    {
        Task<List<Menu>> GetAllDishesAsync();
        Task<Menu> GetDishByIdAsync(int dishId);
        Task AddDishAsync(string name, DishType type, double price, string description);
        Task DeleteDishAsync(int dishId);
    }
}
