using Microsoft.EntityFrameworkCore;
using Portal.Common.Models;
using Portal.Common.Models.Enums;
using Portal.Database.Repositories.Interfaces;
using Portal.Services.PackageService.Exceptions;

namespace Portal.Services.PackageService;

/// <summary>
/// Сервис управления пакетами
/// </summary>
public class PackageService: IPackageService
{
    private readonly IPackageRepository _packageRepository;

    public PackageService(IPackageRepository packageRepository)
    {
        _packageRepository = packageRepository ?? throw new ArgumentNullException(nameof(packageRepository));
    }

    public Task<List<Package>> GetPackagesAsync()
    {
        return _packageRepository.GetAllPackagesAsync();
    }

    public async Task<Package> GetPackageById(Guid packageId)
    {
        try
        {
            var package = await _packageRepository.GetPackageByIdAsync(packageId);
            
            return package;
        }
        catch (InvalidOperationException)
        {
            throw new PackageNotFoundException($"Package with id: {packageId} not found");
        }
    }

    public async Task<Guid> AddPackageAsync(string name, PackageType type, double price,
        int rentalTime, string description)
    {
        try
        {
            var package = new Package(Guid.NewGuid(), name, type, price, rentalTime, description);

            await _packageRepository.InsertPackageAsync(package);

            return package.Id;
        }
        catch (DbUpdateException e)
        {
            throw new PackageCreateException("Package has not been created");
        }
        
    }

    public async Task UpdatePackageAsync(Package package)
    {
        try
        {
            await _packageRepository.UpdatePackageAsync(package);
        }
        catch (InvalidOperationException)
        {
            throw new PackageNotFoundException($"Package with id: {package.Id} not found");
        }
        catch (DbUpdateException)
        {
            throw new PackageUpdateException($"Package with id: {package.Id} has not been updated");
        }
    }

    public async Task RemovePackageAsync(Guid packageId)
    {
        try
        {
            await _packageRepository.DeletePackageAsync(packageId);
        }
        catch (InvalidOperationException)
        {
            throw new PackageNotFoundException($"Package with id: {packageId} not found");
        }
        catch (DbUpdateException)
        {
            throw new PackageRemoveException($"Package with id: {packageId} has not been removed");
        }
    }
}
