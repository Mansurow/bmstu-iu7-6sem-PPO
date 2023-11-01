using Portal.Common.Converter;
using Portal.Common.Dto.Zone;
using Portal.Services.ZoneService;

namespace Portal.Graphql.Queries;

[ExtendObjectType("Query")]
public class ZoneQuery
{
    /// <summary>
    /// Получить все зоны.
    /// </summary>
    /// <param name="zoneService"></param>
    /// <returns>Список зон.</returns>
    [GraphQLName("GetZones")]
    public async Task<IEnumerable<Zone>> GetZones([Service(ServiceKind.Resolver)] IZoneService zoneService)
    {
        var zones = await zoneService.GetAllZonesAsync();

        return zones.Select(ZoneConverter.ConvertCoreToDtoModel);
    }
    
    /// <summary>
    /// Получить зон.
    /// </summary>
    /// <param name="zoneService"></param>
    /// <param name="zoneId">Идентификато зоны.</param>
    /// <returns>Данные зоны.</returns>
    [GraphQLName("GetZone")]
    public async Task<Zone> GetZone([Service(ServiceKind.Resolver)]  IZoneService zoneService,
        Guid zoneId)
    {
        var package = await zoneService.GetZoneByIdAsync(zoneId);

        return ZoneConverter.ConvertCoreToDtoModel(package);
    }
}