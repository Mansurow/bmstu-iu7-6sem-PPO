using Portal.Database.Models;
using Portal.Common.Models;

namespace Portal.Converter;

public static class InventoryConverter
{
    public static Inventory ConvertDbModelToAppModel(InventoryDbModel inventory)
    {
        return new Inventory(id: inventory.Id,
                        name: inventory.Name,
                        description: inventory.Description,
                        yearOfProduction: inventory.YearOfProduction);
    }

    public static InventoryDbModel ConvertAppModelToDbModel(Inventory inventory)
    {
        return new InventoryDbModel(id: inventory.Id,
                        name: inventory.Name,
                        description: inventory.Description,
                        yearOfProduction: inventory.YearOfProduction);
    }
}
