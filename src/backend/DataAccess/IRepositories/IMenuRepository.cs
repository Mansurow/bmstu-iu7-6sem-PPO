using Anticafe.DataAccess.DBModels;

namespace Anticafe.DataAccess.IRepositories;

public interface IMenuRepository
{
    Task<List<MenuDbModel>> GetAllDishes();
    Task<MenuDbModel> GetDishById(int dishId);
    Task InsertDish(MenuDbModel menu);
    Task UpdateDish(MenuDbModel menu);
    Task DeleteDish(int dishId);
}
