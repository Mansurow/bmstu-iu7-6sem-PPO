using Anticafe.DataAccess.DBModels;

namespace Anticafe.DataAccess.IRepositories;
public interface IRoomRepository
{
    Task<List<RoomDbModel>> GetAllRoomAsync();
    Task<RoomDbModel> GetRoomByIdAsync(int roomId);
    Task<RoomDbModel> GetRoomByNameAsync(string roomName);
    Task InsertRoomAsync(RoomDbModel createRoom);
    Task UpdateRoomAsync(RoomDbModel updateRoom);
    Task UpdateRoomRaitingAsync(int roomId, int raiting);
    Task DeleteRoomAsync(int roomId);
}
