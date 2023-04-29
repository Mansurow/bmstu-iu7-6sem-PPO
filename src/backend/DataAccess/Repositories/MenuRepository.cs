using Anticafe.DataAccess.DBModels;
using Anticafe.DataAccess.IRepositories;
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
            throw new Exception($"Dish not found with id: {dishId}");
        }

        return dish;
    }

    public async Task InsertDishAsync(MenuDbModel menu) 
    {
        try
        {
            _context.Menu.Add(menu);
            await _context.SaveChangesAsync();
        } catch (Exception ex) 
        {
            throw new Exception($"Dish not create");
        }
    }

    public async Task UpdateDishAsync(MenuDbModel menu) 
    {
        try 
        {
            _context.Menu.Update(menu);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Dish not update");
        }
    }

    public async Task DeleteDishAsync(int dishId) 
    {
        try
        {
            var menu = await _context.Menu.FirstOrDefaultAsync(m => m.Id == dishId);
            if (menu is null) 
            {
                throw new Exception();
            }

            _context.Menu.Remove(menu);
        }
        catch (Exception ex)
        {
            throw new Exception($"Dish not delete");
        }
    }
}
