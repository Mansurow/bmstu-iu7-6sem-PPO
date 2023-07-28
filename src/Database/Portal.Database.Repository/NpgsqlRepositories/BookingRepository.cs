using Anticafe.DataAccess.Exceptions;
using Microsoft.EntityFrameworkCore;
using Portal.Common.Converter;
using Portal.Common.Models;
using Portal.Common.Models.Enums;
using Portal.Database.Context;
using Portal.Database.Models;
using Portal.Database.Repositories.Interfaces;

namespace Portal.Database.Repositories.Repositories;

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
        return _context.Bookings.Select(b => BookingConverter.ConvertDbModelToAppModel(b)!)
            .ToListAsync();
    }

    public Task<List<Booking>> GetBookingByUserAsync(Guid userId) 
    {
        return _context.Bookings.Where(b => b.UserId == userId)
            .Select(b => BookingConverter.ConvertDbModelToAppModel(b)!)
            .ToListAsync();
    }

    public Task<List<Booking>> GetBookingByZoneAsync(Guid zoneId)
    {
        return _context.Bookings.Where(b => b.ZoneId == zoneId)
            .Select(b => BookingConverter.ConvertDbModelToAppModel(b)!)
            .ToListAsync();
    }

    public Task<List<Booking>> GetBookingByUserAndZoneAsync(Guid userId, Guid zoneId) 
    {
        return _context.Bookings.Where(b => b.UserId == userId && b.ZoneId == zoneId)
            .Select(b => BookingConverter.ConvertDbModelToAppModel(b)!)
            .ToListAsync();
    }
    
    public async Task<Booking> GetBookingByIdAsync(Guid bookingId)
    {
        var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId);

        return BookingConverter.ConvertDbModelToAppModel(booking);
    }

    public async Task InsertBookingAsync(Booking createBooking) 
    {
        try
        {
            var booking = BookingConverter.ConvertAppModelToDbModel(createBooking);
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        } catch
        {
            throw new BookingCreateException("Booking not create");
        }
    }

    public async Task UpdateNoActualBookingAsync(Guid bookingId) 
    {
        var booking = await GetBookingByIdAsync(bookingId);
        booking.Status = BookingStatus.NoActual;
        await UpdateBookingAsync(booking);
    }

    public async Task UpdateBookingAsync(Booking updateBooking) 
    {
        try
        {
            var booking = BookingConverter.ConvertAppModelToDbModel(updateBooking);
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }
        catch
        {
            throw new BookingUpdateException("Booking not update");
        }
    }

    public async Task DeleteBookingAsync(Guid bookingId) 
    {
        try
        {
            var booking = await _context.Bookings.FirstAsync(b => b.Id == bookingId);
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }
        catch
        {
            throw new BookingDeleteException("Booking not delete");
        }
    }
}
