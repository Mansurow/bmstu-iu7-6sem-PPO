using Portal.Common.Models;
using Portal.Converter;
using Portal.Database.Models;

namespace Portal.Common.Converter
{
    public static class ZoneConverter
    {
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
