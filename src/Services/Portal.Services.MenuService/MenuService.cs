using Portal.Database.Repositories.Interfaces;
using Portal.Common.Models;
using Portal.Services.MenuService.Exceptions;
using Portal.Common.Models.Enums;

namespace Portal.Services.MenuService;

public class MenuService: IMenuService
{
    private readonly IMenuRepository _menuRepository;

    public MenuService(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public Task<List<Dish>> GetAllDishesAsync() 
    {
        return _menuRepository.GetAllDishesAsync();
    }

    public async Task<Dish> GetDishByIdAsync(Guid dishId) 
    {
        var dish = await _menuRepository.GetDishByIdAsync(dishId);
        if (dish is null)
        {
            throw new MenuNotFoundException($"Dish not found by id: {dishId}");
        }

        return dish;
    }

    public async Task UpdateDishAsync(Dish updateDish) 
    {
        var dish = await _menuRepository.GetDishByIdAsync(updateDish.Id);
        if (dish is null) 
        {
            throw new MenuNotFoundException($"Dish not found by id: {updateDish.Id}");
        }
        await _menuRepository.UpdateDishAsync(updateDish);
    }

    public async Task AddDishAsync(string name, DishType type, double price, string description) 
    {
        var dish = new Dish(Guid.NewGuid(), name, type, price, description);
        await _menuRepository.InsertDishAsync(dish);
    }

    public async Task DeleteDishAsync(Guid dishId) 
    {
        var dish = await _menuRepository.GetDishByIdAsync(dishId);
        if (dish is null) 
        {
            throw new MenuNotFoundException($"Dish not found by id: {dishId}");
        }

        await _menuRepository.DeleteDishAsync(dishId);
    }
}
