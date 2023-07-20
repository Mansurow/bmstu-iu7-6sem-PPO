using Portal.Common.Models;

namespace Portal.Database.Repositories.Interfaces;

public interface IBookingRepository
{
    Task<List<Booking>> GetAllBookingAsync();
    Task<List<Booking>> GetBookingByUserAsync(Guid userId);
    Task<List<Booking>> GetBookingByZoneAsync(Guid zoneId);
    Task<List<Booking>> GetBookingByUserAndZoneAsync(Guid userId, Guid zoneId);
    Task<Booking> GetBookingByIdAsync(Guid bookingId);
    Task InsertBookingAsync(Booking createBooking);
    Task UpdateNoActualBookingAsync(Guid bookingId);
    Task UpdateBookingAsync(Booking updateBooking);
    Task DeleteBookingAsync(Guid bookingId);
}
