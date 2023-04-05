using Anticafe.BL.Models;

namespace Anticafe.BL.Sevices.RoomService;

public interface IRoomService
{
    Task<List<Room>> GetAllRoomsAsync();
    Task<Room> GetRoomById(int roomId);
    Task AddRoomAsync(Room createRoom);
    Task UpdateRoomAsync(Room createRoom);
    Task DeleteRoomAsync(int roomId);
}
