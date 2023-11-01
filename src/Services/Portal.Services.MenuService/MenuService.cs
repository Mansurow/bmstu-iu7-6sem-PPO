using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portal.Common.Core;
using Portal.Services.MenuService.Exceptions;
using Portal.Common.Enums;
using Portal.Database.Core.Repositories;

namespace Portal.Services.MenuService;

/// <summary>
/// Сервис меню блюд
/// </summary>
public class MenuService: IMenuService
{
    private readonly IMenuRepository _menuRepository;
    private readonly ILogger<MenuService> _logger;

    /// <summary>
    /// Конструктор сервиса меню блюд
    /// </summary>
    /// <param name="menuRepository">Репозиторий меню блюд</param>
    /// <param name="logger">Инструмент логгирования</param>
    /// <exception cref="ArgumentNullException">Параметры неверно переданы</exception>
    public MenuService(IMenuRepository menuRepository, ILogger<MenuService> logger)
    {
        _menuRepository = menuRepository ?? throw new ArgumentNullException(nameof(menuRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Dish with id: {DishId} not found", dishId);
            throw new DishNotFoundException($"Dish with id: {dishId} not found");
        }
    }

    public async Task UpdateDishAsync(Dish updateDish)
    {

        try
        {
            await _menuRepository.GetDishByIdAsync(updateDish.Id);
            await _menuRepository.UpdateDishAsync(updateDish);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Dish with id: {DishId} not found", updateDish.Id);
            throw new DishNotFoundException($"Dish not found by id: {updateDish.Id}");
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error while updating dish: {DishId}", updateDish.Id);
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
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error while creating dish");
            throw new DishCreateException($"Dish was not created");
        }
    }

    public async Task RemoveDishAsync(Guid dishId)
    {
        try
        {
            await _menuRepository.DeleteDishAsync(dishId);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Dish with id: {DishId} not found", dishId);
            throw new DishNotFoundException($"Dish with id: {dishId} not found");
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error while removing dish: {DishId}", dishId);
            throw new DishRemoveException("The dish has not been removed");
        }
    }
}
