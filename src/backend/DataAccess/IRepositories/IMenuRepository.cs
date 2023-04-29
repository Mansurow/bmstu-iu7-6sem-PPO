using Anticafe.DataAccess.DBModels;

namespace Anticafe.DataAccess.IRepositories;

public interface IMenuRepository
{
    Task<List<MenuDbModel>> GetAllDishesAsync();
    Task<MenuDbModel> GetDishByIdAsync(int dishId);
    Task InsertDishAsync(MenuDbModel menu);
    Task UpdateDishAsync(MenuDbModel menu);
    Task DeleteDishAsync(int dishId);
}
