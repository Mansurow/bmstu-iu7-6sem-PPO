using Portal.Common.Core;
using Portal.Common.Enums;
using Portal.Services.BookingService.Exceptions;
using Portal.Services.PackageService.Exceptions;
using Portal.Services.ZoneService.Exceptions;

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
        /// <exception cref="BookingUpdateException">Ошибка при обновлении неактуальных броней</exception>
        Task<List<Booking>> GetAllBookingAsync();
        
        /// <summary>
        /// Получить все брони пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользовтаеля</param>
        /// <returns>Список всех броней пользователя</returns>
        /// <exception cref="BookingUpdateException">Ошибка при обновлении неактуальных броней</exception>
        Task<List<Booking>> GetBookingByUserAsync(Guid userId);
        
        /// <summary>
        /// Получить все брони зоны
        /// </summary>
        /// <param name="zoneId">Идентификатор зоны</param>
        /// <returns>Список всех броней зоны</returns>
        /// <exception cref="BookingUpdateException">Ошибка при обновлении неактуальных броней</exception>
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
        /// <exception cref="BookingNotFoundException">Бронь не найдена</exception>
        /// <exception cref="BookingUpdateException">При обновлении неактуальных броней</exception>
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
        /// <exception cref="ZoneNotFoundException">Зона не найден</exception>
        /// <exception cref="PackageNotFoundException">Пакет не найден</exception>
        /// <exception cref="BookingCreateException">При создании брони</exception>
        /// <exception cref="BookingExistsException">Пользователь уже забронировал зону на указанный день</exception>
        /// <exception cref="BookingReversedException">Бронь уже существует на указанный день и время</exception>
        Task<Guid> AddBookingAsync(Guid userId, Guid zoneId, Guid packageId, DateOnly date, TimeOnly startTime, TimeOnly endTime);
        
        /// <summary>
        /// Изменить статус брони
        /// </summary>
        /// <param name="bookingId">Идентификатор брони</param>
        /// <param name="status">Новый статус брони</param>
        /// <exception cref="BookingNotSuitableStatusException">Неверное изменение статуса</exception>
        /// <exception cref="BookingNotFoundException">Бронь не найдена</exception>
        /// <exception cref="BookingUpdateException">При обновлении брони</exception>
        Task ChangeBookingStatusAsync(Guid bookingId, BookingStatus status);
        
        /// <summary>
        /// Обновить бронь зоны
        /// </summary>
        /// <param name="booking">Данные для обновления брони зоны</param>
        /// <exception cref="BookingChangeDateTimeException">Время указано неверно для изменение. Нельзя бронировать время прошедших дней</exception>
        /// <exception cref="BookingExceedsLimitException">Максимальное количество людей превысило лимит по зоне</exception>
        /// <exception cref="BookingNotFoundException">Бронь не найдена</exception>
        /// <exception cref="BookingUpdateException">При обновлении брони</exception>
        Task UpdateBookingAsync(Booking booking);
        
        /// <summary>
        /// Удалить бронь зоны
        /// </summary>
        /// <param name="bookingId">Идентификатор брони</param>
        /// <exception cref="BookingNotFoundException">Бронь не найдена</exception>
        /// <exception cref="BookingRemoveException">При удалении брони</exception>
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
