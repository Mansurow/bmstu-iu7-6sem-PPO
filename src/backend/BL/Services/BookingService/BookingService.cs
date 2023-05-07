using System.Globalization;
using Anticafe.BL.Exceptions;
using Anticafe.BL.Models;
using Anticafe.Common.Enums;
using Anticafe.DataAccess.Converter;
using Anticafe.DataAccess.IRepositories;

namespace Anticafe.BL.Sevices.BookingService
{
    public class BookingService: IBookingService
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
            var bookings = (await _bookingRepository.GetAllBookingAsync())
                            .Select(b => BookingConverter.ConvertDbModelToAppModel(b)).ToList();

            await UpdateNoActualBookings(bookings);

            return bookings;
        }

        public async Task<List<Booking>> GetBookingByUserAsync(int userId) 
        {
            var bookings = (await _bookingRepository.GetBookingByUserAsync(userId))
                            .Select(b => BookingConverter.ConvertDbModelToAppModel(b)).ToList();
     
            await UpdateNoActualBookings(bookings);

            return bookings;
        }

        private async Task UpdateNoActualBookings(List<Booking> bookings) 
        {
            foreach (var b in bookings)
                if (b.IsBookingExpired() && b.Status is not BookingStatus.NoActual)
                {
                    b.ChangeStatus(BookingStatus.NoActual);
                    await _bookingRepository.UpdateNoActualBookingAsync(b.Id);
                }
        }

        public async Task<List<Booking>> GetBookingByRoomAsync(int roomId)
        {
            var bookings = (await _bookingRepository.GetBookingByRoomAsync(roomId))
                .Select(b => BookingConverter.ConvertDbModelToAppModel(b)).ToList();

            await UpdateNoActualBookings(bookings);

            return bookings;
        }

        public async Task<List<Booking>> GetBookingByUserAndRoomAsync(int userId, int roomId)
        {
            var bookings = (await _bookingRepository.GetBookingByUserAndRoomAsync(userId, roomId))
                           .Select(b => BookingConverter.ConvertDbModelToAppModel(b)).ToList();
            if (bookings is null)
            {
                throw new BookingNotFoundException($"Bookings not found for user with id: {userId} and room with id: {roomId}");
            }

            await UpdateNoActualBookings(bookings);

            return bookings;
        }

        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            var booking = BookingConverter.ConvertDbModelToAppModel(await _bookingRepository.GetBookingByIdAsync(bookingId));
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

        public async Task CreateBookingAsync(int userId, int roomId, int amount, string startTime, string EndTime)
        {
            var culture = CultureInfo.CreateSpecificCulture("ru-RU");
            var startDateTime = DateTime.Parse(startTime, culture);
            var endDateTime = DateTime.Parse(EndTime, culture);

            var userBooking = await _bookingRepository.GetUserBookingByRoomForTimeRangeAsync(userId, roomId, startDateTime, endDateTime);
            if (userBooking is not null)
            {
                throw new BookingReversedException($"User with id: {userId} reversed since {startDateTime} to {endDateTime} for room with id: {roomId}.");
            }

            var bookings = await _bookingRepository.GetAllBookingByRoomForTimeRange(roomId, startDateTime, endDateTime);
            if (bookings.Count > 0) 
            {
                throw new BookingReversedException($"All reversed since {startDateTime} to {endDateTime} for room with id: {roomId}.");
            } 

            var booking = new Booking(1, roomId, userId, amount, startDateTime, endDateTime, BookingStatus.Reserved);
            await _bookingRepository.InsertBookingAsync(BookingConverter.ConvertAppModelToDbModel(booking));
        }

        public async Task ChangeBookingStatusAsync(int bookingId, BookingStatus status) 
        {
            var booking = await GetBookingByIdAsync(bookingId);

            booking.ChangeStatus(status);
            await UpdateBookingAsync(booking);
        }

        public async Task UpdateBookingAsync(Booking updateBooking)
        {
            var bookingDbModel = await _bookingRepository.GetBookingByIdAsync(updateBooking.Id);
            if (bookingDbModel is not null)
            {
                throw new BookingExistsException($"Booking already exists with id: {updateBooking.Id}");
            }

            var booking = BookingConverter.ConvertDbModelToAppModel(bookingDbModel);

            if (booking.IsBookingExpired()) 
                booking.ChangeStatus(BookingStatus.NoActual);

            await _bookingRepository.UpdateBookingAsync(BookingConverter.ConvertAppModelToDbModel(booking));
        }

        public async Task DeleteBookingAsync(int bookingId)
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
