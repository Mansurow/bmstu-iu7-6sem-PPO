using Portal.Common.Models;

namespace Portal.Database.Core.Repositories;

/// <summary>
/// Интерфейс репозитория пакета
/// </summary>
public interface IPackageRepository
{
    /// <summary>
    /// Получить все пакеты
    /// </summary>
    /// <returns>Список пакетов</returns>
    Task<List<Package>> GetAllPackagesAsync();
    
    /// <summary>
    /// Получить пакет
    /// </summary>
    /// <param name="packageId">Идентификатор пакета</param>
    /// <returns>Пакета</returns>
    Task<Package> GetPackageByIdAsync(Guid packageId);
    
    /// <summary>
    /// Добавить пакет
    /// </summary>
    /// <param name="package">Данные нового пакета</param>
    /// <returns></returns>
    Task InsertPackageAsync(Package package);
    
    /// <summary>
    /// Обновить пакет
    /// </summary>
    /// <param name="package">Данные пакета дял обновления</param>
    /// <returns></returns>
    Task UpdatePackageAsync(Package package);
    
    /// <summary>
    /// Удалить пакет
    /// </summary>
    /// <param name="packageId">Идентификатор пакета</param>
    /// <returns></returns>
    Task DeletePackageAsync(Guid packageId);
}
