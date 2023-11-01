using Microsoft.EntityFrameworkCore;
using Portal.Common.Converter;
using Portal.Common.Core;
using Portal.Database.Context;
using Portal.Database.Core.Repositories;
using Portal.Database.Models;

namespace Portal.Database.Repositories.NpgsqlRepositories;

public class PackageRepository: BaseRepository, IPackageRepository
{
    private readonly PortalDbContext _context;

    public PackageRepository(PortalDbContext context)
    {
        _context = context;
    }

    public Task<List<Package>> GetAllPackagesAsync()
    {
        return _context.Packages
            .Include(p => p.Zones)
            .Include(p => p.Dishes)
            .AsNoTracking()
            .Select(p => PackageConverter.ConvertDBToCoreModel(p))
            .ToListAsync();
    }

    public async Task<Package> GetPackageByIdAsync(Guid packageId)
    {
        var package = await _context.Packages
            .Include(p => p.Zones)
            .Include(p => p.Dishes)
            .AsNoTracking()
            .FirstAsync(p => p.Id == packageId);

        return PackageConverter.ConvertDBToCoreModel(package);
    }

    public async Task InsertPackageAsync(Package package)
    {
        var packageDb = new PackageDbModel(
                package.Id,
                package.Name,
                package.Type,
                package.Price,
                package.RentalTime,
                package.Description
            );
        
        await _context.Packages.AddAsync(packageDb);
        
        /*var dishes = package.Dishes.Select(MenuConverter.ConvertAppModelToDbModel).ToList();
        packageDb.Dishes = dishes;*/

        foreach (var dish in package.Dishes)
        {
            packageDb.Dishes.Add(_context.Menu.First(d => d.Id == dish.Id));
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePackageAsync(Package package)
    {
        var packageDb = await _context.Packages
            .Include(p => p.Zones)    
            .Include(p => p.Dishes)
            .FirstAsync(p => p.Id == package.Id);

        packageDb.Name = package.Name;
        packageDb.Description = package.Description;
        packageDb.Type = package.Type;
        packageDb.RentalTime = package.RentalTime;
        packageDb.Price = package.Price;
        packageDb.Dishes = package.Dishes.Select(MenuConverter.ConvertCoreToDBModel).ToList();
        // packageDb.Zones = package.Zones.Select(ZoneConverter.ConvertAppModelToDbModel).ToList();
        
        await _context.SaveChangesAsync();
    }

    public async Task DeletePackageAsync(Guid packageId)
    {
        var package = await _context.Packages.FirstAsync(p => p.Id == packageId);

        _context.Packages.Remove(package);
        await _context.SaveChangesAsync();
    }
}