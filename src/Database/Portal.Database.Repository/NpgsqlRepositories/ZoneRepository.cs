using Microsoft.EntityFrameworkCore;
using Portal.Common.Converter;
using Portal.Common.Models;
using Portal.Database.Context;
using Portal.Database.Models;
using Portal.Database.Repositories.Interfaces;

namespace Portal.Database.Repositories.NpgsqlRepositories;

public class ZoneRepository: BaseRepository, IZoneRepository
{
    private readonly PortalDbContext _context;

    public ZoneRepository(PortalDbContext context)
    {
        _context = context;
    }

    public Task<List<Zone>> GetAllZonesAsync()
    {
        return _context.Zones
            .Select(z => ZoneConverter.ConvertDbModelToAppModel(z))
            .ToListAsync();
    }

    public async Task<Zone> GetZoneByIdAsync(Guid zoneId)
    {
        var zone = await _context.Zones.FirstAsync(z => z.Id == zoneId);

        return ZoneConverter.ConvertDbModelToAppModel(zone);
    }

    public async Task<Zone> GetZoneByNameAsync(string name)
    {
        var zone = await _context.Zones
            .FirstAsync(z => String.Equals(z.Name, name, StringComparison.CurrentCultureIgnoreCase));

        return ZoneConverter.ConvertDbModelToAppModel(zone);
    }

    public async Task InsertZoneAsync(Zone zone)
    {
        var zoneDb = ZoneConverter.ConvertAppModelToDbModel(zone);

        await _context.Zones.AddAsync(zoneDb);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateZoneAsync(Zone zone)
    {
        var zoneDb = ZoneConverter.ConvertAppModelToDbModel(zone);

        _context.Zones.Update(zoneDb);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateZoneRatingAsync(Guid zoneId, double rating)
    {
        var zone = await _context.Zones.FirstAsync(z => z.Id == zoneId);
        zone.Rating = rating;
        
        _context.Zones.Update(zone);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteZoneAsync(Guid zoneId)
    {
        var zone = await _context.Zones.FirstAsync(z => z.Id == zoneId);

        _context.Remove(zone);
        await _context.SaveChangesAsync();
    }
}