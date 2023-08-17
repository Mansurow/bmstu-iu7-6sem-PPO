using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portal.Common.Models;
using Portal.Common.Models.Enums;
using Portal.Database.Repositories.Interfaces;
using Portal.Services.MenuService.Exceptions;
using Portal.Services.PackageService.Exceptions;

namespace Portal.Services.PackageService;

/// <summary>
/// Сервис управления пакетами
/// </summary>
public class PackageService: IPackageService
{
    private readonly IPackageRepository _packageRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly ILogger<PackageService> _logger;

    public PackageService(IPackageRepository packageRepository, ILogger<PackageService> logger, IMenuRepository menuRepository)
    {
        _packageRepository = packageRepository ?? throw new ArgumentNullException(nameof(packageRepository));
        _menuRepository = menuRepository ?? throw new ArgumentNullException(nameof(menuRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Package with id: {PackageId} not found", packageId);
            throw new PackageNotFoundException($"Package with id: {packageId} not found");
        }
    }

    public async Task<Guid> AddPackageAsync(string name, PackageType type, double price,
        int rentalTime, string description, List<Guid> dishes)
    {
        try
        {
            var package = new Package(Guid.NewGuid(), name, type, price, rentalTime, description);

            foreach (var dishId in dishes)
            {
                try
                {
                    var dish = await _menuRepository.GetDishByIdAsync(dishId);
                    
                    // TODO: Проверка что блюдо уже добавлено в пакет
                    
                    package.Dishes.Add(dish);
                }
                catch (InvalidOperationException e)
                {
                    _logger.LogError(e, "Dish: {DishId} not found for adding in package: {PackageId}", dishId, package.Id);
                    throw new DishNotFoundException($"Dish: {dishId} not found for adding in package: {package.Id}");
                }
            }

            await _packageRepository.InsertPackageAsync(package);

            return package.Id;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error while creating package");
            throw new PackageCreateException("Package has not been created");
        }
        
    }

    public async Task UpdatePackageAsync(Package package)
    {
        try
        {
            await _packageRepository.UpdatePackageAsync(package);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Package with id: {PackageId} not found", package.Id);
            throw new PackageNotFoundException($"Package with id: {package.Id} not found");
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error while updating package: {PackageId}", package.Id);
            throw new PackageUpdateException($"Package with id: {package.Id} has not been updated");
        }
    }

    public async Task RemovePackageAsync(Guid packageId)
    {
        try
        {
            await _packageRepository.DeletePackageAsync(packageId);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Package with id: {PackageId} not found", packageId);
            throw new PackageNotFoundException($"Package with id: {packageId} not found");
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error while removing package: {PackageId}", packageId);
            throw new PackageRemoveException($"Package with id: {packageId} has not been removed");
        }
    }
}
