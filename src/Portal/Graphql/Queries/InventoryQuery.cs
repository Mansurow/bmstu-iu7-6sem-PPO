using HotChocolate.Authorization;
using Portal.Common.Converter;
using Portal.Common.Dto.Inventory;
using Portal.Common.Enums;
using Portal.Services.InventoryServices;

namespace Portal.Graphql.Queries;

[ExtendObjectType("Query")]
public class InventoryQuery
{
    /// <summary>
    /// Получить весь инвентаря.
    /// </summary>
    /// <param name="inventoryService"></param>
    /// <returns>Список инвентаря.</returns>
    [GraphQLName("GetInventories")]
    [Authorize(Roles = new []{nameof(Role.Administrator)})]
    public async Task<IEnumerable<Inventory>> GetInventories([Service(ServiceKind.Resolver)] InventoryService inventoryService)
    {
        var inventories = await inventoryService.GetAllInventoriesAsync();

        return inventories.Select(InventoryConverter.ConvertCoreToDtoModel);
    }
}