﻿using System.Net.NetworkInformation;
using Anticafe.BL.Models;
using Anticafe.DataAccess.DBModels;
using Common.Models.DTO;

namespace Anticafe.DataAccess.Converter
{
    public static class RoomConverter
    {
        public static Room ConvertDbModelToAppModel(RoomDbModel room) 
        {
            return new Room(id: room.Id,
                            name: room.Name,
                            size: room.Size,
                            price: room.Price,
                            rating: room.Rating,
                            inventories: room.Inventories?.Select(i => InventoryConverter.ConvertDbModelToAppModel(i)).ToList());
        }

        public static RoomDbModel ConvertAppModelToDbModel(Room room)
        {
            return new RoomDbModel(id: room.Id,
                            name: room.Name,
                            size: room.Size,
                            price: room.Price,
                            rating: room.Rating,
                            inventories: room.Inventories?.Select(i => InventoryConverter.ConvertAppModelToDbModel(i)).ToList());
        }

        public static RoomDto ConvertAppModelToDto(Room room)
        {
            return new RoomDto(id: room.Id,
                            name: room.Name,
                            size: room.Size,
                            price: room.Price,
                            rating: room.Rating,
                            inventories: room.Inventories?.Select(i => InventoryConverter.ConvertAppModelToDto(i)).ToList());
        }

        public static Room ConvertDtoToAppModel(RoomDto room)
        {
            return new Room(id: room.Id,
                            name: room.Name,
                            size: room.Size,
                            price: room.Price,
                            rating: room.Rating,
                            inventories: room.Inventories?.Select(i => InventoryConverter.ConvertDtoToAppModel(i)).ToList());
        }
    }
}
