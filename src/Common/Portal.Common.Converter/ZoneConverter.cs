using Portal.Common.Dto;
using Portal.Common.Dto.Zone;
using PackageCore = Portal.Common.Core.Package;
using PackageDB = Portal.Database.Models.PackageDbModel;
using PackageDto = Portal.Common.Dto.Package.Package;
using ZoneCore = Portal.Common.Core.Zone;
using ZoneDB = Portal.Database.Models.ZoneDbModel;
using ZoneDto = Portal.Common.Dto.Zone.Zone;

namespace Portal.Common.Converter
{
    /// <summary>
    /// Конвертатор модели Zone
    /// </summary>
    public static class ZoneConverter
    {
        /// <summary>
        /// Преобразовать из модели базы данных в модель бизнес логики приложения
        /// </summary>
        /// <param name="zone">Модель базы данных</param>
        /// <returns>Модель бизнес логики</returns>
        public static ZoneCore ConvertDBToCoreModel(ZoneDB zone) 
        {
            return new ZoneCore(id: zone.Id,
                name: zone.Name,
                address: zone.Address,
                size: zone.Size,
                limit: zone.Limit,
                rating: zone.Rating,
                inventories: zone.Inventories.Select(InventoryConverter.ConvertDBToCoreModel).ToList(),
                packages: zone.Packages.Select(PackageConverter.ConvertDBToCoreModelNoInclude).ToList());
        }
        
        /// <summary>
        /// Преобразовать из модели базы данных в модель бизнес логики приложения
        /// </summary>
        /// <param name="zone">Модель базы данных</param>
        /// <returns>Модель бизнес логики</returns>
        public static ZoneCore ConvertDBToCoreModelNoInclude(ZoneDB zone) 
        {
            return new ZoneCore(id: zone.Id,
                name: zone.Name,
                address: zone.Address,
                size: zone.Size,
                limit: zone.Limit,
                rating: zone.Rating,
                inventories: zone.Inventories.Select(InventoryConverter.ConvertDBToCoreModel).ToList(),
                packages: new List<PackageCore>());
        }
        
        public static ZoneDB ConvertCoreToDBModel(ZoneCore zone)
        {
            return new ZoneDB(id: zone.Id,
                name: zone.Name,
                address: zone.Address,
                size: zone.Size,
                limit: zone.Limit,
                rating: zone.Rating,
                inventories: zone.Inventories.Select(InventoryConverter.ConvertCoreToDBModel).ToList(),
                packages: zone.Packages.Select(PackageConverter.ConvertCoreToDBModel).ToList());
        }
        
        public static ZoneDto ConvertCoreToDtoModel(ZoneCore zone)
        {
            return new ZoneDto(id: zone.Id,
                name: zone.Name,
                address: zone.Address,
                size: zone.Size,
                limit: zone.Limit,
                rating: zone.Rating,
                inventories: zone.Inventories.Select(InventoryConverter.ConvertCoreToDtoModel).ToList(),
                packages: zone.Packages.Select(PackageConverter.ConvertCoreToDtoModel).ToList());
        }
        
        /// <summary>
        /// Преобразовать из модели DTO в модели бизнес логики
        /// </summary>
        /// <param name="zone">Модель DTO</param>
        /// <returns>Модель бизнес логики</returns>
        public static ZoneCore ConvertDtoToCoreModel(ZoneDto zone)
        {
            return new ZoneCore(id: zone.Id,
                name: zone.Name,
                address: zone.Address,
                size: zone.Size,
                limit: zone.Limit,
                rating: zone.Rating,
                inventories: zone.Inventories.Select(InventoryConverter.ConvertDtoToCoreModel).ToList(),
                packages: zone.Packages.Select(PackageConverter.ConvertDtoToCoreModel).ToList());
        }
        
        public static ZoneCore ConvertDtoToCoreModel(UpdateZone zone)
        {
            return new ZoneCore(id: zone.Id,
                name: zone.Name,
                address: zone.Address,
                size: zone.Size,
                limit: zone.Limit,
                rating: 0,
                inventories: zone.Inventories.Select(InventoryConverter.ConvertDtoToCoreModel).ToList(),
                packages: zone.Packages.Select(PackageConverter.ConvertDtoToCoreModel).ToList());
        }
    }
}
