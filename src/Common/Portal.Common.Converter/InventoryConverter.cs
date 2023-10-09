using InventoryCore = Portal.Common.Core.Inventory;
using InventoryDB = Portal.Database.Models.InventoryDbModel;
using InventoryDto = Portal.Common.Dto.Inventory.Inventory;

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
    public static InventoryCore ConvertDBToCoreModel(InventoryDB inventory)
    {
        return new InventoryCore(id: inventory.Id,
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
    public static InventoryDB ConvertCoreToDBModel(InventoryCore inventory)
    {
        return new InventoryDB(id: inventory.Id,
        zoneId: inventory.ZoneId,
        name: inventory.Name,
        description: inventory.Description,
        yearOfProduction: inventory.YearOfProduction,
        isWrittenOff: inventory.IsWrittenOff);
    }
    
    /// <summary>
    /// Преобразовать из модели бизнес логики в модели DTO
    /// </summary>
    /// <param name="inventory">Модель бизнес логики</param>
    /// <returns>Модель DTO</returns>
    public static InventoryDto ConvertCoreToDtoModel(InventoryCore inventory)
    {
        return new InventoryDto(id: inventory.Id,
            zoneId: inventory.ZoneId,
            name: inventory.Name,
            description: inventory.Description,
            yearOfProduction: inventory.YearOfProduction,
            isWrittenOff: inventory.IsWrittenOff);
    }
    
    /// <summary>
    /// Преобразовать из модели DTO в модели бизнес логики
    /// </summary>
    /// <param name="inventory">Модель DTO</param>
    /// <returns>Модель бизнес логики</returns>
    public static InventoryCore ConvertDtoToCoreModel(InventoryDto inventory)
    {
        return new InventoryCore(id: inventory.Id,
            zoneId: inventory.ZoneId,
            name: inventory.Name,
            description: inventory.Description,
            yearOfProduction: inventory.YearOfProduction,
            isWrittenOff: inventory.IsWrittenOff);
    }
}
