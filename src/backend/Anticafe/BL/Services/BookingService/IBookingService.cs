using Anticafe.BL.Enums;
using Anticafe.BL.Models;

namespace Anticafe.BL.Sevices.BookingService
{
    public interface IBookingService
    {
        Task<List<Booking>> GetAllBookingAsync();
        Task<List<Booking>> GetBookingByUserAsync(int userId);
        Task<List<Booking>> GetBookingByRoomAsync(int roomId);
        Task<List<Booking>> GetBookingByUserAndRoomAsync(int userId, int roomId);
        Task<Booking> GetBookingByIdAsync(int bookingId);
        Task CreateBookingAsync(int userId, int roomId, string startTime, string EndTime);
        Task ChangeBookingStatusAsync(int bookingId, BookingStatus status);
        Task UpdateBookingAsync(Booking booking);
        Task DeleteBookingAsync(int bookingId);

    }
}
