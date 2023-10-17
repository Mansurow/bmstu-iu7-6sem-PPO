using HotChocolate.Authorization;
using Portal.Common.Converter;
using Portal.Common.Dto;
using Portal.Common.Dto.Zone;
using Portal.Common.Enums;
using Portal.Services.ZoneService;

namespace Portal.Graphql.Mutations;

[ExtendObjectType("Mutation")]
public class ZoneMutation
{
    /// <summary>
    /// Добавить зону.
    /// </summary>
    /// <param name="zoneService"></param>
    /// <param name="createZone">Данные для добавления зоны.</param>
    /// <returns>Идентификатор зоны.</returns>
    [GraphQLName("AddZone")]
    [Authorize(Roles = new []{nameof(Role.Administrator)})]
    public async Task<IdResponse> AddZone([Service(ServiceKind.Resolver)] IZoneService zoneService, 
        CreateZone createZone)
    {
        var zoneId = await zoneService.AddZoneAsync(createZone.Name, 
            createZone.Address, 
            createZone.Size, 
            createZone.Limit);

        await zoneService.AddInventoryAsync(zoneId, createZone.Inventories);
        await zoneService.AddPackageAsync(zoneId, createZone.Packages);
            
        return new IdResponse(zoneId);
    }
    
    /// <summary>
    /// Обновить зону.
    /// </summary>
    /// <param name="zoneService"></param>
    /// <param name="updateZone">Данные для обновления зоны.</param>
    [GraphQLName("UpdateZone")]
    [Authorize(Roles = new []{nameof(Role.Administrator)})]
    public async Task<IdResponse> UpdateZone([Service(ServiceKind.Resolver)] IZoneService zoneService, 
        UpdateZone updateZone)
    {
        var zone = ZoneConverter.ConvertDtoToCoreModel(updateZone);
            
        await zoneService.UpdateZoneAsync(zone);

        return new IdResponse(updateZone.Id);
    }
    
    /// <summary>
    /// Удалить зону.
    /// </summary>
    /// <param name="zoneService"></param>
    /// <param name="zoneId">Идентификатор зоны.</param>
    [GraphQLName("DeleteZone")]
    [Authorize(Roles = new []{nameof(Role.Administrator)})]
    public async Task<IdResponse> DeleteZone([Service(ServiceKind.Resolver)] IZoneService zoneService,
        Guid zoneId)
    {
        await zoneService.RemoveZoneAsync(zoneId);
        
        return new IdResponse(zoneId);
    }
}