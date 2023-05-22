using Anticafe.DataAccess.DBModels;
using Anticafe.DataAccess.Exceptions;
using Anticafe.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Anticafe.MongoDB.Repositories;

public class MenuRepository: BaseRepository, IMenuRepository
{
    private readonly IMongoCollection<MenuDbModel> _menuCollection;

    public MenuRepository(IDbCollectionFactory collections) 
    {
        _menuCollection = collections.getMenuCollection();
    }

    public async Task<List<MenuDbModel>> GetAllDishesAsync()
    {
        return await _menuCollection.Find(_ => true).ToListAsync();
    }

    public async Task<MenuDbModel> GetDishByIdAsync(int dishId)
    {
        var dish = await _menuCollection.Find(m => m.Id == dishId).FirstOrDefaultAsync();
        if (dish is null)
        {
            throw new DishNotFoundException($"Dish not found with id: {dishId}");
        }

        return dish;
    }

    public async Task InsertDishAsync(MenuDbModel menu)
    {
        try
        {
            await _menuCollection.InsertOneAsync(menu);
        }
        catch
        {
            throw new DishCreateException($"Dish not create");
        }
    }

    public async Task UpdateDishAsync(MenuDbModel menu)
    {
        try
        {
            var filter = Builders<MenuDbModel>.Filter.Eq(u => u.Id, menu.Id);
            var update = Builders<MenuDbModel>.Update.Set(u => u.Name, menu.Name)
                                                     .Set(u => u.Type, menu.Type)
                                                     .Set(u => u.Price, menu.Price)
                                                     .Set(u => u.Description, menu.Description);

            await _menuCollection.UpdateOneAsync(filter, update);
        }
        catch
        {
            throw new DishUpdateException($"Dish not update");
        }
    }

    public async Task DeleteDishAsync(int dishId)
    {
        try
        {
            var menu = await GetDishByIdAsync(dishId);
            var filter = Builders<MenuDbModel>.Filter.Lt(u => u.Id, dishId);

            await _menuCollection.DeleteOneAsync(filter);
        }
        catch
        {
            throw new DishDeleteException($"Dish not delete");
        }
    }
}
