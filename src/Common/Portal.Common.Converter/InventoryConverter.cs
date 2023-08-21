using Portal.Common.Models;
using Portal.Database.Models;

namespace Portal.Common.Converter;

/// <summary>
/// Конвертатор модели Inventory
/// </summary>
public static class InventoryConverter
{
    /// <summary>
    /// Преобразовать из модели базы данных в модель бизнес логики приложения
    /// </summary>
    /// <param name="inventory">Модель базы данных</param>
    /// <returns>Модель бизнес логики</returns>
    public static Inventory ConvertDbModelToAppModel(InventoryDbModel inventory)
    {
        return new Inventory(id: inventory.Id,
            zoneId: inventory.ZoneId,    
            name: inventory.Name,
            description: inventory.Description,
            yearOfProduction: inventory.YearOfProduction,
            isWrittenOff: inventory.IsWrittenOff);
    }

    /// <summary>
    /// Преобразовать из модели бизнес логики в модели базы данных приложения
    /// </summary>
    /// <param name="inventory">Модель бизнес логики</param>
    /// <returns>Модель базы данных </returns>
    public static InventoryDbModel ConvertAppModelToDbModel(Inventory inventory)
    {
        return new InventoryDbModel(id: inventory.Id,
        zoneId: inventory.ZoneId,
        name: inventory.Name,
        description: inventory.Description,
        yearOfProduction: inventory.YearOfProduction,
        isWrittenOff: inventory.IsWrittenOff);
    }
}
