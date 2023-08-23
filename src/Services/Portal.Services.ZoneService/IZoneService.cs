using Portal.Common.Models;
using Portal.Common.Models.Dto;
using Portal.Services.ZoneService.Exceptions;

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
    /// <exception cref="ZoneNotFoundException">Зона не найдена</exception>
    Task<Zone> GetZoneByIdAsync(Guid zoneId);

    /// <summary>
    /// Добавить зону
    /// </summary>
    /// <param name="name">Название зоны</param>
    /// <param name="address">Адрес зоны</param>
    /// <param name="size">Размер зоны в кв. метрах</param>
    /// <param name="limit">Максимальное количество людей</param>
    /// <param name="price">Цена за аренду в рублях </param>
    /// <returns>Идентификатор новой зоны</returns>
    /// <exception cref="ZoneNameExistException">Названии зоны уже существует</exception>
    /// <exception cref="ZoneCreateException">При создании зоны</exception>
    Task<Guid> AddZoneAsync(string name, string address, double size, int limit, double price);
    
    /// <summary>
    /// Добавить зону
    /// </summary>
    /// <param name="name">Название зоны</param>
    /// <param name="address">Адрес зоны</param>
    /// <param name="size">Размер зоныв кв. метрах</param>
    /// <param name="limit">Максимальное количество людей</param>
    /// <param name="price">Цена в рублях за час</param>
    /// <param name="inventories">Список инвентаря</param>
    /// <param name="packages">Список пакетов</param>
    /// <returns>Идентификатор новой зоны</returns>
    /// <exception cref="ZoneNameExistException">Названии зоны уже существует</exception>
    /// <exception cref="ZoneCreateException">При создании зоны</exception>
    Task<Guid> AddZoneAsync(string name, string address, double size, int limit, double price, List<Inventory> inventories, List<Package> packages);
    
    /// <summary>
    /// Обновить зону
    /// </summary>
    /// <param name="zone">Данные новой зоны</param>
    /// <exception cref="ZoneNotFoundException">Зона не найдена</exception>
    /// <exception cref="ZoneNameExistException">Названии зоны уже существует</exception>
    /// <exception cref="ZoneUpdateException">При обновлении зоны</exception>
    Task UpdateZoneAsync(Zone zone);
    
    /// <summary>
    /// Удалить зоны
    /// </summary>
    /// <param name="zoneId">Идентификатор зоны</param>
    /// <exception cref="ZoneNotFoundException">Зона не найдена</exception>
    /// <exception cref="ZoneRemoveException">При удалении зоны</exception>
    Task RemoveZoneAsync(Guid zoneId);
    
    /// <summary>
    /// Добавить инвентарь
    /// </summary>
    /// <param name="zoneId">Идентификатор зоны</param>
    /// <param name="inventories">Данные инвентаря</param>
    /// <exception cref="ZoneNotFoundException">Зона не найдена</exception>
    /// <exception cref="ZoneUpdateException">При обновлении зоны</exception>
    Task AddInventoryAsync(Guid zoneId, List<CreateInventoryDto> inventories);

    /// <summary>
    /// Добавить пакет
    /// </summary>
    /// <param name="zoneId">Идентификатор зоны</param>
    /// <param name="packageId">Идентификаторы пакета</param>
    /// <exception cref="ZonePackageExistsException">Пакет уже добавлен в зону</exception>
    /// <exception cref="ZoneNotFoundException">Зона не найдена</exception>
    /// <exception cref="ZoneUpdateException">При обновлении зоны</exception>
    Task AddPackageAsync(Guid zoneId, List<Guid> packageId);    
    
    /// <summary>
    /// Добавить пакет
    /// </summary>
    /// <param name="zoneId">Идентификатор зоны</param>
    /// <param name="packageId">Идентификатор пакета</param>
    /// <exception cref="ZonePackageExistsException">Пакет уже добавлен в зону</exception>
    /// <exception cref="ZoneNotFoundException">Зона не найдена</exception>
    /// <exception cref="ZoneUpdateException">При обновлении зоны</exception>
    Task AddPackageAsync(Guid zoneId, Guid packageId);
}
