using Microsoft.EntityFrameworkCore;
using Portal.Common.Converter;
using Portal.Common.Models;
using Portal.Database.Context;
using Portal.Database.Core.Repositories;

namespace Portal.Database.Repositories.NpgsqlRepositories;

public class InventoryRepository: BaseRepository, IInventoryRepository
{
    private readonly PortalDbContext _context;

    public InventoryRepository(PortalDbContext context)
    {
        _context = context;
    }
    
    public Task<List<Inventory>> GetAllInventoryAsync()
    {
        return _context.Inventories
            .Select(i => InventoryConverter.ConvertDbModelToAppModel(i))
            .ToListAsync();
    }

    public async Task<Inventory> GetInventoryByIdAsync(Guid inventoryId)
    {
        var inventory = await _context.Inventories.FirstAsync(i => i.Id == inventoryId);

        return InventoryConverter.ConvertDbModelToAppModel(inventory);
    }

    public async Task InsertInventoryAsync(Inventory inventory)
    {
        var inventoryDb = InventoryConverter.ConvertAppModelToDbModel(inventory);
        
        await _context.Inventories.AddAsync(inventoryDb);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateInventoryAsync(Inventory inventory)
    {
        var inventoryDb = await _context.Inventories.FirstAsync(inv => inv.Id == inventory.Id);

        inventoryDb.Name = inventory.Name;
        inventoryDb.Description = inventory.Description;
        inventoryDb.YearOfProduction = inventory.YearOfProduction;
        inventoryDb.IsWrittenOff = inventory.IsWrittenOff;
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteInventoryAsync(Guid inventoryId)
    {
        var inventory = await _context.Inventories.FirstAsync(i => i.Id == inventoryId);

        _context.Inventories.Remove(inventory);
        await _context.SaveChangesAsync();
    }
}