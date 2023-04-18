using Anticafe.BL.Exceptions;
using Anticafe.BL.IRepositories;
using Anticafe.BL.Models;

namespace Anticafe.BL.Sevices.RoomService;

public class RoomService: IRoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository) 
    {
        _roomRepository = roomRepository;
    }

    public async Task<List<Room>> GetAllRoomsAsync()
    {
        return await _roomRepository.GetAllRoomAsync();
    }

    public async Task<Room> GetRoomByIdAsync(int roomId)
    {
        var room = await _roomRepository.GetRoomByIdAsync(roomId);
        if (room is null) 
        {
            throw new RoomNotFoundException($"Room not found in anticafe by id: {roomId}");
        }

        return room;
    }

    public async Task AddRoomAsync(string name, int size, double price)
    {
        var room = await _roomRepository.GetRoomByNameAsync(name);
        if (room is not null)
        {
            throw new RoomNameExistException($"This name \"{name}\" of room exists.");
        }

        var listRooms = await _roomRepository.GetAllRoomAsync();
        if (listRooms is null)
            room = new Room(1, name, size, price, 0, null, null);
        else
            room = new Room(listRooms.Count() + 1, name, size, price, 0, null, null);

        await _roomRepository.InsertRoomAsync(room);
    }

    public async Task UpdateRoomAsync(Room updateRoom)
    {
        if (!_RoomExists(updateRoom.Id).Result)
        {
            throw new RoomNotFoundException($"Room not found in anticafe by id: {updateRoom.Id}");
        }

        var room = await _roomRepository.GetRoomByNameAsync(updateRoom.Name);
        if (room is not null && room.Id != updateRoom.Id)
        {
            throw new RoomNameExistException($"This name \"{updateRoom.Name}\" of room exists.");
        }

        await _roomRepository.UpdateRoomAsync(updateRoom);
    }

    public async Task AddInventoryForRoomAsync(int roomId, Inventory inventory)
    {
        var room = await GetRoomByIdAsync(roomId);

        room.AddInventory(inventory);

        await UpdateRoomAsync(room);
    }

    public async Task DeleteRoomAsync(int roomId)
    {
        if (!_RoomExists(roomId).Result)
        {
            throw new RoomNotFoundException($"Room not found in anticafe by id: {roomId}");
        }

        await _roomRepository.DeleteRoomAsync(roomId);
    }

    private async Task<Boolean> _RoomExists(int roomId) 
    {
        var room = await _roomRepository.GetRoomByIdAsync(roomId);
        var res = false;
        if (room != null)
            res = true;
        return res;
    }
}
