using Portal.Common.Models;
using Portal.Database.Repositories.Interfaces;
using Portal.Services.InventoryServices.Exceptions;

namespace Portal.Services.InventoryServices;

public class InventoryService: IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository;

    public InventoryService(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));
    }

    public Task<List<Inventory>> GetAllInventoriesAsync()
    {
        return _inventoryRepository.GetAllInventoryAsync();
    }

    public async Task<Inventory> GetInventoryByIdAsync(Guid inventoryId)
    {
        var inventory = await _inventoryRepository.GetInventoryByIdAsync(inventoryId);
        if (inventory is null)
        {
            throw new InventoryNotFoundException($"Inventory with id: {inventoryId} not found");
        }

        return inventory;
    }

    public async Task<Guid> AddInventoryAsync(string name, DateOnly yearOfProduction, string description)
    {
        var inventory = new Inventory(Guid.NewGuid(), name, description, yearOfProduction);

        await _inventoryRepository.InsertInventoryAsync(inventory);
        
        return inventory.Id;
    }

    public async Task UpdateInventoryAsync(Inventory updateInventory)
    {
        var inventory = await _inventoryRepository.GetInventoryByIdAsync(updateInventory.Id);
        if (inventory is null)
        {
            throw new InventoryNotFoundException($"Inventory with id: {updateInventory.Id} not found");
        }

        await _inventoryRepository.UpdateInventoryAsync(updateInventory);
    }

    public async Task RemoveInventoryAsync(Guid inventoryId)
    {
        var inventory = await _inventoryRepository.GetInventoryByIdAsync(inventoryId);
        if (inventory is null)
        {
            throw new InventoryNotFoundException($"Inventory with id: {inventoryId} not found");
        }

        await _inventoryRepository.DeleteInventoryAsync(inventoryId);
    }
}