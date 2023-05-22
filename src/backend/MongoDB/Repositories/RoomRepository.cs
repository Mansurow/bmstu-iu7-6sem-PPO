using Anticafe.DataAccess.DBModels;
using Anticafe.DataAccess.Exceptions;
using Anticafe.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Anticafe.MongoDB.Repositories;

public class RoomRepository: BaseRepository, IRoomRepository
{
    private readonly IMongoCollection<RoomDbModel> _roomCollection;

    public RoomRepository(IDbCollectionFactory collections)
    {
        _roomCollection = collections.GetRoomCollection();
    }
    public async Task<List<RoomDbModel>> GetAllRoomAsync()
    {
        return await _roomCollection.Find(_ => true).ToListAsync();
    }

    public async Task<RoomDbModel> GetRoomByIdAsync(int roomId)
    {
        var room = await _roomCollection.Find(r => r.Id == roomId).FirstOrDefaultAsync();
        if (room is null)
        {
            throw new RoomNotFoundException("Room not found");
        }
        return room;
    }

    public async Task<RoomDbModel> GetRoomByNameAsync(string roomName)
    {
        var room = await _roomCollection.Find(r => r.Name == roomName).FirstOrDefaultAsync();
        return room;
    }

    public async Task InsertRoomAsync(RoomDbModel createRoom)
    {
        try
        {
            await _roomCollection.InsertOneAsync(createRoom);
        }
        catch
        {
            throw new RoomCreateException("Room not create");
        }
    }

    public async Task UpdateRoomAsync(RoomDbModel updateRoom)
    {
        try
        {
            var filter = Builders<RoomDbModel>.Filter.Eq(u => u.Id, updateRoom.Id);
            var update = Builders<RoomDbModel>.Update.Set(u => u.Name, updateRoom.Name)
                                                     .Set(u => u.Size, updateRoom.Size)
                                                     .Set(u => u.Price, updateRoom.Price)
                                                     .Set(u => u.Rating, updateRoom.Rating)
                                                     .Set(u => u.Inventories, updateRoom.Inventories);
            await _roomCollection.UpdateOneAsync(filter, update);
        }
        catch
        {
            throw new RoomUpdateException("Feedback not create");
        }
    }

    public async Task UpdateRoomRaitingAsync(int roomId, int raiting)
    {
        var room = await GetRoomByIdAsync(roomId);
        room.Rating = raiting;
        await UpdateRoomAsync(room);

    }
    public async Task DeleteRoomAsync(int roomId)
    {
        try
        {
            var filter = Builders<RoomDbModel>.Filter.Lt(u => u.Id, roomId);
            await _roomCollection.DeleteOneAsync(filter);
        }
        catch
        {
            throw new RoomDeleteException("Feedback not create");
        }
    }
}
