using Portal.Common.Models;

namespace Portal.Services.InventoryServices;

/// <summary>
/// Интерфейс сервиса инвентаря
/// </summary>
public interface IInventoryService
{
    /// <summary>
    /// Получить весь инвентарь
    /// </summary>
    /// <returns>Список всего инвентаря</returns>
    Task<List<Inventory>> GetAllInventoriesAsync();

    /// <summary>
    /// Получить инвентарь
    /// </summary>
    /// <param name="inventoryId">Идентификатор инвентаря</param>
    /// <returns>Данные инвентаря</returns>
    Task<Inventory> GetInventoryByIdAsync(Guid inventoryId);

    /// <summary>
    /// Добавить инвентарь
    /// </summary>
    /// <param name="zoneId">Идентификатор зоны</param>
    /// <param name="name">Название</param>
    /// <param name="yearOfProduction">Год выпуска или производства</param>
    /// <param name="description">Описание инветаря</param>
    /// <returns>Идентификатор нового инветаря</returns>
    Task<Guid> AddInventoryAsync(Guid zoneId, string name, DateOnly yearOfProduction, string description);

    /// <summary>
    /// Обновить инвентарь
    /// </summary>
    /// <param name="inventory">Данные инвентаря на обновление</param>
    /// <returns></returns>
    Task UpdateInventoryAsync(Inventory inventory);
    
    /// <summary>
    /// Удалить инвентарь
    /// </summary>
    /// <param name="inventoryId">Идентификатор инвентаря</param>
    /// <returns></returns>
    Task RemoveInventoryAsync(Guid inventoryId);
}