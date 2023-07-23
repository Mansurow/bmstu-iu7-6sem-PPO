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
        var package = await _packageRepository.GetPackageByIdAsync(packageId);
        if (package is null)
        {
            throw new PackageNotFoundException($"Package with id: {packageId} not found");
        } 

        return package;
    }

    public async Task<Guid> AddPackageAsync(string name, PackageType type, double price,
        int rentalTime, string description)
    {
        var package = new Package(Guid.NewGuid(), name, type, price, rentalTime, description);

        await _packageRepository.InsertPackageAsync(package);

        return package.Id;
    }

    public async Task UpdatePackageAsync(Package package)
    {
        var getPackage = await _packageRepository.GetPackageByIdAsync(package.Id);
        if (getPackage is null)
        {
            throw new PackageNotFoundException($"Package with id: {package.Id} not found");
        }
        
        await _packageRepository.UpdatePackageAsync(package);
    }

    public async Task RemovePackageAsync(Guid packageId)
    {
        var package = await _packageRepository.GetPackageByIdAsync(packageId);
        if(package is null)
        {
            throw new PackageNotFoundException($"Package with id: {packageId} not found");
        }

        await _packageRepository.DeletePackageAsync(packageId);
    }
}
