using Anticafe.BL.Models;

namespace Anticafe.BL.IRepositories;

public interface IRoomRepository
{
    Task<List<Room>> GetAllRoomAsync();
    Task<Room> GetRoomByIdAsync(int roomId);
    Task InsertRoomAsync(Room createRoom);
    Task UpdateRoomAsync(Room updateRoom);
    Task DeleteRoomAsync(int roomId);
}
