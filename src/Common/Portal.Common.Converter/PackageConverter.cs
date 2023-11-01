using PackageCore = Portal.Common.Core.Package;
using PackageDB = Portal.Database.Models.PackageDbModel;
using PackageDto = Portal.Common.Dto.Package.Package;
using ZoneCore = Portal.Common.Core.Zone;
using ZoneDB = Portal.Database.Models.ZoneDbModel;
using ZoneDto = Portal.Common.Dto.Zone.Zone;

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
    public static PackageCore ConvertDBToCoreModel(PackageDB package)
    {
        return new PackageCore(id: package.Id,
            name: package.Name,
            type: package.Type,
            price: package.Price,
            rentalTime: package.RentalTime,
            description: package.Description,
            dishes: package.Dishes.Select(MenuConverter.ConvertDBToCoreModel).ToList(),
            zones: package.Zones.Select(ZoneConverter.ConvertDBToCoreModelNoInclude).ToList());
    }
    
    /// <summary>
    /// Преобразовать из модели базы данных в модель бизнес логики приложения
    /// </summary>
    /// <param name="package">Модель базы данных</param>
    /// <returns>Модель бизнес логики</returns>
    public static PackageCore ConvertDBToCoreModelNoInclude(PackageDB package)
    {
        return new PackageCore(id: package.Id,
            name: package.Name,
            type: package.Type,
            price: package.Price,
            rentalTime: package.RentalTime,
            description: package.Description,
            dishes: package.Dishes.Select(MenuConverter.ConvertDBToCoreModel).ToList(),
            zones: new List<ZoneCore>());
    }
    
    /// <summary>
    /// Преобразовать из модели бизнес логики в модели базы данных приложения
    /// </summary>
    /// <param name="package">Модель бизнес логики</param>
    /// <returns>Модель базы данных </returns>
    public static PackageDB ConvertCoreToDBModel(PackageCore package)
    {
        return new PackageDB(id: package.Id,
            name: package.Name,
            type: package.Type,
            price: package.Price,
            rentalTime: package.RentalTime,
            description: package.Description,
            dishes: package.Dishes.Select(MenuConverter.ConvertCoreToDBModel).ToList(),
            zones: package.Zones.Select(ZoneConverter.ConvertCoreToDBModel).ToList());
    }
    
    /// <summary>
    /// Преобразовать из модели бизнес логики в модели DTO
    /// </summary>
    /// <param name="package">Модель бизнес логики</param>
    /// <returns>Модель DTO</returns>
    public static PackageDto ConvertCoreToDtoModel(PackageCore package)
    {
        return new PackageDto(id: package.Id,
            name: package.Name,
            type: package.Type,
            price: package.Price,
            rentalTime: package.RentalTime,
            description: package.Description,
            dishes: package.Dishes.Select(MenuConverter.ConvertCoreToDtoModel).ToList(),
            zones: package.Zones.Select(ZoneConverter.ConvertCoreToDtoModel).ToList());
    }
    
    /// <summary>
    /// Преобразовать из модели DTO в модели бизнес логики
    /// </summary>
    /// <param name="package">Модель DTO</param>
    /// <returns>Модель бизнес логики</returns>
    public static PackageCore ConvertDtoToCoreModel(PackageDto package)
    {
        return new PackageCore(id: package.Id,
            name: package.Name,
            type: package.Type,
            price: package.Price,
            rentalTime: package.RentalTime,
            description: package.Description,
            dishes: package.Dishes.Select(MenuConverter.ConvertDtoToCoreModel).ToList(),
            zones: package.Zones.Select(ZoneConverter.ConvertDtoToCoreModel).ToList());
    }
}
