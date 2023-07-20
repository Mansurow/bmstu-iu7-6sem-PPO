using Portal.Common.Models;
using Portal.Common.Models.Enums;

namespace Portal.Services.PackageService;

public interface IPackageService
{
    Task<List<Package>> GetPackagesAsync();
    Task<Package> GetPackageById(Guid packageId);
    Task AddPackageAsync(string name, PackageType Type, double price, 
        int rentalTime, string description);
    Task UpdatePackageAsync(Package package);
    Task RemovePackageAsync(Guid packageId);
}