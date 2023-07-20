using Portal.Common.Models;
using Portal.Common.Models.Enums;
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
        
        private async Task UpdateNoActualBookingsAsync(IEnumerable<Booking> bookings)
        {
            foreach (var b in bookings.Where(b => b.IsBookingExpired() 
                                                  && b.Status is not BookingStatus.NoActual))
            {
                b.ChangeStatus(BookingStatus.NoActual);
                await _bookingRepository.UpdateNoActualBookingAsync(b.Id);
            }
        }
        
        public async Task<List<Booking>> GetBookingByZoneAsync(Guid zoneId)
        {
            var bookings = await _bookingRepository.GetBookingByZoneAsync(zoneId);

            await UpdateNoActualBookingsAsync(bookings);

            return bookings;
        }
        
        public async Task<Booking> GetBookingByIdAsync(Guid bookingId)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
            if (booking is null)
            {
                throw new BookingNotFoundException($"Bookings with id: {bookingId} not found");
            }

            if (booking.IsBookingExpired())
            {
                booking.ChangeStatus(BookingStatus.NoActual);
                await _bookingRepository.UpdateNoActualBookingAsync(booking.Id);
            }

            return booking;
        }

        public async Task<List<FreeTime>> GetReservedTimeAsync(Guid zoneId, DateOnly date)
        {
            var bookings = (await GetBookingByZoneAsync(zoneId))
                .FindAll(e => e.Date == date).OrderBy(e => e.StartTime).ToList();
            

            var freeTimes = new List<FreeTime>();
            var startTimeWork = new TimeOnly(8, 0);
            var endTimeWork = new TimeOnly(23, 0);
            for (var i = 0; i < bookings.Count; i++)
            {
                FreeTime addFreeTime;
                if (i == 0)
                {
                    addFreeTime = startTimeWork < bookings[i].StartTime
                        ? new FreeTime(startTimeWork, bookings[i].EndTime)
                        : new FreeTime(bookings[i].EndTime, bookings[i + 1].StartTime);
                }
                else if (i == bookings.Count - 1)
                {
                    addFreeTime = new FreeTime(bookings[i].EndTime, endTimeWork);
                }
                else
                {
                    if (freeTimes.Count > 0 && freeTimes[^1].EndTime < bookings[i].StartTime)
                    {
                        addFreeTime = new FreeTime(freeTimes[^1].EndTime, bookings[i].StartTime);
                    }
                    else
                    {
                        addFreeTime = new FreeTime(bookings[i].EndTime, bookings[i + 1].StartTime);
                    }
                }
                
                if (addFreeTime.EndTime - addFreeTime.StartTime >= new TimeSpan(1, 0, 0))
                    freeTimes.Add(addFreeTime);
            }

            return freeTimes;
        }

        public async Task CreateBookingAsync(Guid userId, Guid zoneId, Guid packageId, string date, string startTime, string endTime)
        {
            var culture = CultureInfo.CreateSpecificCulture("ru-RU");
            var dateRu = DateOnly.Parse(date, culture);
            var startTimeRu = TimeOnly.Parse(startTime, culture);
            var endTimeRu = TimeOnly.Parse(endTime, culture);
            
            var booking = _bookingRepository.GetBookingByUserAndZoneAsync(userId, zoneId);
            if (booking is not null)
            {
                throw new BookingExistsException($"User with id: {userId} reversed for zone with id: {zoneId}");
            }
                
            if (!await IsFreeTimeAsync(dateRu, startTimeRu, endTimeRu))
            {
                throw new BookingReversedException($"Zone full or partial reversed on {dateRu} from {startTime} to {endTime}");
            }
            
            await _bookingRepository.InsertBookingAsync(
                new Booking(Guid.NewGuid(), zoneId, userId, packageId, 
                1, BookingStatus.TemporaryReserved, 
                dateRu, startTimeRu, startTimeRu));
        }
        
        public async Task<bool> IsFreeTimeAsync(DateOnly date, TimeOnly startTime, TimeOnly endTime)
        {
            return (await _bookingRepository.GetAllBookingAsync())
                .FindAll(b => b.Date == date && b.Status == BookingStatus.NoActual)
                .All(b => (b.StartTime <= startTime && b.EndTime < startTime) 
                                || (b.StartTime >= endTime && b.EndTime > startTime));
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
                throw new BookingNotFoundException($"Booking with id: {updateBooking.Id} not found");
            }

            if (booking.IsBookingExpired())
                booking.ChangeStatus(BookingStatus.NoActual);

            await _bookingRepository.UpdateBookingAsync(booking);
        }

        public async Task RemoveBookingAsync(Guid bookingId)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
            if (booking is null)
            {
                throw new BookingNotFoundException($"Booking with id: {bookingId} not found");
            }

            await _bookingRepository.DeleteBookingAsync(bookingId);
        }
    }
}
