using Portal.Common.Models;
using Portal.Database.Repositories.Interfaces;
using Portal.Services.ZoneService.Exceptions;
using Portal.Sevices.ZoneService;

namespace Portal.Services.ZoneService;

public class ZoneService: IZoneService
{
    private readonly IZoneRepository _zoneRepository;
    private readonly IInventoryRepository _inventoryRepository;

    public ZoneService(IZoneRepository zoneRepository, IInventoryRepository inventoryRepository) 
    {
        _zoneRepository = zoneRepository;
        _inventoryRepository = inventoryRepository;
    }

    public Task<List<Zone>> GetAllZonesAsync()
    {
        return _zoneRepository.GetAllZonesAsync();
    }

    public async Task<Zone> GetZoneByIdAsync(Guid roomId)
    {
        var room = await _zoneRepository.GetZoneByIdAsync(roomId);
        if (room is null) 
        {
            throw new ZoneNotFoundException($"Room not found in anticafe by id: {roomId}");
        }

        return room;
    }

    public async Task AddZoneAsync(string name, string address, double size, int limit, double price)
    {
        var room = await _zoneRepository.GetZoneByNameAsync(name);
        if (room is not null)
        {
            throw new ZoneNameExistException($"This name \"{name}\" of room exists.");
        }

        await _zoneRepository.InsertZoneAsync(new Zone(Guid.NewGuid(), name, address, size, limit, price, 0.0));
    }

    public async Task UpdateZoneAsync(Zone zone)
    {
        if (!_ZoneExists(zone.Id).Result)
        {
            throw new ZoneNotFoundException($"Room not found in anticafe by id: {zone.Id}");
        }

        var room = await _zoneRepository.GetZoneByNameAsync(zone.Name);
        if (room is not null)
        {
            throw new ZoneNameExistException($"This name \"{zone.Name}\" of room exists.");
        }

        await _zoneRepository.UpdateZoneAsync(zone);
    }

    // TODO: Доработать идею лобавления инвентаря)))
    public async Task AddInventoryForZoneAsync(Guid zoneId, Inventory inventory)
    {
        var zone = await GetZoneByIdAsync(zoneId);

        zone.AddInventory(inventory);

        await UpdateZoneAsync(zone);
    }

    // при удалении зоны инвентарь удалять надо???
    public async Task DeleteZoneAsync(Guid zoneId)
    {
        if (!_ZoneExists(zoneId).Result)
        {
            throw new ZoneNotFoundException($"Room not found in anticafe by id: {zoneId}");
        }

        await _zoneRepository.DeleteZoneAsync(zoneId);
    }

    private async Task<Boolean> _ZoneExists(Guid zoneId) 
    {
        var zone = await _zoneRepository.GetZoneByIdAsync(zoneId);

        if (zone is not null)
            return true;

        return false;
    }
}
