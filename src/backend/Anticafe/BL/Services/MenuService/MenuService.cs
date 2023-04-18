using Anticafe.BL.Enums;
using Anticafe.BL.IRepositories;
using Anticafe.BL.Models;

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
            return await _menuRepository.GetAllDishes();
        }

        public async Task<Menu> GetDishByIdAsync(int dishId) 
        {
            var dish = await _menuRepository.GetDishById(dishId);
            if (dish is null)
            {
                throw new Exception();
            }

            return dish;
        }

        public async Task AddDishAsync(string name, DishType type, double price, string description) 
        {
            var menu = await GetAllDishesAsync();
            var dish = new Menu(menu.Count() + 1, name, type, price, description);
            await _menuRepository.InsertDish(dish);
        }

        public async Task DeleteDishAsync(int dishId) 
        {
            var dish = await _menuRepository.GetDishById(dishId);
            if (dish is null) 
            {
                throw new Exception();
            }

            await _menuRepository.DeleteDish(dishId);
        }
    }
}
