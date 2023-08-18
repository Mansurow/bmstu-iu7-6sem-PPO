using Portal.Common.Models;
using Portal.Common.Models.Enums;
using Portal.Services.MenuService.Exceptions;
using Portal.Services.PackageService.Exceptions;

namespace Portal.Services.PackageService;

/// <summary>
/// Интерфейс Сервиса управления пакетами для зон
/// </summary>
public interface IPackageService
{
    /// <summary>
    /// Достать все пакеты
    /// </summary>
    /// <returns>Список всех пакетов</returns>
    Task<List<Package>> GetPackagesAsync();
    
    /// <summary>
    /// Достать пакет 
    /// </summary>
    /// <param name="packageId">Идентификатор пакета</param>
    /// <returns>Пакет</returns>
    /// <exception cref="PackageNotFoundException">Пакет не найден</exception>
    Task<Package> GetPackageById(Guid packageId);

    /// <summary>
    /// Добавить пакет зоны
    /// </summary>
    /// <param name="name">Название пакета</param>
    /// <param name="type">Тип пакета</param>
    /// <param name="price">Цена пакета (общая)</param>
    /// <param name="rentalTime">Общее время</param>
    /// <param name="description">Описание пакета</param>
    /// <param name="dishes">Блюда для добавления</param>
    /// <returns>Идентификатор созданного пакета</returns>
    /// <exception cref="DishNotFoundException">Блюдо не найден</exception>
    /// <exception cref="PackageCreateException">При создании пакета</exception>
    Task<Guid> AddPackageAsync(string name, PackageType type, double price, 
        int rentalTime, string description, List<Guid> dishes);
    
    /// <summary>
    /// Обновить пакет 
    /// </summary>
    /// <param name="package">Данные пакета</param>
    /// <exception cref="PackageNotFoundException">Пакет не найден</exception>
    /// <exception cref="PackageUpdateException">При обновлении пакета</exception>
    Task UpdatePackageAsync(Package package);
    
    /// <summary>
    /// Удалить пакет
    /// </summary>
    /// <param name="packageId">Идентификатор пакета</param>
    /// <exception cref="PackageNotFoundException">Пакет не найден</exception>
    /// <exception cref="PackageRemoveException">При удалении пакета</exception>
    Task RemovePackageAsync(Guid packageId);
}