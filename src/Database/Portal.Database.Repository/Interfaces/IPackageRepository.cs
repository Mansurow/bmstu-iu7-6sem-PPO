using Portal.Common.Models;

namespace Portal.Database.Repositories.Interfaces;

public interface IPackageRepository
{
    Task<List<Package>> GetAllPackagesAsync();
    Task<Package> GetPackageByIdAsync(Guid packageId);
    Task InsertPackageAsync(Package package);
    Task UpdatePackageAsync(Package package);
    Task DeletePackageAsync(Guid packageId);
}
