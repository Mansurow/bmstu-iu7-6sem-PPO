using HotChocolate.Authorization;
using Portal.Common.Dto;
using Portal.Common.Dto.Booking;
using Portal.Common.Enums;
using Portal.Services.BookingService;

namespace Portal.Graphql.Mutations;

[ExtendObjectType("Mutation")]
public class BookingMutation
{
    /// <summary>
    /// Забронировать зону.
    /// </summary>
    /// <param name="bookingService"></param>
    /// <param name="createBooking">Данные для брони зоны.</param>
    /// <returns>Идентификатор брони зоны.</returns>
    [GraphQLName("ReverseBooking")]
    [Authorize]
    public async Task<IdResponse> ReverseBooking([Service(ServiceKind.Resolver)] IBookingService bookingService, CreateBooking createBooking)
    {
        var bookingId = await bookingService.AddBookingAsync(
            createBooking.UserId, createBooking.ZoneId, createBooking.PackageId,
            createBooking.Date, createBooking.StartTime, createBooking.EndTime);
        
        return new IdResponse(bookingId);
    }
    
    /// <summary>
    /// Подтвердить бронь зоны.
    /// </summary>
    /// <param name="bookingService"></param>
    /// <param name="confirmBooking">Данные для подтверждения брони зоны.</param>
    [GraphQLName("ConfirmBooking")]
    [Authorize]
    public async Task ConfirmBooking([Service(ServiceKind.Resolver)] IBookingService bookingService, ConfirmBooking confirmBooking)
    {
        var booking = await bookingService.GetBookingByIdAsync(confirmBooking.Id);
        booking.Date = confirmBooking.Date;
        booking.StartTime = confirmBooking.StartTime;
        booking.EndTime = confirmBooking.EndTime;
        booking.AmountPeople = confirmBooking.AmountPeople;
        booking.PackageId = confirmBooking.PackageId;
        booking.ChangeStatus(BookingStatus.Reserved);

        await bookingService.UpdateBookingAsync(booking);
    }
    
    /// <summary>
    /// Отменить зону.
    /// </summary>
    /// <param name="bookingService"></param>
    /// <param name="bookingId">Идентификатор зоны.</param>
    [GraphQLName("CancellBooking")]
    [Authorize]
    public async Task<IdResponse> CancellBooking([Service(ServiceKind.Resolver)] IBookingService bookingService, Guid bookingId)
    {
        await bookingService.ChangeBookingStatusAsync(bookingId, BookingStatus.Cancelled);

        return new IdResponse(bookingId);
    }
}