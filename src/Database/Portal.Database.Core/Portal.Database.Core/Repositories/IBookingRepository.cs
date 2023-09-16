using Portal.Common.Models;

namespace Portal.Database.Core.Repositories;

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
    /// Получить все брони пользователя по зоне
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="zoneId">Идентификатор зоны</param>
    /// <returns></returns>
    Task<List<Booking>> GetBookingByUserAndZoneAsync(Guid userId, Guid zoneId);
    
    /// <summary>
    /// Получить бронь
    /// </summary>
    /// <param name="bookingId">Идентификатор брони</param>
    /// <returns></returns>
    Task<Booking> GetBookingByIdAsync(Guid bookingId);
    
    /// <summary>
    /// Создать бронь
    /// </summary>
    /// <param name="createBooking">Данные модели зоны</param>
    /// <returns></returns>
    Task InsertBookingAsync(Booking createBooking);
    
    /// <summary>
    /// Обновить статус зоны на неактуальный
    /// </summary>
    /// <param name="bookingId">Идентификатор зоны</param>
    /// <returns></returns>
    Task UpdateNoActualBookingAsync(Guid bookingId);
    
    /// <summary>
    /// Обновить бронь
    /// </summary>
    /// <param name="updateBooking">Дынные бронь для обновление</param>
    /// <returns></returns>
    Task UpdateBookingAsync(Booking updateBooking);
    
    /// <summary>
    /// Удалить бронь
    /// </summary>
    /// <param name="bookingId">Идентификатор брони</param>
    /// <returns></returns>
    Task DeleteBookingAsync(Guid bookingId);
}
