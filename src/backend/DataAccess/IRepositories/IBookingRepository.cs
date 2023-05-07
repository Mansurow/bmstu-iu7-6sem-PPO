using Anticafe.DataAccess.DBModels;

namespace Anticafe.DataAccess.IRepositories;

public interface IBookingRepository
{
    Task<List<BookingDbModel>> GetAllBookingAsync();
    Task<List<BookingDbModel>> GetBookingByUserAsync(int userId);
    Task<List<BookingDbModel>> GetBookingByRoomAsync(int roomId);
    Task<List<BookingDbModel>> GetBookingByUserAndRoomAsync(int userId, int roomId);
    Task<BookingDbModel> GetUserBookingByRoomForTimeRangeAsync(int userId, int roomId, DateTime startTime, DateTime endTime);
    Task<BookingDbModel> GetBookingByIdAsync(int bookingId);
    Task<List<BookingDbModel>> GetAllBookingByRoomForTimeRange(int roomId, DateTime startTime, DateTime endTime);
    Task InsertBookingAsync(BookingDbModel createBooking);
    Task UpdateNoActualBookingAsync(int bookindId);
    Task UpdateBookingAsync(BookingDbModel updateBooking);
    Task DeleteBookingAsync(int bookingId);
}
