using Portal.Common.Converter;
using Portal.Common.Dto.Dish;
using Portal.Services.MenuService;

namespace Portal.Graphql.Queries;

[ExtendObjectType("Query")]
public class MenuQuery
{
    /// <summary>
    /// Получить меню блюд.
    /// </summary>
    /// <param name="menuService"></param>
    /// <returns>Список блюд меню.</returns>
    [GraphQLName("GetMenu")]
    public async Task<IEnumerable<Dish>> GetMenu([Service(ServiceKind.Resolver)] IMenuService menuService)
    {
        var menu = await menuService.GetAllDishesAsync();

        return menu.Select(MenuConverter.ConvertCoreToDtoModel);
    }
    
    /// <summary>
    /// Получить блюдо меню.
    /// </summary>
    /// <param name="menuService"></param>
    /// <param name="dishId">Идентификатор блюда.</param>
    /// <returns>Блюдо меню.</returns>
    [GraphQLName("GetDish")]
    public async Task<Dish> GetDish([Service(ServiceKind.Resolver)] IMenuService menuService,
        Guid dishId)
    {
        var dish = await menuService.GetDishByIdAsync(dishId);

        return MenuConverter.ConvertCoreToDtoModel(dish);
    }
}