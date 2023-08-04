using Microsoft.EntityFrameworkCore;
using Portal.Common.Converter;
using Portal.Common.Models;
using Portal.Database.Context;
using Portal.Database.Models;
using Portal.Database.Repositories.Interfaces;

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
            .Select(p => PackageConverter.ConvertDbModelToAppModel(p)!)
            .ToListAsync();
    }

    public async Task<Package> GetPackageByIdAsync(Guid packageId)
    {
        var package = await _context.Packages.FirstAsync(p => p.Id == packageId);

        return PackageConverter.ConvertDbModelToAppModel(package);
    }

    public async Task InsertPackageAsync(Package package)
    {
        var packageDb = PackageConverter.ConvertAppModelToDbModel(package);
        
        await _context.Packages.AddAsync(packageDb);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePackageAsync(Package package)
    {
        var packageDb = PackageConverter.ConvertAppModelToDbModel(package);
        
        _context.Packages.Update(packageDb);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePackageAsync(Guid packageId)
    {
        var package = await _context.Packages.FirstAsync(p => p.Id == packageId);

        _context.Packages.Remove(package);
        await _context.SaveChangesAsync();
    }
}