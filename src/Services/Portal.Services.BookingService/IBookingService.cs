﻿using Portal.Common.Models;
using Portal.Common.Models.Enums;

namespace Portal.Services.BookingService
{
    /// <summary>
    /// Интерфейс Сервиса бронирования зон
    /// </summary>
    public interface IBookingService
    {
        /// <summary>
        /// Получить все брони 
        /// </summary>
        /// <returns>Список всех броней</returns>
        Task<List<Booking>> GetAllBookingAsync();
        
        /// <summary>
        /// Получить все брони пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользовтаеля</param>
        /// <returns>Список всех броней пользователя</returns>
        Task<List<Booking>> GetBookingByUserAsync(Guid userId);
        
        /// <summary>
        /// Получить все брони зоны
        /// </summary>
        /// <param name="zoneId">Идентификатор зоны</param>
        /// <returns>Список всех броней зоны</returns>
        Task<List<Booking>> GetBookingByZoneAsync(Guid zoneId);
        
        /// <summary>
        /// Получить список свободного времени для бронирования
        /// </summary>
        /// <param name="zoneId">Идентификатор зоны</param>
        /// <param name="date">Дата бронирования</param>
        /// <returns>Список свободного времени</returns>
        Task<List<FreeTime>> GetFreeTimeAsync(Guid zoneId, DateOnly date);
        
        /// <summary>
        /// Получить бронь зоны
        /// </summary>
        /// <param name="bookingId">Идентификатор брони</param>
        /// <returns>Бронь комнаты</returns>
        Task<Booking> GetBookingByIdAsync(Guid bookingId);
        
        /// <summary>
        /// Создать бронь зоны
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="zoneId">Идентификатор зоны</param>
        /// <param name="packageId">Идентификатор пакета</param>
        /// <param name="date">Дата брони</param>
        /// <param name="startTime">Время брони (начало)</param>
        /// <param name="endTime">Время брони (конец)</param>
        Task<Guid> AddBookingAsync(Guid userId, Guid zoneId, Guid packageId, string date, string startTime, string endTime);
        
        /// <summary>
        /// Изменить статус брони
        /// </summary>
        /// <param name="bookingId">Идентификатор брони</param>
        /// <param name="status">Новый статус брони</param>
        /// <returns></returns>
        Task ChangeBookingStatusAsync(Guid bookingId, BookingStatus status);
        
        /// <summary>
        /// Обновить бронь зоны
        /// </summary>
        /// <param name="booking">Данные для обновления брони зоны</param>
        /// <returns></returns>
        Task UpdateBookingAsync(Booking booking);
        
        /// <summary>
        /// Удалить бронь зоны
        /// </summary>
        /// <param name="bookingId">Идентификатор брони</param>
        /// <returns></returns>
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
