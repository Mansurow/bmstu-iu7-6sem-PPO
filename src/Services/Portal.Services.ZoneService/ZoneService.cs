using Portal.Common.Models;
using Portal.Database.Repositories.Interfaces;
using Portal.Services.InventoryServices.Exceptions;
using Portal.Services.PackageService.Exceptions;
using Portal.Services.ZoneService.Exceptions;

namespace Portal.Services.ZoneService;

/// <summary>
/// Сервис зон
/// </summary>
public class ZoneService: IZoneService
{
    private readonly IZoneRepository _zoneRepository;
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IPackageRepository _packageRepository;

    public ZoneService(IZoneRepository zoneRepository, 
        IInventoryRepository inventoryRepository,
        IPackageRepository packageRepository) 
    {
        _zoneRepository = zoneRepository;
        _inventoryRepository = inventoryRepository;
        _packageRepository = packageRepository;
    }

    public Task<List<Zone>> GetAllZonesAsync()
    {
        return _zoneRepository.GetAllZonesAsync();
    }

    public async Task<Zone> GetZoneByIdAsync(Guid zoneId)
    {
        try
        {
            var zone = await _zoneRepository.GetZoneByIdAsync(zoneId);
            
            return zone;
        }
        catch (Exception)
        {
            throw new ZoneNotFoundException($"Zone not found with id: {zoneId}");
        }
    }

    public async Task<Guid> AddZoneAsync(string name, string address, double size, int limit, double price)
    {
        var zone = await _zoneRepository.GetZoneByNameAsync(name);
        if (zone is not null)
        {
            throw new ZoneNameExistException($"This name \"{name}\" of zone exists.");
        }

        var newZone = new Zone(Guid.NewGuid(), name, address, size, limit, price, 0.0);
        await _zoneRepository.InsertZoneAsync(newZone);

        return newZone.Id;
    }

    public async Task UpdateZoneAsync(Zone updateZone)
    {
        var zone = await GetZoneByIdAsync(updateZone.Id);
        if (zone.Name == updateZone.Name)
        {
            throw new ZoneNameExistException($"This name \"{zone.Name}\" of zone exists.");
        }

        await _zoneRepository.UpdateZoneAsync(updateZone);
    }
    
    public async Task AddInventoryAsync(Guid zoneId, Inventory inventory)
    {
        var zone = await GetZoneByIdAsync(zoneId);

        try
        {
            zone.AddInventory(inventory);
            await _zoneRepository.UpdateZoneAsync(zone);
        }
        catch (Exception)
        {
            throw new ZoneUpdateException($"Zone was not updated: {zoneId}");
        }
        
    }
    
    public async Task AddPackageAsync(Guid zoneId, Guid packageId)
    {
        var zone = await GetZoneByIdAsync(zoneId);
        var package = await _packageRepository.GetPackageByIdAsync(packageId);

        if (package is null)
        {
            throw new PackageNotFoundException($"Package with id: {packageId} not found");
        }

        var zonePackage = zone.Packages.FirstOrDefault(p => p.Id == packageId);
        if (zonePackage is not null)
        {
            throw new ZonePackageExistsException($"Package with id: {packageId} for zone with id: {zoneId} already exists");
        }

        try
        {
            zone.AddPackage(package);
            await _zoneRepository.UpdateZoneAsync(zone);
        }
        catch (Exception e)
        {
            throw new ZoneUpdateException($"Zone was not updated: {zoneId}");
        }
        
    }
    
    public async Task RemoveZoneAsync(Guid zoneId)
    {
        var zone = await GetZoneByIdAsync(zoneId);
        try
        {
            await _zoneRepository.DeleteZoneAsync(zoneId);
        }
        catch (Exception)
        {
            throw new ZoneDeleteException($"Zone was not updated: {zoneId}");
        }
        
    }
}
