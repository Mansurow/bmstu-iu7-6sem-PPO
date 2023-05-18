using Anticafe.DataAccess.DBModels;
using Anticafe.DataAccess.IRepositories;
using Anticafe.DataAccess.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Anticafe.DataAccess.Repositories;

public class MenuRepository : BaseRepository, IMenuRepository
{
    private readonly AppDbContext _context;

    public MenuRepository(IDbContextFactory contextFactory)
    {
        _context = contextFactory.getDbContext();
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
            // _context.Menu.Update(menu);
            var dish = await _context.Menu.FirstOrDefaultAsync(d => d.Id == menu.Id);
            if (dish is not null) 
            {
                dish.Name = menu.Name;
                dish.Type = menu.Type;
                dish.Price = menu.Price;
                dish.Description = menu.Description;
            }

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
