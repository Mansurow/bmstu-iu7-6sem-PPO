using Anticafe.BL.Models;

namespace Anticafe.BL.IRepositories;

public interface IBookingRepository
{
    Task<List<Booking>> GetAllBookingAsync();
    Task<List<Booking>> GetBookingByUserAsync(int userId);
    Task<List<Booking>> GetBookingByRoomAsync(int roomId);
    Task<List<Booking>> GetBookingByUserAndRoomAsync(int userId, int roomId);
    Task<Booking> GetUserBookingByRoomAsync(int userId, int roomId, DateTime startTime, DateTime endTime);
    Task<Booking> GetBookingByIdAsync(int bookingId);
    Task<List<Booking>> GetAllBookingByRoomForTimeRange(int roomId, DateTime startTime, DateTime endTime);
    Task InsertBookingAsync(Booking createBooking);
    Task UpdateNoActualBookingAsync(int bookindId);
    Task UpdateBookingAsync(Booking updateBooking);
    Task DeleteBookingAsync(int bookingId);
}
