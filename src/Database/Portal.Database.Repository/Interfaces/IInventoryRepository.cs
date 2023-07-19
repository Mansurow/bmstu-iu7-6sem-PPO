using Portal.Common.Models;

namespace Portal.Database.Repositories.Interfaces;

public interface IInventoryRepository
{
    Task<List<Inventory>> GetAllInventoryAsync();
    Task<Inventory> GetInventoryByIdAsync(Guid inventoryId);
    Task InsertInventoryAsync(Inventory inventory);
    Task UpdateInventory(Inventory inventory);
    Task DeleteInventory(Guid inventoryId);
}
