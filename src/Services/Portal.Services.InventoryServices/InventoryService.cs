using Microsoft.EntityFrameworkCore;
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
        try
        {
            var inventory = await _inventoryRepository.GetInventoryByIdAsync(inventoryId);

            return inventory;
        }
        catch (InvalidOperationException)
        {
            throw new InventoryNotFoundException($"Inventory with id: {inventoryId} not found");
        }
    }

    public async Task<Guid> AddInventoryAsync(Guid zoneId, string name, DateOnly yearOfProduction, string description)
    {
        try
        {
            var inventory = new Inventory(Guid.NewGuid(), zoneId, name, description, yearOfProduction);

            await _inventoryRepository.InsertInventoryAsync(inventory);

            return inventory.Id;
        }
        catch (DbUpdateException)
        {
            throw new InventoryCreateException("Inventory has not been created");
        }
    }

    public async Task UpdateInventoryAsync(Inventory updateInventory)
    {
        try
        {
            await _inventoryRepository.UpdateInventoryAsync(updateInventory);
        }
        catch (InvalidOperationException)
        {
            throw new InventoryNotFoundException($"Inventory with id: {updateInventory.Id} not found");
        }
        catch (DbUpdateException)
        {
            throw new InventoryUpdateException($"Inventory with id: {updateInventory.Id} has not been updated");
        }
    }

    public async Task RemoveInventoryAsync(Guid inventoryId)
    {
        try
        {
            await _inventoryRepository.DeleteInventoryAsync(inventoryId);
        }
        catch (InvalidOperationException)
        {
            throw new InventoryNotFoundException($"Inventory with id: {inventoryId} not found");
        }
        catch (DbUpdateException)
        {
            throw new InventoryRemoveException($"Inventory with id: {inventoryId} has not been removed");
        }
    }
}