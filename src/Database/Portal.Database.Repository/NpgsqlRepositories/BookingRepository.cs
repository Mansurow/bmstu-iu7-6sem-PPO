using Microsoft.EntityFrameworkCore;
using Portal.Common.Converter;
using Portal.Common.Models;
using Portal.Common.Models.Enums;
using Portal.Database.Context;
using Portal.Database.Repositories.Interfaces;

namespace Portal.Database.Repositories.NpgsqlRepositories;

/// <summary>
/// Репозиторий бронирования
/// </summary>
public class BookingRepository: BaseRepository, IBookingRepository
{
    private readonly PortalDbContext _context;

    public BookingRepository(PortalDbContext context): base()
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task<List<Booking>> GetAllBookingAsync() 
    {
        return _context.Bookings
            .Select(b => BookingConverter.ConvertDbModelToAppModel(b))
            .ToListAsync();
    }

    public Task<List<Booking>> GetBookingByUserAsync(Guid userId) 
    {
        return _context.Bookings.Where(b => b.UserId == userId)
            .Select(b => BookingConverter.ConvertDbModelToAppModel(b))
            .ToListAsync();
    }

    public Task<List<Booking>> GetBookingByZoneAsync(Guid zoneId)
    {
        return _context.Bookings.Where(b => b.ZoneId == zoneId)
            .Select(b => BookingConverter.ConvertDbModelToAppModel(b))
            .ToListAsync();
    }

    public Task<List<Booking>> GetBookingByUserAndZoneAsync(Guid userId, Guid zoneId) 
    {
        return _context.Bookings.Where(b => b.UserId == userId && b.ZoneId == zoneId)
            .Select(b => BookingConverter.ConvertDbModelToAppModel(b))
            .ToListAsync();
    }
    
    public async Task<Booking> GetBookingByIdAsync(Guid bookingId)
    {
        var booking = await _context.Bookings.FirstAsync(b => b.Id == bookingId);

        return BookingConverter.ConvertDbModelToAppModel(booking);
    }

    public async Task InsertBookingAsync(Booking createBooking) 
    {
        var booking = BookingConverter.ConvertAppModelToDbModel(createBooking);
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateNoActualBookingAsync(Guid bookingId) 
    {
        var booking = await _context.Bookings.FirstAsync(b => b.Id == bookingId);
        booking.Status = BookingStatus.NoActual;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBookingAsync(Booking updateBooking) 
    {
        var booking = BookingConverter.ConvertAppModelToDbModel(updateBooking);
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBookingAsync(Guid bookingId) 
    {
        var booking = await _context.Bookings.FirstAsync(b => b.Id == bookingId);
        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();
    }
}
