using Portal.Common.Core;
using Portal.Services.InventoryServices.Exceptions;

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
    /// <exception cref="InventoryNotFoundException">Инвентарь не найден</exception>
    Task<Inventory> GetInventoryByIdAsync(Guid inventoryId);

    /// <summary>
    /// Добавить инвентарь
    /// </summary>
    /// <param name="zoneId">Идентификатор зоны</param>
    /// <param name="name">Название</param>
    /// <param name="yearOfProduction">Год выпуска или производства</param>
    /// <param name="description">Описание инветаря</param>
    /// <returns>Идентификатор нового инветаря</returns>
    /// <exception cref="InventoryCreateException">При создании инвентаря</exception>
    Task<Guid> AddInventoryAsync(Guid zoneId, string name, DateOnly yearOfProduction, string description);

    /// <summary>
    /// Обновить инвентарь
    /// </summary>
    /// <param name="inventory">Данные инвентаря на обновление</param>
    /// <exception cref="InventoryNotFoundException">Инвентарь не найден</exception>
    /// <exception cref="InventoryUpdateException">При обновлении инвентаря</exception>
    Task UpdateInventoryAsync(Inventory inventory);
    
    /// <summary>
    /// Удалить инвентарь
    /// </summary>
    /// <param name="inventoryId">Идентификатор инвентаря</param>
    /// <exception cref="InventoryNotFoundException">Инвентарь не найден</exception>
    /// <exception cref="InventoryRemoveException">При удалении инвентаря</exception>
    Task RemoveInventoryAsync(Guid inventoryId);
}