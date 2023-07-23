using Anticafe.DataAccess.DBModels;
using Anticafe.DataAccess.IRepositories;
using Anticafe.DataAccess.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Anticafe.DataAccess.Repositories;

public class MenuRepository : BaseRepository, IMenuRepository
{
    private readonly AppDbContext _context;

    public MenuRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<MenuDbModel>> GetAllDishesAsync()
    {
        return await _context.Menu.ToListAsync();
    }

    public async Task<MenuDbModel> GetDishByIdAsync(int dishId)
    {
        var dish = await _context.Menu.FirstOrDefaultAsync(m => m.Id == dishId);
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
            _context.Menu.Add(menu);
            await _context.SaveChangesAsync();
        } catch
        {
            throw new DishCreateException($"Dish not create");
        }
    }

    public async Task UpdateDishAsync(MenuDbModel menu) 
    {
        try 
        {
            _context.Menu.Update(menu);
            await _context.SaveChangesAsync();
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
            _context.Menu.Remove(menu);
        }
        catch
        {
            throw new DishDeleteException($"Dish not delete");
        }
    }
}
