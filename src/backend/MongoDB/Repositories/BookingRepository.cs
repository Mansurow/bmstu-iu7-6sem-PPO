using Anticafe.BL.Models;
using Anticafe.Common.Enums;
using Anticafe.DataAccess.DBModels;
using Anticafe.DataAccess.Exceptions;
using Anticafe.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Anticafe.MongoDB.Repositories;

public class BookingRepository: BaseRepository, IBookingRepository
{
    private readonly IMongoCollection<BookingDbModel> _bookingCollection;

    public BookingRepository(IDbCollectionFactory collections) 
    {
        _bookingCollection = collections.GetBookingCollection();
    }

    public async Task<List<BookingDbModel>> GetAllBookingAsync()
    {
        return await _bookingCollection.Find(_ => true).ToListAsync();
    }

    public async Task<List<BookingDbModel>> GetBookingByUserAsync(int userId)
    {
        return await _bookingCollection.Find(b => b.UserId == userId).ToListAsync();
    }

    public async Task<List<BookingDbModel>> GetBookingByRoomAsync(int roomId)
    {
        return await _bookingCollection.Find(b => b.RoomId == roomId).ToListAsync();
    }

    public async Task<List<BookingDbModel>> GetBookingByUserAndRoomAsync(int userId, int roomId)
    {
        return await _bookingCollection.Find(b => b.RoomId == roomId && b.UserId == userId).ToListAsync();
    }

    public async Task<BookingDbModel> GetUserBookingByRoomForTimeRangeAsync(int userId, int roomId, DateTime startTime, DateTime endTime)
    {
        return await _bookingCollection.Find(b => b.RoomId == roomId
                                               && b.UserId == userId
                                               && b.StartTime >= startTime
                                               && b.EndTime <= endTime).FirstOrDefaultAsync();
    }

    public async Task<List<BookingDbModel>> GetAllBookingByRoomForTimeRange(int roomId, DateTime startTime, DateTime endTime)
    {
        return await _bookingCollection.Find(b => b.RoomId == roomId
                                               && b.StartTime >= startTime
                                               && b.EndTime <= endTime).ToListAsync();
    }

    public async Task<BookingDbModel> GetBookingByIdAsync(int bookingId)
    {
        var booking = await _bookingCollection.Find(b => b.Id == bookingId).FirstOrDefaultAsync();
        if (booking is null)
        {
            throw new BookingNotFoundException("Booking not found");
        }

        return booking;
    }

    public async Task InsertBookingAsync(BookingDbModel createBooking)
    {
        try
        {
            await _bookingCollection.InsertOneAsync(createBooking);
        }
        catch
        {
            throw new BookingCreateException("Booking not create");
        }
    }

    public async Task UpdateNoActualBookingAsync(int bookindId)
    {
        var booking = await GetBookingByIdAsync(bookindId);
        booking.Status = BookingStatus.NoActual;
        await UpdateBookingAsync(booking);
    }

    public async Task UpdateBookingAsync(BookingDbModel updateBooking)
    {
        try
        {
            var filter = Builders<BookingDbModel>.Filter.Eq(u => u.Id, updateBooking.Id);
            var update = Builders<BookingDbModel>.Update.Set(u => u.RoomId, updateBooking.RoomId)
                                                         .Set(u => u.StartTime, updateBooking.StartTime)
                                                         .Set(u => u.EndTime, updateBooking.EndTime)
                                                         .Set(u => u.Status, updateBooking.Status);
            await _bookingCollection.UpdateOneAsync(filter, update);
        }
        catch
        {
            throw new BookingUpdateException("Booking not update");
        }
    }

    public async Task DeleteBookingAsync(int bookingId)
    {
        try
        {
            var filter = Builders<BookingDbModel>.Filter.Lt(u => u.Id, bookingId);
            await _bookingCollection.DeleteOneAsync(filter);
        }
        catch
        {
            throw new BookingDeleteException("Booking not delete");
        }
    }
}
