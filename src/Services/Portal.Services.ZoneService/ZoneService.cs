using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<ZoneService> _logger;

    public ZoneService(IZoneRepository zoneRepository, 
        IInventoryRepository inventoryRepository,
        IPackageRepository packageRepository,
        ILogger<ZoneService> logger) 
    {
        _zoneRepository = zoneRepository ?? throw new ArgumentNullException(nameof(zoneRepository));
        _inventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));
        _packageRepository = packageRepository ?? throw new ArgumentNullException(nameof(packageRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Zone with id: {ZoneId} not found", zoneId);
            throw new ZoneNotFoundException($"Zone with id: {zoneId} not found");;
        }
    }

    public async Task<Guid> AddZoneAsync(string name, string address, double size, int limit, double price)
    {
        try
        {
            try
            {
                var zone = await _zoneRepository.GetZoneByNameAsync(name);
                
                _logger.LogError("This name \"{ZoneName}\" of zone exists.", zone.Name);
                throw new ZoneNameExistException($"This name \"{zone.Name}\" of zone exists.");
            }
            catch (InvalidOperationException)
            {
                _logger.LogInformation("This name \"{ZoneName}\" of zone not found.", name);
                var newZone = new Zone(Guid.NewGuid(), name, address, size, limit, price, 0.0);
                await _zoneRepository.InsertZoneAsync(newZone);

                return newZone.Id;
            }
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error while creating zone");
            throw new ZoneCreateException("Zone has not been created");
        }
        
    }

    public async Task UpdateZoneAsync(Zone updateZone)
    {
        try
        {
            await _zoneRepository.UpdateZoneAsync(updateZone);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Zone with id: {ZoneId} not found", updateZone.Id);
            throw new ZoneNotFoundException($"Zone with id: {updateZone.Id} not found");
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error while updating zone: {ZoneId}", updateZone.Id);
            throw new ZoneUpdateException($"Zone with id: {updateZone.Id} was not updated");
        }
    }
    
    public async Task AddInventoryAsync(Guid zoneId, Inventory inventory)
    {
        try
        {
            var zone = await _zoneRepository.GetZoneByIdAsync(zoneId);
            var zoneInventory = zone.Inventories.FirstOrDefault(i => i.Id == inventory.Id);
            if (zoneInventory is not null)
            {
                _logger.LogError("Inventory: {InventoryId} already has been included in zone in zone: {ZoneId}", inventory.Id, zone.Id);
                throw new ZoneExistsInventoryException($"Inventory with id: {inventory.Id} already has been included in zone with id: {zoneId}");
            }
            
            zone.AddInventory(inventory);
            await _zoneRepository.UpdateZoneAsync(zone);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Zone with id: {ZoneId} not found", zoneId);
            throw new ZoneNotFoundException($"Zone with id: {zoneId} not found");
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error while adding inventory in zone: {ZoneId}", zoneId);
            throw new ZoneUpdateException($"Zone with id: {zoneId} has not been updated");
        }
    }
    
    public async Task AddPackageAsync(Guid zoneId, Guid packageId)
    {
        try
        {
            var zone = await _zoneRepository.GetZoneByIdAsync(zoneId);
            try
            {
                var package = await _packageRepository.GetPackageByIdAsync(packageId);
                
                var zonePackage = zone.Packages.FirstOrDefault(p => p.Id == packageId);
                if (zonePackage is not null)
                {
                    _logger.LogError("Package: {PackageId} already has been included in zone in zone: {ZoneId}", packageId, zone.Id);
                    throw new ZonePackageExistsException($"Package with id: {packageId} for zone with id: {zoneId} already exists");
                }
                
                zone.AddPackage(package);
                await _zoneRepository.UpdateZoneAsync(zone);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e, "Package with id: {PackageId} not found", packageId);
                throw new PackageNotFoundException($"Package with id: {packageId} not found");
            }
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Zone with id: {ZoneId} not found", zoneId);
            throw new ZoneNotFoundException($"Zone with id: {zoneId} not found");
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error while adding package for zone: {ZoneId}", zoneId);
            throw new ZoneUpdateException($"Zone with id: {zoneId} has not been updated");
        }
        
    }
    
    public async Task RemoveZoneAsync(Guid zoneId)
    {
        try
        {
            await _zoneRepository.DeleteZoneAsync(zoneId);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Zone with id: {ZoneId} not found", zoneId);
            throw new ZoneNotFoundException($"Zone with id: {zoneId} not found");
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error while removing zone: {ZoneId}", zoneId);
            throw new ZoneDeleteException($"Zone was not updated: {zoneId}");
        }
    }
}
