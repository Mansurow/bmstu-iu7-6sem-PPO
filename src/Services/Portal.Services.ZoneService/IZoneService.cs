using Portal.Common.Models;

namespace Portal.Sevices.ZoneService;

public interface IZoneService
{
    Task<List<Zone>> GetAllZonesAsync();
    Task<Zone> GetZoneByIdAsync(Guid zoneId);
    Task AddZoneAsync(string name, string address, double size, int limit, double price);
    Task UpdateZoneAsync(Zone zone);
    Task DeleteZoneAsync(Guid zoneId);
    Task AddInventoryForZoneAsync(Guid zoneId, Inventory inventory);
}
