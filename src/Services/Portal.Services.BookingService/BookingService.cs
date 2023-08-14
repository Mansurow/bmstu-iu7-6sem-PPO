using Portal.Common.Models;
using Portal.Common.Models.Enums;
using Portal.Database.Repositories.Interfaces;
using Portal.Services.BookingService.Exceptions;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portal.Services.PackageService.Exceptions;
using Portal.Services.ZoneService.Exceptions;

namespace Portal.Services.BookingService
{
    /// <summary>
    ///  Сервис бронирования зон
    /// </summary>
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IPackageRepository _packageRepository;
        private readonly IZoneRepository _zoneRepository;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IBookingRepository bookingRepository, 
            IPackageRepository packageRepository,
            IZoneRepository zoneRepository,
            ILogger<BookingService> logger)
        {
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
            _packageRepository = packageRepository ?? throw new ArgumentNullException(nameof(packageRepository));
            _zoneRepository = zoneRepository ?? throw new ArgumentNullException(nameof(zoneRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<Booking>> GetAllBookingAsync()
        {
            try
            {
                var bookings = await _bookingRepository.GetAllBookingAsync();

                await UpdateNoActualBookingsAsync(bookings);

                return bookings;
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e,"Error while updating no actual bookings while getting all bookings");
                throw new BookingUpdateException("No actual bookings were not updated");
            }
        }

        public async Task<List<Booking>> GetBookingByUserAsync(Guid userId)
        {
            try
            {
                var bookings = await _bookingRepository.GetBookingByUserAsync(userId);

                await UpdateNoActualBookingsAsync(bookings);

                return bookings;
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e,"Error while updating no actual bookings when getting bookings for user: {UserId}", userId);
                throw new BookingUpdateException("No actual bookings were not updated");
            }
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
            try
            {
                var bookings = await _bookingRepository.GetBookingByZoneAsync(zoneId);

                await UpdateNoActualBookingsAsync(bookings);

                return bookings;
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error while updating no actual bookings when getting bookings for zone: {ZoneId}", zoneId);
                throw new BookingUpdateException("No actual bookings were not updated");
            }
            
        }
        
        public async Task<Booking> GetBookingByIdAsync(Guid bookingId)
        {
            try
            {
                var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
                
                if (booking.IsBookingExpired())
                {
                    booking.ChangeStatus(BookingStatus.NoActual);
                    await _bookingRepository.UpdateNoActualBookingAsync(booking.Id);
                }
                
                return booking;
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e,"Bookings with id: {BookingId} not found", bookingId);
                throw new BookingNotFoundException($"Bookings with id: {bookingId} not found");
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error while updating no actual booking: {BookingId}", bookingId);
                throw new BookingUpdateException("No actual bookings were not updated");
            }
        }

        public async Task<List<FreeTime>> GetFreeTimeAsync(Guid zoneId, DateOnly date)
        {
            var bookings = (await GetBookingByZoneAsync(zoneId))
                .FindAll(e => e.Date == date && e.Status != BookingStatus.NoActual)
                .OrderBy(e => e.StartTime).ToList();
            
            var freeTimes = new List<FreeTime>();
            var startTimeWork = new TimeOnly(8, 0);
            var endTimeWork = new TimeOnly(23, 0);
            for (var i = 0; i < bookings.Count; i++)
            {
                FreeTime addFreeTime;
                if (i == 0)
                {
                    addFreeTime = new FreeTime(startTimeWork, bookings[i].StartTime);
                }
                else if (i == bookings.Count - 1)
                {
                    addFreeTime = new FreeTime(bookings[i].EndTime, endTimeWork);
                }
                else
                {
                    addFreeTime = new FreeTime(bookings[i - 1].EndTime, bookings[i].StartTime);
                }
                
                if (addFreeTime.EndTime - addFreeTime.StartTime >= new TimeSpan(1, 0, 0))
                    freeTimes.Add(addFreeTime);
            }

            return freeTimes.OrderBy(f => f.StartTime).ToList();
        }

        public async Task<Guid> AddBookingAsync(Guid userId, Guid zoneId, Guid packageId, string date, string startTime, string endTime)
        {
            try
            {
                var culture = CultureInfo.CreateSpecificCulture("ru-RU");
                var dateRu = DateOnly.Parse(date, culture);
                var startTimeRu = TimeOnly.Parse(startTime, culture);
                var endTimeRu = TimeOnly.Parse(endTime, culture);
                
                var zone = await _zoneRepository.GetZoneByIdAsync(zoneId);

                try
                {
                    await _packageRepository.GetPackageByIdAsync(packageId);
                }
                catch (InvalidOperationException e)
                {
                    _logger.LogError(e, "Package with id: {PackageId} not found", packageId);
                    throw new PackageNotFoundException($"Package with id: {packageId} not found");
                }
                
                var booking = (await _bookingRepository.GetBookingByUserAndZoneAsync(userId, zoneId))
                    .FirstOrDefault(b => b.Date == dateRu);
                if (booking is not null)
                {
                    _logger.LogError("User with id: {UserId} reversed for zone with id: {ZoneId}", userId, zoneId);
                    throw new BookingExistsException($"User with id: {userId} reversed for zone with id: {zoneId}");
                }

                if (!await IsFreeTimeAsync(dateRu, startTimeRu, endTimeRu))
                {
                    _logger.LogError("Zone full or partial reversed on {Date} from {StartTime} to {EndTime}", date, startTime, endTime);
                    throw new BookingReversedException(
                        $"Zone full or partial reversed on {date} from {startTime} to {endTime}");
                }

                var newBooking = new Booking(Guid.NewGuid(), zoneId, userId, packageId,
                    zone.Limit, BookingStatus.TemporaryReserved,
                    dateRu, startTimeRu, endTimeRu);
                await _bookingRepository.InsertBookingAsync(newBooking);

                return newBooking.Id;
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e, "Zone with id: {ZoneId} not found", zoneId);
                throw new ZoneNotFoundException($"Zone with id: {zoneId} not found");
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error while creating booking");
                throw new BookingCreateException("Booking has bot been created");
            }
        }
        
        public async Task<bool> IsFreeTimeAsync(DateOnly date, TimeOnly startTime, TimeOnly endTime)
        {
            var bookings =  (await _bookingRepository.GetAllBookingAsync())
                .FindAll(b => b.Date == date);
            
            return bookings.Count != 0 
                   && bookings.All(b => (b.StartTime < startTime && b.EndTime <= startTime) 
                                        || (b.StartTime >= endTime && b.EndTime > startTime));
        }
        
        public async Task ChangeBookingStatusAsync(Guid bookingId, BookingStatus status)
        {
            try
            {
                var booking =  await _bookingRepository.GetBookingByIdAsync(bookingId);

                if (!booking.IsSuitableStatus(status))
                {
                    _logger.LogError("Changing for booking with id: {BookingId} for user: {UserId} isn't suitable for next step", booking.Id, booking.UserId);
                    throw new BookingNotSuitableStatusException(
                        $"Changing for booking with id: {booking.Id} for user: {booking.UserId} isn't suitable for next step");
                }

                booking.ChangeStatus(status);
                await _bookingRepository.UpdateBookingAsync(booking);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e,"Bookings with id: {BookingId} not found", bookingId);
                throw new BookingNotFoundException($"Booking with id: {bookingId} not found");
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error while updating no actual bookings when getting changing status for booking: {BookingId}", bookingId);
                throw new BookingUpdateException($"No actual booking: {bookingId} was not updated");
            }
        }

        public async Task UpdateBookingAsync(Booking updateBooking)
        {
            try
            {
                var booking = await _bookingRepository.GetBookingByIdAsync(updateBooking.Id);

                var zone = await _zoneRepository.GetZoneByIdAsync(updateBooking.ZoneId);
                if (zone is not null && zone.Limit < updateBooking.AmountPeople)
                {
                    _logger.LogError("Exceed limit amount of people for booking with id: {BookingId}", booking.Id);
                    throw new BookingExceedsLimitException(
                        $"Exceed limit amount of people for booking with id: {updateBooking.Id}");
                }

                if (booking.IsChangeDateTime(updateBooking))
                {
                    _logger.LogError("Changing date or time for booking with id: {BookingId}", booking.Id);
                    throw new BookingChangeDateTimeException(
                        $"Changing date or time for booking with id: {booking.Id}");
                }

                // if (booking.Status != BookingStatus.NoActual && booking.IsBookingExpired())
                //     booking.ChangeStatus(BookingStatus.NoActual);

                await _bookingRepository.UpdateBookingAsync(updateBooking);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e, "Bookings with id: {BookingId} not found", updateBooking.Id);
                throw new BookingNotFoundException($"Booking with id: {updateBooking.Id} not found");
            }
        }

        public async Task RemoveBookingAsync(Guid bookingId)
        {
            try
            {
                await _bookingRepository.DeleteBookingAsync(bookingId);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e, "Bookings with id: {BookingId} not found", bookingId);
                throw new BookingNotFoundException($"Booking with id: {bookingId} not found");
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error while removing booking: {BookingId}", bookingId);
                throw new BookingRemoveException($"Booking with id: {bookingId} has not been removed");
            }
        }
    }
}
