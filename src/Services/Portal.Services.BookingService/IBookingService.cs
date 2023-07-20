using Portal.Common.Models;
using Portal.Common.Models.Enums;

namespace Portal.Services.BookingService
{
    public interface IBookingService
    {
        Task<List<Booking>> GetAllBookingAsync();
        Task<List<Booking>> GetBookingByUserAsync(Guid userId);
        Task<List<Booking>> GetBookingByZoneAsync(Guid zoneId);
        Task<List<FreeTime>> GetReservedTimeAsync(Guid zoneId, DateOnly date);
        Task<Booking> GetBookingByIdAsync(Guid bookingId);
        Task CreateBookingAsync(Guid userId, Guid zoneId, Guid packageId, string date, string startTime, string endTime);
        Task ChangeBookingStatusAsync(Guid bookingId, BookingStatus status);
        Task UpdateBookingAsync(Booking booking);
        Task RemoveBookingAsync(Guid bookingId);
        
        /// <summary>
        /// Определение свободного времени для бронирования зоны
        /// </summary>
        /// <param name="date">Дата брони</param>
        /// <param name="startTime">Начало времени брони</param>
        /// <param name="endTime">Конец времени брони</param>
        /// <returns>Результат проверки</returns>
        Task<bool> IsFreeTimeAsync(DateOnly date, TimeOnly startTime, TimeOnly endTime);
    }
}
