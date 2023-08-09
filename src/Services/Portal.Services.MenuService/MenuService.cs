using Portal.Database.Repositories.Interfaces;
using Portal.Common.Models;
using Portal.Services.MenuService.Exceptions;
using Portal.Common.Models.Enums;


namespace Portal.Services.MenuService;

/// <summary>
/// Сервис меню блюд
/// </summary>
public class MenuService: IMenuService
{
    private readonly IMenuRepository _menuRepository;

    public MenuService(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository ?? throw new ArgumentNullException(nameof(menuRepository));
    }

    public Task<List<Dish>> GetAllDishesAsync() 
    {
        return _menuRepository.GetAllDishesAsync();
    }

    public async Task<Dish> GetDishByIdAsync(Guid dishId) 
    {
        try
        {
            var dish = await _menuRepository.GetDishByIdAsync(dishId);
            
            return dish;
        }
        catch (Exception e)
        {
            throw new DishNotFoundException($"Dish not found by id: {dishId}");
        }
    }

    public async Task UpdateDishAsync(Dish updateDish)
    {

        try
        {
            var dish = await _menuRepository.GetDishByIdAsync(updateDish.Id);
            await _menuRepository.UpdateDishAsync(updateDish);
        }
        catch (InvalidOperationException)
        {
            throw new DishNotFoundException($"Dish not found by id: {updateDish.Id}");
        }
        catch (Exception)
        {
            throw new DishUpdateException($"Dish was not updated: {updateDish.Id}");
        }
    }

    public async Task<Guid> AddDishAsync(string name, DishType type, double price, string description) 
    {
        try
        {
            var dish = new Dish(Guid.NewGuid(), name, type, price, description);

            await _menuRepository.InsertDishAsync(dish);

            return dish.Id;
        }
        catch (Exception)
        {
            throw new DishCreateException($"Dish was not created");
        }
    }

    public async Task RemoveDishAsync(Guid dishId)
    {
        await GetDishByIdAsync(dishId);
        try
        {
            await _menuRepository.DeleteDishAsync(dishId);
        }
        catch (Exception)
        {
            throw new DishDeleteException("The dish has not been removed");
        }
    }
}
