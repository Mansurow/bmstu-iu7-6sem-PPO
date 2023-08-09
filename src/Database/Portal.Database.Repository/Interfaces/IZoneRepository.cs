using Portal.Common.Models;

namespace Portal.Database.Repositories.Interfaces;

/// <summary>
/// Интерфейс репозитория зоны
/// </summary>
public interface IZoneRepository
{
    /// <summary>
    /// Получить все зоны
    /// </summary>
    /// <returns>Список всех зон</returns>
    Task<List<Zone>> GetAllZonesAsync();
    
    /// <summary>
    /// Получить зону
    /// </summary>
    /// <param name="zoneId">Идентификатор зоны</param>
    /// <returns>Зона</returns>
    Task<Zone> GetZoneByIdAsync(Guid zoneId);
    
    /// <summary>
    /// Получить зоны по названию
    /// </summary>
    /// <param name="name">Название зоны</param>
    /// <returns>Зона</returns>
    Task<Zone> GetZoneByNameAsync(string name);
    
    /// <summary>
    /// Добавить зону
    /// </summary>
    /// <param name="zone">Данные новой зоны</param>
    /// <returns></returns>
    Task InsertZoneAsync(Zone zone);
    
    /// <summary>
    /// Обновить зоны
    /// </summary>
    /// <param name="zone">Данные зоны для обновления</param>
    /// <returns></returns>
    Task UpdateZoneAsync(Zone zone);
    
    /// <summary>
    /// Обновить рейтинг зоны
    /// </summary>
    /// <param name="zoneId">Идентификатор зоны</param>
    /// <param name="rating">Новый рейнтиг зоны</param>
    /// <returns></returns>
    Task UpdateZoneRatingAsync(Guid zoneId, double rating);
    
    /// <summary>
    /// Удалить зону
    /// </summary>
    /// <param name="zoneId">Идентифкатор зоны</param>
    /// <returns></returns>
    Task DeleteZoneAsync(Guid zoneId);
}
