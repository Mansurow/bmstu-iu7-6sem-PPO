using Portal.Common.Models;
using Portal.Database.Models;

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
        public static Zone ConvertDbModelToAppModel(ZoneDbModel zone) 
        {
            return new Zone(id: zone.Id,
                name: zone.Name,
                address: zone.Address,
                size: zone.Size,
                limit: zone.Limit,
                price: zone.Price,
                rating: zone.Rating,
                inventories: zone.Inventories.Select(i => InventoryConverter.ConvertDbModelToAppModel(i)).ToList(),
                packages: zone.Packages.Select(p => PackageConverter.ConvertDbModelToAppModel(p)).ToList());
        }

        public static ZoneDbModel ConvertAppModelToDbModel(Zone zone)
        {
            return new ZoneDbModel(id: zone.Id,
                name: zone.Name,
                address: zone.Address,
                size: zone.Size,
                limit: zone.Limit,
                price: zone.Price,
                rating: zone.Rating,
                inventories: zone.Inventories.Select(i => InventoryConverter.ConvertAppModelToDbModel(i)).ToList(),
                packages: zone.Packages.Select(p => PackageConverter.ConvertAppModelToDbModel(p)).ToList());
        }
    }
}
