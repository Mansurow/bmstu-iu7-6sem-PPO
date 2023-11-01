using HotChocolate.Authorization;
using Portal.Common.Dto;
using Portal.Common.Enums;
using Portal.Services.InventoryServices;

namespace Portal.Graphql.Mutations;

[ExtendObjectType("Mutation")]
public class InventoryMutation
{
    /// <summary>
    /// Списать инвентаря.
    /// </summary>
    /// <param name="inventoryService"></param>
    /// <param name="inventoryId">Идентификатор инветаря.</param>
    [GraphQLName("WriteOffInventory")]
    [Authorize(Roles = new []{nameof(Role.Administrator)})]
    public async Task<IdResponse> WriteOffInventory([Service(ServiceKind.Resolver)] IInventoryService inventoryService,
        Guid inventoryId)
    {
        var inventory = await inventoryService.GetInventoryByIdAsync(inventoryId);
        inventory.IsWrittenOff = true;
        await inventoryService.UpdateInventoryAsync(inventory);

        return new IdResponse(inventoryId);
    }
}