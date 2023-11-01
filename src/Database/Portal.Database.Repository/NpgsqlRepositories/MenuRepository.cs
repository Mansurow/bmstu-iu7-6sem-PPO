using Microsoft.EntityFrameworkCore;
using Portal.Common.Converter;
using Portal.Common.Core;
using Portal.Database.Context;
using Portal.Database.Core.Repositories;

namespace Portal.Database.Repositories.NpgsqlRepositories;

public class MenuRepository: BaseRepository, IMenuRepository
{
    private readonly PortalDbContext _context;

    public MenuRepository(PortalDbContext context)
    {
        _context = context;
    }
    
    public Task<List<Dish>> GetAllDishesAsync()
    {
        return _context.Menu
            .Select(d => MenuConverter.ConvertDBToCoreModel(d))
            .ToListAsync();
    }
    
    public async Task<Dish> GetDishByIdAsync(Guid dishId)
    {
        var dish = await _context.Menu.FirstAsync(d => d.Id == dishId);

        return MenuConverter.ConvertDBToCoreModel(dish);
    }

    public async Task InsertDishAsync(Dish dish)
    {
        var dishDb = MenuConverter.ConvertCoreToDBModel(dish);
        
        await _context.Menu.AddAsync(dishDb);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateDishAsync(Dish dish)
    {
        var updatedDish = await _context.Menu.FirstAsync(d => d.Id == dish.Id);
        updatedDish.Name = dish.Name;
        updatedDish.Type = dish.Type;
        updatedDish.Price = dish.Price;
        updatedDish.Description = dish.Description;
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteDishAsync(Guid dishId)
    {
        var dishDb = await _context.Menu.FirstAsync(d => d.Id == dishId);

        _context.Menu.Remove(dishDb);
        await _context.SaveChangesAsync();
    }
}