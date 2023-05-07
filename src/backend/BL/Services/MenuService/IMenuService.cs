using Anticafe.BL.Models;
using Anticafe.Common.Enums;

namespace Anticafe.BL.Services.MenuService
{
    public interface IMenuService
    {
        Task<List<Menu>> GetAllDishesAsync();
        Task<Menu> GetDishByIdAsync(int dishId);
        Task AddDishAsync(string name, DishType type, double price, string description);
        Task UpdateDishAsync(Menu updateDish);
        Task DeleteDishAsync(int dishId);
    }
}
