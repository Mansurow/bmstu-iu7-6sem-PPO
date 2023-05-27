using Anticafe.BL.Sevices.BookingService;
using Anticafe.DataAccess.Converter;
using Anticafe.DataAccess.Exceptions;
using Common.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Anticafe.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    private readonly ILogger<BookingController> _logger;

    public BookingController(IBookingService bookingService, ILogger<BookingController> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    [HttpGet("user/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookingDto[]))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetAllUserBookings([FromRoute] int userId)
    {
        try
        {
            var bookings = await _bookingService.GetBookingByUserAsync(userId);
            _logger.LogInformation("Get all rooms information successfully.");
            return Ok(bookings.Select(BookingConverter.ConvertAppModelToDto).ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("{bookingId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookingDto))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetBookingInfo([FromRoute] int bookingId)
    {
        try
        {
            var booking = await _bookingService.GetBookingByIdAsync(bookingId);
            _logger.LogInformation("Get booking information successfully.");
            return Ok(BookingConverter.ConvertAppModelToDto(booking));
        }
        catch (BookingNotFoundException ex)
        {
            _logger.LogError(ex, "{ex.Message}", ex.Message);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> CallcelBooking([FromBody] BookingDto bookingDto)
    {
        try
        {
            await _bookingService.CreateBookingAsync(bookingDto.UserId, bookingDto.RoomId, bookingDto.AmountPeople, bookingDto.StartTime, bookingDto.EndTime);
            _logger.LogInformation("Booking add successfully.");
            return Ok();
        }
        catch (RoomNotFoundException ex)
        {
            _logger.LogError(ex, "Room not exist: {ex.Message}", ex.Message);
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
        catch (UserNotFoundByIdException ex)
        {
            _logger.LogError(ex, "User not exist: {ex.Message}", ex.Message);
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete("{bookingId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> DeleteBooking([FromRoute] int bookingId)
    {
        try
        {
            await _bookingService.DeleteBookingAsync(bookingId);
            _logger.LogInformation("Booking successfully delete.");
            return Ok();
        }
        catch (BookingNotFoundException ex)
        {
            _logger.LogError(ex, "Booking not exist: {ex.Message}", ex.Message);
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
