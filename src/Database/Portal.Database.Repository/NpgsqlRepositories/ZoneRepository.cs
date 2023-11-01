using Microsoft.EntityFrameworkCore;
using Portal.Common.Converter;
using Portal.Common.Core;
using Portal.Database.Context;
using Portal.Database.Core.Repositories;
using Portal.Database.Models;

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
            .Include(z => z.Inventories)
            .Include(z => z.Packages)
            .AsNoTracking()
            .Select(z => ZoneConverter.ConvertDBToCoreModel(z))
            .ToListAsync();
    }

    public async Task<Zone> GetZoneByIdAsync(Guid zoneId)
    {
        var zone = await _context.Zones
            .Include(z => z.Inventories)
            .Include(z => z.Packages)
            .AsNoTracking()
            .FirstAsync(z => z.Id == zoneId);

        return ZoneConverter.ConvertDBToCoreModel(zone);
    }

    public async Task<Zone> GetZoneByNameAsync(string name)
    {
        var zone = await _context.Zones
            .FirstAsync(z => string.Equals(z.Name, name, StringComparison.CurrentCultureIgnoreCase));

        return ZoneConverter.ConvertDBToCoreModel(zone);
    }

    public async Task InsertZoneAsync(Zone zone)
    {
        var zoneDb = ZoneConverter.ConvertCoreToDBModel(zone);

        await _context.Zones.AddAsync(zoneDb);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateZoneAsync(Zone zone)
    {
        var zoneDb =  await _context.Zones
            .Include(z => z.Inventories)
            .Include(z => z.Packages)
            .FirstAsync(z => z.Id == zone.Id);

        zoneDb.Name = zone.Name;
        // zoneDb.Rating = zone.Rating;
        zoneDb.Limit = zone.Limit;
        zoneDb.Address = zone.Address;
        zoneDb.Size = zone.Size;
        zoneDb.Inventories = zone.Inventories.Select(InventoryConverter.ConvertCoreToDBModel).ToList();
        // zoneDb.Packages = zone.Packages.Select(PackageConverter.ConvertAppModelToDbModel).ToList();

        var packages = new List<PackageDbModel>();
        foreach (var package in zone.Packages)
        {
            var packageDb = await _context.Packages.FirstAsync(p => p.Id == package.Id);
            packages.Add(packageDb);
        }

        zoneDb.Packages = packages;
        
        // _context.Zones.Update(zoneDb);
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