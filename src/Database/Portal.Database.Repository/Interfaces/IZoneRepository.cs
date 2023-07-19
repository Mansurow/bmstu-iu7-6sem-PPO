using Portal.Common.Models;

namespace Portal.Database.Repositories.Interfaces;

public interface IZoneRepository
{
    Task<List<Zone>> GetAllZonesAsync();
    Task<Zone> GetZoneByIdAsync(Guid zoneId);
    Task<Zone> GetZoneByNameAsync(string name);
    Task InsertZoneAsync(Zone zone);
    Task UpdateZoneAsync(Zone zone);
    Task UpdateZoneRaitingAsync(Guid zoneId, double raiting);
    Task DeleteZoneAsync(Guid zoneId);
}
