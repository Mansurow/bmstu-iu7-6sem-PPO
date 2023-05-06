using Anticafe.Common.Enums;
using Anticafe.DataAccess.DBModels;
using Anticafe.DataAccess.Exceptions;
using Anticafe.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Anticafe.DataAccess.Repositories;

public class BookingRepository: BaseRepository, IBookingRepository
{
    private readonly AppDbContext _context;

    public BookingRepository(AppDbContext context): base()
    {
        _context = context;
    }

    public async Task<List<BookingDbModel>> GetAllBookingAsync() 
    {
        return await _context.Bookings.ToListAsync();
    }

    public async Task<List<BookingDbModel>> GetBookingByUserAsync(int userId) 
    {
        return await _context.Bookings.Where(b => b.UserId == userId).ToListAsync();
    }

    public async Task<List<BookingDbModel>> GetBookingByRoomAsync(int roomId)
    {
        return await _context.Bookings.Where(b => b.RoomId == roomId).ToListAsync();
    }

    public async Task<List<BookingDbModel>> GetBookingByUserAndRoomAsync(int userId, int roomId) 
    {
        return await _context.Bookings.Where(b => b.RoomId == roomId && b.UserId == userId).ToListAsync();
    }

    public async Task<BookingDbModel> GetUserBookingByRoomAsync(int userId, int roomId, DateTime startTime, DateTime endTime)
    {
        return await _context.Bookings.Where(b => b.RoomId == roomId
                                               && b.UserId == userId
                                               && b.StartTime >= startTime
                                               && b.EndTime <= endTime).FirstOrDefaultAsync();
    }

    public async Task<List<BookingDbModel>> GetAllBookingByRoomForTimeRange(int roomId, DateTime startTime, DateTime endTime)
    {
        return await _context.Bookings.Where(b => b.RoomId == roomId
                                               && b.StartTime >= startTime
                                               && b.EndTime <= endTime).ToListAsync();
    }

    public async Task<BookingDbModel> GetBookingByIdAsync(int bookingId) 
    {
        var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId);
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
            _context.Bookings.Add(createBooking);
            await _context.SaveChangesAsync();
        } catch
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
            _context.Bookings.Update(updateBooking);
            await _context.SaveChangesAsync();
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
            var booking = await GetBookingByIdAsync(bookingId);
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }
        catch
        {
            throw new BookingDeleteException("Booking not delete");
        }
    }
}
