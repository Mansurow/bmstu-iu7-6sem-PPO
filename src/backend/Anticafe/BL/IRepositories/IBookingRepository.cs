using Anticafe.BL.Models;

namespace Anticafe.BL.IRepositories;

public interface IBookingRepository
{
    Task<List<Booking>> GetBookingByUserAsync(int userId);
    Task<List<Booking>> GetAllBookingAsync();
    Task AddBookingAsync(Booking createBooking);
    Task UpdateBookingAsync(Booking updateBooking);
    Task DeleteBookingByUserAsync(int userId);
}
