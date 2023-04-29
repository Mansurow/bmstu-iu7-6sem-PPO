using Anticafe.BL.Models;
using Anticafe.DataAccess.DBModels;
using Anticafe.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Anticafe.DataAccess.Repositories;

public class RoomRepository: BaseRepository, IRoomRepository
{
    private readonly AppDbContext _context;

    public RoomRepository(AppDbContext context): base()
    {
        _context = context;
    }
    public async Task<List<RoomDbModel>> GetAllRoomAsync() 
    {
        return await _context.Rooms.ToListAsync();
    }

    public async Task<RoomDbModel> GetRoomByIdAsync(int roomId) 
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == roomId);
        if (room is null) 
        {
            throw new Exception("Room not found");
        }

        return room;
    }

    public async Task<RoomDbModel> GetRoomByNameAsync(string roomName) 
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Name == roomName);
        if (room is null)
        {
            throw new Exception("Room not found");
        }

        return room;
    }

    public async Task InsertRoomAsync(RoomDbModel createRoom) 
    {
        try
        {
            _context.Rooms.Add(createRoom);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Feedback not create");
        }
    }

    public async Task UpdateRoomAsync(RoomDbModel updateRoom) 
    {
        try
        {
            _context.Rooms.Update(updateRoom);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Feedback not create");
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
            var room = await GetRoomByIdAsync(roomId);
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Feedback not create");
        }
    }
}
