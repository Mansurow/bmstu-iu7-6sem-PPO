using HotChocolate.Authorization;
using Portal.Common.Converter;
using Portal.Common.Dto;
using Portal.Common.Dto.Dish;
using Portal.Common.Enums;
using Portal.Services.MenuService;

namespace Portal.Graphql.Mutations;

[ExtendObjectType("Mutation")]
public class MenuMutation
{
    /// <summary>
    /// Добавить блюдо в меню.
    /// </summary>
    /// <param name="menuService"></param>
    /// <param name="createDish">Данные для добавления блюда.</param>
    /// <returns>Идентификатор блюда.</returns>
    [GraphQLName("AddDish")]
    [Authorize(Roles = new []{nameof(Role.Administrator)})]
    public async Task<IdResponse> AddDish([Service(ServiceKind.Resolver)] IMenuService menuService, CreateDish createDish)
    {
        var dishId = await menuService.AddDishAsync(createDish.Name, 
            createDish.Type, 
            createDish.Price, 
            createDish.Description);
            
        return new IdResponse(dishId);
    }
    
    /// <summary>
    /// Обновить блюдо в мнею.
    /// </summary>
    /// <param name="menuService"></param>
    /// <param name="updateDish">Данные для обновления блюда.</param>
    [GraphQLName("UpdateDish")]
    [Authorize(Roles = new []{nameof(Role.Administrator)})]
    public async Task<IdResponse> UpdateDish([Service(ServiceKind.Resolver)] IMenuService menuService, Dish updateDish)
    {
        var dish = MenuConverter.ConvertDtoToCoreModel(updateDish);
            
        await menuService.UpdateDishAsync(dish);

        return new IdResponse(updateDish.Id);
    }
    
    /// <summary>
    /// Удалить блюдо из меню.
    /// </summary>
    /// <param name="menuService"></param>
    /// <param name="dishId">Идентификатор блюда.</param>
    [GraphQLName("DeleteDish")]
    [Authorize(Roles = new []{nameof(Role.Administrator)})]
    public async Task<IdResponse> DeleteDish([Service(ServiceKind.Resolver)] IMenuService menuService,
        Guid dishId)
    {
        await menuService.RemoveDishAsync(dishId);

        return new IdResponse(dishId);
    }
}