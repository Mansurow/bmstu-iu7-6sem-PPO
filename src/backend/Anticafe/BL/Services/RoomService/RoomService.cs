using System.Runtime.CompilerServices;
using Anticafe.BL.IRepositories;
using Anticafe.BL.Models;

namespace Anticafe.BL.Sevices.RoomService;

public class RoomService: IRoomService
{
    private readonly IRoomRepository _roomRepository;

    RoomService(IRoomRepository roomRepository) 
    {
        _roomRepository = roomRepository;
    }

    public async Task<List<Room>> GetAllRoomsAsync()
    {
        return (await _roomRepository.GetAllRoomAsync());
    }

    public async Task<Room> GetRoomById(int roomId)
    {
        return (await _roomRepository.GetRoomByIdAsync(roomId));
    }

    public async Task AddRoomAsync(Room createRoom)
    {
        if (RoomExists(createRoom.Id).Result)
        {
            throw new Exception();
        }
        await _roomRepository.InsertRoomAsync(createRoom);
    }

    public async Task UpdateRoomAsync(Room createRoom)
    {
        if (!RoomExists(createRoom.Id).Result)
        {
            throw new Exception();
        }

        await _roomRepository.UpdateRoomAsync(createRoom);
    }

    public async Task DeleteRoomAsync(int roomId)
    {
        if (!RoomExists(roomId).Result)
        {
            throw new Exception();
        }

        await _roomRepository.DeleteRoomAsync(roomId);
    }

    private async Task<Boolean> RoomExists(int roomId) 
    {
        var room = await _roomRepository.GetRoomByIdAsync(roomId);
        var res = false;
        if (room != null)
            res = true;
        return res;
    }
}
