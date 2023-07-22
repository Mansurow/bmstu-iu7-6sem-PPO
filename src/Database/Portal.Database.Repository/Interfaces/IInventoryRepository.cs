using Portal.Common.Models;

namespace Portal.Database.Repositories.Interfaces;

/// <summary>
/// Интерфейс репозитория инвентаря
/// </summary>
public interface IInventoryRepository
{
    /// <summary>
    /// Получить весь инвентарь
    /// </summary>
    /// <returns>Список всего инвентаря</returns>
    Task<List<Inventory>> GetAllInventoryAsync();
    
    /// <summary>
    /// Получить инвентарь
    /// </summary>
    /// <param name="inventoryId">Идентификатор инвентаря</param>
    /// <returns>Инвентарь</returns>
    Task<Inventory> GetInventoryByIdAsync(Guid inventoryId);
    
    /// <summary>
    /// Добавить инвентарь 
    /// </summary>
    /// <param name="inventory">Данные новго инвентаря</param>
    /// <returns></returns>
    Task InsertInventoryAsync(Inventory inventory);
    
    /// <summary>
    /// Обновить инвентарь
    /// </summary>
    /// <param name="inventory">Данные инвентаря для обновления</param>
    /// <returns></returns>
    Task UpdateInventoryAsync(Inventory inventory);
    
    /// <summary>
    /// Удалить инвентарь
    /// </summary>
    /// <param name="inventoryId">Идентификатор инвентаря</param>
    /// <returns></returns>
    Task DeleteInventoryAsync(Guid inventoryId);
}
