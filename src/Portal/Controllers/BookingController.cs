using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Common.Models;
using Portal.Common.Models.Dto;
using Portal.Common.Models.Enums;
using Portal.Services.BookingService;
using Portal.Services.BookingService.Exceptions;
using Portal.Services.PackageService.Exceptions;
using Portal.Services.ZoneService.Exceptions;

namespace Portal.Controllers;

/// <summary>
/// Контроллер бронирования
/// </summary>
[ApiController]
[Route("api/v1/bookings/")]
public class BookingController: ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<BookingController> _logger;
    
    /// <summary>
    /// Конструктор контроллера бронирования
    /// </summary>
    /// <param name="bookingService"></param>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public BookingController(IBookingService bookingService, ILogger<BookingController> logger)
    {
        _bookingService = bookingService ?? throw new ArgumentNullException(nameof(bookingService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [Authorize(Roles = nameof(Role.Administrator))]
    public async Task<IActionResult> GetBookings()
    {
        try
        {
            var bookings = await _bookingService.GetAllBookingAsync();

            return Ok(bookings);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                message = e.Message
            });
        }
    }
    
    [HttpGet("user/{userId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetUserBookings(Guid userId)
    {
        try
        {
            var bookings = await _bookingService.GetBookingByUserAsync(userId);

            return Ok(bookings);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                message = e.Message
            });
        }
    }
    
    [HttpGet("zone/{zoneId:guid}")]
    [Authorize(Roles = nameof(Role.Administrator))]
    public async Task<IActionResult> GetZoneBookings(Guid zoneId)
    {
        try
        {
            var bookings = await _bookingService.GetBookingByZoneAsync(zoneId);

            return Ok(bookings);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                message = e.Message
            });
        }
    }

    [HttpGet("freetime/{zoneId:guid}&{date}")]
    [Authorize]
    public async Task<IActionResult> GetFreeTime([FromRoute] Guid zoneId, [FromRoute] string date)
    {
        try
        {
            var freeTime = await _bookingService.GetFreeTimeAsync(zoneId, DateOnly.Parse(date));

            return Ok(freeTime);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                message = e.Message
            });
        }
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> PostBooking([FromBody] CreateBookingDto bookingDto)
    {
        try
        {
            var bookingId = await _bookingService.AddBookingAsync(bookingDto.UserId, bookingDto.ZoneId,
                bookingDto.PackageId,
                bookingDto.Date, bookingDto.StartTime, bookingDto.EndTime);

            return Ok(new { bookingId });
        }
        catch (BookingExistsException e)
        {
            _logger.LogError(e, "Zone: {ZoneId} already has been reversed by user: {UserId}", bookingDto.ZoneId,
                bookingDto.UserId);
            return BadRequest(new
            {
                message = e.Message
            });
        }
        catch (BookingReversedException e)
        {
            _logger.LogError(e, "Zone: {ZoneId} already has been reversed", bookingDto.ZoneId);
            return BadRequest(new
            {
                message = e.Message
            });
        }
        catch (ZoneNotFoundException e)
        {
            _logger.LogError(e, "Zone: {ZoneId} not found", bookingDto.ZoneId);
            return BadRequest(new
            {
                message = e.Message
            });
        }
        catch (PackageNotFoundException e)
        {
            _logger.LogError(e, "Package: {PackageId} not found", bookingDto.PackageId);
            return BadRequest(new
            {
                message = e.Message
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                message = e.Message
            });
        }
    }
    
    [HttpPatch]
    [Authorize(Roles = nameof(Role.User))]
    public async Task<IActionResult> ConfirmBooking([FromBody] ConfirmBookingDto bookingDto)
    {
        try
        {
            // TODO: проверка на подтверждение только собственной брони   
            
            var booking = await _bookingService.GetBookingByIdAsync(bookingDto.Id);
            booking.Date = bookingDto.Date;
            booking.StartTime = booking.StartTime;
            booking.EndTime = booking.EndTime;
            booking.AmountPeople = bookingDto.AmountPeople;
            booking.PackageId = bookingDto.PackageId;
            booking.ChangeStatus(BookingStatus.Reserved);

            await _bookingService.UpdateBookingAsync(booking);

            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (BookingNotFoundException e)
        {
            _logger.LogError(e, "Booking {BookingId} not found", bookingDto.Id);
            return NotFound(new
            {
                message = e.Message
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                message = e.Message
            });
        }
    }
    
    // TODO: возможно нужен фикс
    [HttpDelete("{bookingId:guid}")]
    [Authorize]
    public async Task<IActionResult> CancelBooking([FromRoute] Guid bookingId)
    {
        try
        {
            // TODO: проверка на отмену собственной брони или админом

            await _bookingService.ChangeBookingStatusAsync(bookingId, BookingStatus.Cancelled);

            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (BookingNotFoundException e)
        {
            _logger.LogError(e, "Booking {BookingId} not found", bookingId);
            return NotFound(new
            {
                message = e.Message
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                message = e.Message
            });
        }
    }
}