using Portal.Common.Models;

namespace Portal.Services.ZoneService;

/// <summary>
/// Интерфейс сервиса зон
/// </summary>
public interface IZoneService
{
    /// <summary>
    /// Получить все зоны
    /// </summary>
    /// <returns>Список всех зон</returns>
    Task<List<Zone>> GetAllZonesAsync();
    
    /// <summary>
    /// Получить зоны
    /// </summary>
    /// <param name="zoneId">Идентификатор зоны</param>
    /// <returns>Данные зоны</returns>
    Task<Zone> GetZoneByIdAsync(Guid zoneId);
    
    /// <summary>
    /// Добавить зону
    /// </summary>
    /// <param name="name">Название зоны</param>
    /// <param name="address">Адрес зоны</param>
    /// <param name="size">Размер зоныв кв. метрах</param>
    /// <param name="limit">Максимальное количество людей</param>
    /// <param name="price">Цена в рублях за час</param>
    /// <returns>Идентификатор новой зоны</returns>
    Task<Guid> AddZoneAsync(string name, string address, double size, int limit, double price);
    
    /// <summary>
    /// Обновить зону
    /// </summary>
    /// <param name="zone">Данные новой зоны</param>
    /// <returns></returns>
    Task UpdateZoneAsync(Zone zone);
    
    /// <summary>
    /// Удалить зоны
    /// </summary>
    /// <param name="zoneId">Идентификатор зоны</param>
    /// <returns></returns>
    Task RemoveZoneAsync(Guid zoneId);
    
    /// <summary>
    /// Добавить инвентарь
    /// </summary>
    /// <param name="zoneId">Идентификатор зоны</param>
    /// <param name="inventory">Данные инвентаря</param>
    /// <returns></returns>
    Task AddInventoryAsync(Guid zoneId, Inventory inventory);

    /// <summary>
    /// Добавить пакет
    /// </summary>
    /// <param name="zoneId">Идентификатор зоны</param>
    /// <param name="packageId">Идентификатор пакета</param>
    /// <returns></returns>
    Task AddPackageAsync(Guid zoneId, Guid packageId);
}
