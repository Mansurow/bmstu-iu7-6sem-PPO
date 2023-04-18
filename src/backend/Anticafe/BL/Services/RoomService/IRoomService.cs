using Anticafe.BL.Models;

namespace Anticafe.BL.Sevices.RoomService;

public interface IRoomService
{
    Task<List<Room>> GetAllRoomsAsync();
    Task<Room> GetRoomByIdAsync(int roomId);
    Task AddRoomAsync(string name, int size, double price);
    Task UpdateRoomAsync(Room createRoom);
    Task DeleteRoomAsync(int roomId);
    Task AddInventoryForRoomAsync(int roomId, Inventory inventory);
}
