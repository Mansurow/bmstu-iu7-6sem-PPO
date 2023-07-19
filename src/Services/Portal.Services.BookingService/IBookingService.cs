using Portal.Common.Models;
using Portal.Common.Models.Enums;

namespace Portal.Services.BookingService
{
    public interface IBookingService
    {
        Task<List<Booking>> GetAllBookingAsync();
        Task<List<Booking>> GetBookingByUserAsync(Guid userId);
        Task<List<Booking>> GetBookingByRoomAsync(Guid roomId);
        Task<List<Booking>> GetBookingByUserAndRoomAsync(Guid userId, Guid roomId);
        Task<Booking> GetBookingByIdAsync(Guid bookingId);
        Task CreateBookingAsync(Guid userId, Guid roomId, int amount, string date, string startTime, string EndTime);
        Task ChangeBookingStatusAsync(Guid bookingId, BookingStatus status);
        Task UpdateBookingAsync(Booking booking);
        Task DeleteBookingAsync(Guid bookingId);

    }
}
