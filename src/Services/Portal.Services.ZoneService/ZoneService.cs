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
        var zone = await _zoneRepository.GetZoneByIdAsync(zoneId);
        if (zone is null) 
        {
            throw new ZoneNotFoundException($"Zone with id: {zoneId} not found");
        }

        return zone;
    }

    public async Task<Guid> AddZoneAsync(string name, string address, double size, int limit, double price)
    {
        var zone = await _zoneRepository.GetZoneByNameAsync(name);
        if (zone is not null)
        {
            throw new ZoneNameExistException($"This name \"{name}\" of room exists.");
        }

        var newZone = new Zone(Guid.NewGuid(), name, address, size, limit, price, 0.0);
        await _zoneRepository.InsertZoneAsync(newZone);

        return newZone.Id;
    }

    public async Task UpdateZoneAsync(Zone updateZone)
    {
        if (!await _ZoneExists(updateZone.Id))
        {
            throw new ZoneNotFoundException($"Zone with id: {updateZone.Id} not found");
        }

        var zone = await _zoneRepository.GetZoneByNameAsync(updateZone.Name);
        if (zone is not null)
        {
            throw new ZoneNameExistException($"This name \"{zone.Name}\" of zone exists.");
        }

        await _zoneRepository.UpdateZoneAsync(updateZone);
    }
    
    public async Task AddInventoryForZoneAsync(Guid zoneId, Inventory inventory)
    {
        var zone = await GetZoneByIdAsync(zoneId);

        zone.AddInventory(inventory);

        await UpdateZoneAsync(zone);
    }

    public async Task AddInventoryForZoneAsync(Guid zoneId, Guid inventoryId)
    {
        var zone = await GetZoneByIdAsync(zoneId);
        var inventory = await _inventoryRepository.GetInventoryByIdAsync(inventoryId);

        if (inventory is null)
        {
            throw new InventoryNotFoundException($"Inventory with id: {inventoryId} not found");
        }
        
        zone.AddInventory(inventory);

        await UpdateZoneAsync(zone);
    }
    
    public async Task AddPackageForZoneAsync(Guid zoneId, Guid packageId)
    {
        var zone = await GetZoneByIdAsync(zoneId);
        var package = await _packageRepository.GetPackageByIdAsync(packageId);

        if (package is null)
        {
            throw new PackageNotFoundException($"Package with id: {packageId} not found");
        }
        
        zone.AddPackage(package);

        await UpdateZoneAsync(zone);
    }
    
    public async Task RemoveZoneAsync(Guid zoneId)
    {
        if (!_ZoneExists(zoneId).Result)
        {
            throw new ZoneNotFoundException($"Zone with id: {zoneId} not found");
        }

        await _zoneRepository.DeleteZoneAsync(zoneId);
    }

    private async Task<Boolean> _ZoneExists(Guid zoneId) 
    {
        var zone = await _zoneRepository.GetZoneByIdAsync(zoneId);

        if (zone is null)
            return false;
        
        return true;
    }
}
