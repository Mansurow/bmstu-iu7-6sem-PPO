using Portal.Common.Models;
using Portal.Common.Models.Enums;
using Portal.Converter;
using Portal.Database.Repositories.Interfaces;
using Portal.Services.BookingService.Exceptions;
using System.Globalization;

namespace Portal.Services.BookingService
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;

        public BookingService(IBookingRepository bookingRepository,
                              IUserRepository userRepository)
        {
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<List<Booking>> GetAllBookingAsync()
        {
            var bookings = await _bookingRepository.GetAllBookingAsync();

            await UpdateNoActualBookingsAsync(bookings);

            return bookings;
        }

        public async Task<List<Booking>> GetBookingByUserAsync(Guid userId)
        {
            var bookings = await _bookingRepository.GetBookingByUserAsync(userId);

            await UpdateNoActualBookingsAsync(bookings);

            return bookings;
        }

        private async Task UpdateNoActualBookingsAsync(List<Booking> bookings)
        {
            foreach (var b in bookings)
                if (b.IsBookingExpired() && b.Status is not BookingStatus.NoActual)
                {
                    b.ChangeStatus(BookingStatus.NoActual);
                    await _bookingRepository.UpdateNoActualBookingAsync(b.Id);
                }
        }

        public async Task<List<Booking>> GetBookingByRoomAsync(Guid zoneId)
        {
            var bookings = await _bookingRepository.GetBookingByRoomAsync(zoneId);

            await UpdateNoActualBookingsAsync(bookings);

            return bookings;
        }

        public async Task<List<Booking>> GetBookingByUserAndRoomAsync(Guid userId, Guid zoneId)
        {
            var bookings = await _bookingRepository.GetBookingByUserAndRoomAsync(userId, zoneId);
            if (bookings is null)
            {
                throw new BookingNotFoundException($"Bookings not found for user with id: {userId} and room with id: {zoneId}");
            }

            await UpdateNoActualBookingsAsync(bookings);

            return bookings;
        }

        public async Task<Booking> GetBookingByIdAsync(Guid bookingId)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
            if (booking is null)
            {
                throw new BookingNotFoundException($"Bookings not found id: {bookingId}");
            }

            if (booking.IsBookingExpired())
            {
                booking.ChangeStatus(BookingStatus.NoActual);
                await _bookingRepository.UpdateNoActualBookingAsync(booking.Id);
            }

            return booking;
        }

        public async Task CreateBookingAsync(Guid userId, Guid zoneId, int amount, string date, string startTime, string EndTime)
        {
            var culture = CultureInfo.CreateSpecificCulture("ru-RU");
            var dateRu = DateOnly.Parse(date, culture);
            var startTimeRu = TimeOnly.Parse(startTime, culture);
            var endTimeRu = TimeOnly.Parse(EndTime, culture);

            var userBooking = await _bookingRepository.GetUserBookingByZoneForTimeRangeAsync(userId, zoneId, dateRu, startTimeRu, startTimeRu);
            if (userBooking is not null)
            {
                throw new BookingReversedException($"User with id: {userId} reversed since {startTimeRu} to {startTimeRu} on {dateRu} for room with id: {zoneId}.");
            }

            var bookings = await _bookingRepository.GetAllBookingByZoneForTimeRangeAsync(zoneId, dateRu, startTimeRu, endTimeRu);
            if (bookings.Count > 0)
            {
                throw new BookingReversedException($"All reversed since {startTimeRu} to {startTimeRu} on {dateRu} for room with id: {zoneId}.");
            }

            var booking = new Booking(Guid.NewGuid(), zoneId, userId, amount, BookingStatus.Reserved, dateRu, startTimeRu, startTimeRu);
            await _bookingRepository.InsertBookingAsync(booking);
        }

        public async Task ChangeBookingStatusAsync(Guid bookingId, BookingStatus status)
        {
            var booking = await GetBookingByIdAsync(bookingId);

            booking.ChangeStatus(status);
            await UpdateBookingAsync(booking);
        }

        public async Task UpdateBookingAsync(Booking updateBooking)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(updateBooking.Id);
            if (booking is null)
            {
                throw new BookingNotFoundException($"Booking not found with id: {updateBooking.Id}");
            }

            if (booking.IsBookingExpired())
                booking.ChangeStatus(BookingStatus.NoActual);

            await _bookingRepository.UpdateBookingAsync(booking);
        }

        public async Task DeleteBookingAsync(Guid bookingId)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
            if (booking is null)
            {
                throw new BookingNotFoundException($"Booking not found with id: {bookingId}");
            }

            await _bookingRepository.DeleteBookingAsync(bookingId);
        }
    }
}
