using Anticafe.BL.Models;

namespace Anticafe.BL.IRepositories
{
    public interface IMenuRepository
    {
        Task<List<Menu>> GetAllDishes();
        Task<Menu> GetDishById(int dishId);
        Task InsertDish(Menu menu);
        Task UpdateDish(Menu menu);
        Task DeleteDish(int dishId);
    }
}
