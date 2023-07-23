using Portal.Common.Models;

namespace Portal.Database.Repositories.Interfaces;

/// <summary>
/// Интерфейс репозитория броней зон
/// </summary>
public interface IBookingRepository
{
    /// <summary>
    /// Получить все брони зон
    /// </summary>
    /// <returns>Список всех броней зон</returns>
    Task<List<Booking>> GetAllBookingAsync();
    
    /// <summary>
    /// Получить все брони зон для пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns>Список всех броней зон для пользователя</returns>
    Task<List<Booking>> GetBookingByUserAsync(Guid userId);
    
    /// <summary>
    /// Получить все брони зоны
    /// </summary>
    /// <param name="zoneId">Идентификатор зоны</param>
    /// <returns>Список всех броней зоны</returns>
    Task<List<Booking>> GetBookingByZoneAsync(Guid zoneId);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="zoneId"></param>
    /// <returns></returns>
    Task<List<Booking>> GetBookingByUserAndZoneAsync(Guid userId, Guid zoneId);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bookingId"></param>
    /// <returns></returns>
    Task<Booking> GetBookingByIdAsync(Guid bookingId);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="createBooking"></param>
    /// <returns></returns>
    Task InsertBookingAsync(Booking createBooking);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bookingId"></param>
    /// <returns></returns>
    Task UpdateNoActualBookingAsync(Guid bookingId);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="updateBooking"></param>
    /// <returns></returns>
    Task UpdateBookingAsync(Booking updateBooking);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bookingId"></param>
    /// <returns></returns>
    Task DeleteBookingAsync(Guid bookingId);
}
