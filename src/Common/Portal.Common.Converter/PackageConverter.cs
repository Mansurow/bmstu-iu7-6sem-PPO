using Portal.Common.Models;
using Portal.Database.Models;

namespace Portal.Common.Converter;

/// <summary>
/// Конвертатор модели Package
/// </summary>
public static class PackageConverter
{
    /// <summary>
    /// Преобразовать из модели базы данных в модель бизнес логики приложения
    /// </summary>
    /// <param name="package">Модель базы данных</param>
    /// <returns>Модель бизнес логики</returns>
    public static Package ConvertDbModelToAppModel(PackageDbModel package)
    {
        return new Package(id: package.Id,
            name: package.Name,
            type: package.Type,
            price: package.Price,
            rentalTime: package.RentalTime,
            description: package.Description,
            dishes: package.Dishes.Select(MenuConverter.ConvertDbModelToAppModel).ToList(),
            zones: package.Zones.Select(ZoneConverter.ConvertDbModelToAppModelNoInclude).ToList());
    }
    
    /// <summary>
    /// Преобразовать из модели базы данных в модель бизнес логики приложения
    /// </summary>
    /// <param name="package">Модель базы данных</param>
    /// <returns>Модель бизнес логики</returns>
    public static Package ConvertDbModelToAppModelNoInclude(PackageDbModel package)
    {
        return new Package(id: package.Id,
            name: package.Name,
            type: package.Type,
            price: package.Price,
            rentalTime: package.RentalTime,
            description: package.Description,
            dishes: package.Dishes.Select(MenuConverter.ConvertDbModelToAppModel).ToList(),
            zones: new List<Zone>());
    }
    
    /// <summary>
    /// Преобразовать из модели бизнес логики в модели базы данных приложения
    /// </summary>
    /// <param name="package">Модель бизнес логики</param>
    /// <returns>Модель базы данных </returns>
    public static PackageDbModel ConvertAppModelToDbModel(Package package)
    {
        return new PackageDbModel(id: package.Id,
            name: package.Name,
            type: package.Type,
            price: package.Price,
            rentalTime: package.RentalTime,
            description: package.Description,
            dishes: package.Dishes.Select(MenuConverter.ConvertAppModelToDbModel).ToList(),
            zones: package.Zones.Select(ZoneConverter.ConvertAppModelToDbModel).ToList());
    }
}
