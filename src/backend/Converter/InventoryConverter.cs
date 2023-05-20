using Anticafe.BL.Models;
using Anticafe.DataAccess.DBModels;
using Common.Models.DTO;

namespace Anticafe.DataAccess.Converter
{
    public static class InventoryConverter
    {
        public static Inventory ConvertDbModelToAppModel(InventoryDbModel inventory)
        {
            return new Inventory(id: inventory.Id,
                            name: inventory.Name,
                            rooms: inventory.Rooms is null ? null : inventory.Rooms.Select(r => RoomConverter.ConvertDbModelToAppModel(r)).ToList());
        }

        public static InventoryDbModel ConvertAppModelToDbModel(Inventory inventory)
        {
            return new InventoryDbModel(id: inventory.Id,
                            name: inventory.Name,
                            rooms: inventory.Rooms is null ? null : inventory.Rooms.Select(r => RoomConverter.ConvertAppModelToDbModel(r)).ToList());
        }

        public static InventoryDto ConvertAppModelToDto(Inventory inventory)
        {
            return new InventoryDto(id: inventory.Id,
                            name: inventory.Name);
        }

        public static Inventory ConvertDtoToAppModel(InventoryDto inventory)
        {
            return new Inventory(id: inventory.Id,
                            name: inventory.Name,
                            rooms: new List<Room>());
        }
    }
}
