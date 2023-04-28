using Anticafe.BL.Models;
using Anticafe.DataAccess.DBModels;

namespace Anticafe.DataAccess.Converter
{
    public static class InventoryConverter
    {
        public static Inventory ConvertDbModelToAppModel(InventoryDbModel inventory)
        {
            return new Inventory(id: inventory.Id,
                            name: inventory.Name,
                            rooms: inventory.Rooms?.Select(r => RoomConverter.ConvertDbModelToAppModel(r)).ToList());
        }

        public static InventoryDbModel ConvertAppModelToDbModel(Inventory inventory)
        {
            return new InventoryDbModel(id: inventory.Id,
                            name: inventory.Name,
                            rooms: inventory.Rooms?.Select(r => RoomConverter.ConvertAppModelToDbModel(r)).ToList());
        }
    }
}
