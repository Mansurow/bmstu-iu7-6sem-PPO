using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portal.Common.Core;
using Portal.Database.Core.Repositories;
using Portal.Services.InventoryServices.Exceptions;

namespace Portal.Services.InventoryServices;

public class InventoryService: IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly ILogger<InventoryService> _logger;

    public InventoryService(IInventoryRepository inventoryRepository,
        ILogger<InventoryService> logger)
    {
        _inventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Inventory with id: {InventoryId} not found", inventoryId);
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
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error while creating inventory");
            throw new InventoryCreateException("Inventory has not been created");
        }
    }

    public async Task UpdateInventoryAsync(Inventory updateInventory)
    {
        try
        {
            await _inventoryRepository.UpdateInventoryAsync(updateInventory);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Inventory with id: {InventoryId} not found", updateInventory.Id);
            throw new InventoryNotFoundException($"Inventory with id: {updateInventory.Id} not found");
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error while updating inventory: {InventoryId}", updateInventory.Id);
            throw new InventoryUpdateException($"Inventory with id: {updateInventory.Id} has not been updated");
        }
    }

    public async Task RemoveInventoryAsync(Guid inventoryId)
    {
        try
        {
            await _inventoryRepository.DeleteInventoryAsync(inventoryId);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Inventory with id: {InventoryId} not found", inventoryId);
            throw new InventoryNotFoundException($"Inventory with id: {inventoryId} not found");
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error while removing inventory: {InventoryId}", inventoryId);
            throw new InventoryRemoveException($"Inventory with id: {inventoryId} has not been removed");
        }
    }
}