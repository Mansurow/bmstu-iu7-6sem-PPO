using Portal.Common.Converter;
using Portal.Database.Models;
using Portal.Common.Models;

namespace Portal.Converter;

public static class PackageConverter
{
    public static Package ConvertDbModelToAppModel(PackageDbModel package)
    {
        return new Package(id: package.Id,
            name: package.Name,
            type: package.Type,
            price: package.Price,
            rentalTime: package.RentalTime,
            description: package.Description,
            dishes: package.Dishes.Select(d => MenuConverter.ConvertDbModelToAppModel(d)).ToList(),
            zones: package.Zones.Select(z => ZoneConverter.ConvertDbModelToAppModel(z)).ToList());
    }

    public static PackageDbModel ConvertAppModelToDbModel(Package package)
    {
        return new PackageDbModel(id: package.Id,
            name: package.Name,
            type: package.Type,
            price: package.Price,
            rentalTime: package.RentalTime,
            description: package.Description,
            dishes: package.Dishes.Select(d => MenuConverter.ConvertAppModelToDbModel(d)).ToList(),
            zones: package.Zones.Select(z => ZoneConverter.ConvertAppModelToDbModel(z)).ToList());
    }
}
