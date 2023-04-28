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
            return (await _menuRepository.GetAllDishes()).Select(m => MenuConverter.ConvertDbModelToAppModel(m)).ToList();
        }

        public async Task<Menu> GetDishByIdAsync(int dishId) 
        {
            var dish = await _menuRepository.GetDishById(dishId);
            if (dish is null)
            {
                throw new Exception();
            }

            return MenuConverter.ConvertDbModelToAppModel(dish);
        }

        public async Task AddDishAsync(string name, DishType type, double price, string description) 
        {
            var menu = await GetAllDishesAsync();
            var dish = new Menu(menu.Count() + 1, name, type, price, description);
            await _menuRepository.InsertDish(MenuConverter.ConvertAppModelToDbModel(dish));
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
