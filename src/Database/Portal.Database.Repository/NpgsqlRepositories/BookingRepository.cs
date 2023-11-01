using Microsoft.EntityFrameworkCore;
using Portal.Common.Converter;
using Portal.Common.Core;
using Portal.Common.Enums;
using Portal.Database.Context;
using Portal.Database.Core.Repositories;

namespace Portal.Database.Repositories.NpgsqlRepositories;

/// <summary>
/// Репозиторий бронирования
/// </summary>
public class BookingRepository: BaseRepository, IBookingRepository
{
    private readonly PortalDbContext _context;

    public BookingRepository(PortalDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task<List<Booking>> GetAllBookingAsync() 
    {
        return _context.Bookings
            .OrderBy(b => b.Date)
            .Select(b => BookingConverter.ConvertDBToCoreModel(b))
            .ToListAsync();
    }

    public Task<List<Booking>> GetBookingByUserAsync(Guid userId) 
    {
        return _context.Bookings.Where(b => b.UserId == userId)
            .OrderBy(b => b.Date)
            .Select(b => BookingConverter.ConvertDBToCoreModel(b))
            .ToListAsync();
    }

    public async Task<List<Booking>> GetBookingByZoneAsync(Guid zoneId)
    {
        var zone = await _context.Zones.FirstAsync(z => z.Id == zoneId);
        
        return await _context.Bookings.Where(b => b.ZoneId == zone.Id)
            .OrderBy(b => b.Date)
            .Select(b => BookingConverter.ConvertDBToCoreModel(b))
            .ToListAsync();
    }

    public Task<List<Booking>> GetBookingByUserAndZoneAsync(Guid userId, Guid zoneId) 
    {
        return _context.Bookings.Where(b => b.UserId == userId && b.ZoneId == zoneId)
            .OrderBy(b => b.Date)
            .Select(b => BookingConverter.ConvertDBToCoreModel(b))
            .ToListAsync();
    }
    
    public async Task<Booking> GetBookingByIdAsync(Guid bookingId)
    {
        var booking = await _context.Bookings.FirstAsync(b => b.Id == bookingId);

        return BookingConverter.ConvertDBToCoreModel(booking);
    }

    public async Task InsertBookingAsync(Booking createBooking) 
    {
        var booking = BookingConverter.ConvertCoreToDBModel(createBooking);
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateNoActualBookingAsync(Guid bookingId) 
    {
        var booking = await _context.Bookings.FirstAsync(b => b.Id == bookingId);
        booking.Status = BookingStatus.Done;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBookingAsync(Booking updateBooking) 
    {
        var booking = await _context.Bookings.FirstAsync(b => b.Id == updateBooking.Id);

        // booking.UserId = updateBooking.UserId;
        // booking.ZoneId = updateBooking.ZoneId;
        booking.PackageId = updateBooking.PackageId;
        booking.Package = await _context.Packages.FirstAsync(p => p.Id == updateBooking.PackageId);
        booking.Status = updateBooking.Status;
        booking.AmountPeople = updateBooking.AmountPeople;
        booking.Date = updateBooking.Date;
        booking.StartTime = updateBooking.StartTime;
        booking.EndTime = updateBooking.EndTime;
        booking.TotalPrice = updateBooking.TotalPrice;
        booking.Date = updateBooking.Date;

        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBookingAsync(Guid bookingId) 
    {
        var booking = await _context.Bookings.FirstAsync(b => b.Id == bookingId);
        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();
    }
}
