using Anticafe.BL.Exceptions;
using Anticafe.BL.Models;
using Anticafe.Common.Enums;
using Anticafe.DataAccess.Converter;
using Anticafe.DataAccess.IRepositories;

namespace Anticafe.BL.Services.MenuService
{
    public class MenuService: IMenuService
    {
        private readonly IMenuRepository _menuRepository;

        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<List<Menu>> GetAllDishesAsync() 
        {
            return (await _menuRepository.GetAllDishesAsync()).Select(m => MenuConverter.ConvertDbModelToAppModel(m)).ToList();
        }

        public async Task<Menu> GetDishByIdAsync(int dishId) 
        {
            var dish = await _menuRepository.GetDishByIdAsync(dishId);
            if (dish is null)
            {
                throw new MenuNotFoundException($"Dish not found by id: {dishId}");
            }

            return MenuConverter.ConvertDbModelToAppModel(dish);
        }

        public async Task UpdateDishAsync(Menu updateDish) 
        {
            var dish = await _menuRepository.GetDishByIdAsync(updateDish.Id);
            if (dish is null) 
            {
                throw new MenuNotFoundException($"Dish not found by id: {updateDish.Id}");
            }
            await _menuRepository.UpdateDishAsync(MenuConverter.ConvertAppModelToDbModel(updateDish));
        }

        public async Task AddDishAsync(string name, DishType type, double price, string description) 
        {
            var menu = await GetAllDishesAsync();
            var dish = new Menu(menu.Count() + 1, name, type, price, description);
            await _menuRepository.InsertDishAsync(MenuConverter.ConvertAppModelToDbModel(dish));
        }

        public async Task DeleteDishAsync(int dishId) 
        {
            var dish = await _menuRepository.GetDishByIdAsync(dishId);
            if (dish is null) 
            {
                throw new MenuNotFoundException($"Dish not found by id: {dishId}");
            }

            await _menuRepository.DeleteDishAsync(dishId);
        }
    }
}
