using System.Security.Claims;
using HotChocolate.Authorization;
using Portal.Common.Converter;
using Portal.Common.Core;
using Portal.Common.Enums;
using Portal.Services.BookingService;
using Booking = Portal.Common.Dto.Booking.Booking;

namespace Portal.Graphql.Queries;

[ExtendObjectType("Query")]
public class BookingQuery
{
    /// <summary>
    /// Получить брони зон.
    /// </summary>
    /// <param name="bookingService"></param>
    /// <returns>Список броней.</returns>
    [GraphQLName("GetBookings")]
    [Authorize(Roles = new []{nameof(Role.Administrator)})]
    public async Task<IEnumerable<Booking>> GetBookings([Service(ServiceKind.Resolver)] IBookingService bookingService)
    {
        var booking = await bookingService.GetAllBookingAsync();

        return booking.Select(BookingConverter.ConvertCoreToDtoModel);
    }
    
    /// <summary>
    /// Получить брони зон пользователя.
    /// </summary>
    /// <param name="bookingService"></param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Список броней.</returns>
    [GraphQLName("GetUserBookings")]
    [Authorize]
    public async Task<IEnumerable<Booking>> GetUserBookings([Service(ServiceKind.Resolver)] IBookingService bookingService,
        Guid userId)
    {
        var booking = await bookingService.GetBookingByUserAsync(userId);

        return booking.Select(BookingConverter.ConvertCoreToDtoModel);
    }
    
    /// <summary>
    /// Получить брони зоны.
    /// </summary>
    /// <param name="bookingService"></param>
    /// <param name="zoneId">Идентификатор зоны.</param>
    /// <returns>Список броней.</returns>
    [GraphQLName("GetZoneBookings")]
    [Authorize(Roles = new []{nameof(Role.Administrator)})]
    public async Task<IEnumerable<Booking>> GetZoneBookings([Service(ServiceKind.Resolver)] IBookingService bookingService,
        Guid zoneId)
    {
        var booking = await bookingService.GetBookingByZoneAsync(zoneId);

        return booking.Select(BookingConverter.ConvertCoreToDtoModel);
    }
    
    /// <summary>
    /// Получить свободное время для бронирования зоны.
    /// </summary>
    /// <param name="bookingService"></param>
    /// <param name="zoneId">Идентификатор зоны.</param>
    /// <param name="date">Дата брони.</param>
    /// <returns>Список свободного времени для бронирвоания зоны.</returns>
    [GraphQLName("GetFreeTime")]
    [Authorize]
    public async Task<IEnumerable<FreeTime>> GetFreeTime([Service(ServiceKind.Resolver)] IBookingService bookingService,
        Guid zoneId, DateOnly? date)
    {
        var freeTimes = await bookingService.GetFreeTimeAsync(zoneId, date);

        return freeTimes;
    }
}