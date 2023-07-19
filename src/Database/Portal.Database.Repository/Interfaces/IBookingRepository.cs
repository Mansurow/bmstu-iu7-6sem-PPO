using Portal.Common.Models;

namespace Portal.Database.Repositories.Interfaces;

public interface IBookingRepository
{
    Task<List<Booking>> GetAllBookingAsync();
    Task<List<Booking>> GetBookingByUserAsync(Guid userId);
    Task<List<Booking>> GetBookingByRoomAsync(Guid roomId);
    Task<List<Booking>> GetBookingByUserAndRoomAsync(Guid userId, Guid roomId);
    Task<Booking> GetUserBookingByZoneForTimeRangeAsync(Guid userId, Guid roomId, DateOnly date, TimeOnly startTime, TimeOnly endTime);
    Task<Booking> GetBookingByIdAsync(Guid bookingId);
    Task<List<Booking>> GetAllBookingByZoneForTimeRangeAsync(Guid roomId, DateOnly date, TimeOnly startTime, TimeOnly endTime);
    Task InsertBookingAsync(Booking createBooking);
    Task UpdateNoActualBookingAsync(Guid bookindId);
    Task UpdateBookingAsync(Booking updateBooking);
    Task DeleteBookingAsync(Guid bookingId);
}
