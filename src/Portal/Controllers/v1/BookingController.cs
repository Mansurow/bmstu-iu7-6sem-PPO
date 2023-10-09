using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Common.Core;
using Portal.Common.Dto;
using Portal.Common.Dto.Booking;
using Portal.Common.Enums;
using Portal.Common.Models.Dto;
using Portal.Services.BookingService;
using Portal.Services.BookingService.Exceptions;
using Portal.Services.PackageService.Exceptions;
using Portal.Services.UserService.Exceptions;
using Portal.Services.ZoneService.Exceptions;
using Swashbuckle.AspNetCore.Annotations;
using BookingDto = Portal.Common.Dto.Booking.Booking;

namespace Portal.Controllers.v1;

/// <summary>
/// Контроллер бронирования.
/// </summary>
[ApiController]
[Route("api/v1/bookings/")]
public class BookingController: ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<BookingController> _logger;
    
    /// <summary>
    /// Конструктор контроллера бронирования.
    /// </summary>
    /// <param name="bookingService">Сервис бронирования зон.</param>
    /// <param name="logger">Инструмент логгирования.</param>
    public BookingController(IBookingService bookingService, ILogger<BookingController> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>
    /// Получить брони зон.
    /// </summary>
    /// <returns>Список броней зон.</returns>
    /// <response code="200">OK. Список бронь зон.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpGet]
    [Authorize(Roles = nameof(Role.Administrator))]
    [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<BookingDto>), description: "Список броней зон.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя недостаточно прав доступа.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
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
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(e));
        }
    }
    
    /// <summary>
    /// Получить всех броней пользователя.
    /// </summary>
    /// <param name="userId" example="f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6">Идентификатор пользователя.</param>
    /// <returns>Список все бронь пользователя.</returns>
    /// <response code="200">OK. Список всех броней пользователя.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="404">NotFound. Пользователь не найден.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpGet("user/{userId:guid}")]
    [Authorize]
    [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<BookingDto>), description: "Список броней зон.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя недостаточно прав доступа.")]
    [SwaggerResponse(statusCode: 404, description: "Пользователь не найден.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> GetUserBookings(Guid userId)
    {
        try
        {
            var bookings = await _bookingService.GetBookingByUserAsync(userId);

            return Ok(bookings);
        }
        catch (UserNotFoundException e)
        {
            _logger.LogError(e, "User {UserId} not found", userId);
            return NotFound(new
            {
                message = e.Message
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(e));
        }
    }
    
    /// <summary>
    /// Получить все брони определенной зоны.
    /// </summary>
    /// <returns>Список все бронь пользователя.</returns>
    /// <param name="zoneId" example="f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6">Идентификатор зоны.</param>
    /// <response code="200">OK. Список все бронь пользователя.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="403">NotFound. Зона не найдена.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpGet("zone/{zoneId:guid}")]
    [Authorize(Roles = nameof(Role.Administrator))]
    [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<BookingDto>), description: "Список броней зон.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя недостаточно прав доступа.")]
    [SwaggerResponse(statusCode: 404, description: "Пользователь не найден.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> GetZoneBookings(Guid zoneId)
    {
        try
        {
            var bookings = await _bookingService.GetBookingByZoneAsync(zoneId);

            return Ok(bookings);
        }
        catch (ZoneNotFoundException e)
        {
            _logger.LogError(e, "Zone {ZoneId} not found", zoneId);
            return NotFound(new
            {
                message = e.Message
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(e));
        }
    }

    /// <summary>
    /// Получить свободное время для бронирования зоны.
    /// </summary>
    /// <param name="zoneId" example="f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6">Идентификатор зоны.</param>
    /// <param name="date" example="10.08.2023">Дата бронирования.</param>
    /// <returns>Список свободного времени для бронирования зоны.</returns>
    /// <response code="200">OK. Список свободного времени для бронирования зоны.</response>
    /// <response code="400">Bad request. Некорректные данные</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpGet("freetime/{zoneId:guid}&{date}")]
    [Authorize]
    [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<FreeTime>), description: "Список свободного времени для бронирования зоны.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя недостаточно прав доступа.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
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
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(e));
        }
    }
    
    /// <summary>
    /// Забронировать зоны.
    /// </summary>
    /// <param name="bookingDto">Данные для создании брони зоны.</param>
    /// <response code="204">NoContent. Бронь успешно создана.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpPost]
    [Authorize]
    [SwaggerResponse(statusCode: 204, description: "Бронь успешно создана.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя недостаточно прав доступа.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> PostBooking([FromBody] CreateBooking bookingDto)
    {
        try
        {
            var bookingId = await _bookingService.AddBookingAsync(bookingDto.UserId, bookingDto.ZoneId,
                bookingDto.PackageId,
                bookingDto.Date, bookingDto.StartTime, bookingDto.EndTime);

            return Ok(new IdResponse() { Id = bookingId });
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
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(e));
        }
    }
    
    /// <summary>
    /// Подтвердить зону.
    /// </summary>
    /// <param name="bookingDto">Данные для подтверждении брони зоны.</param>
    /// <response code="204">NoContent. Бронь успешно подтверждена.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="404">NotFound. Бронь зоны не найдена.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpPatch]
    [Authorize(Roles = nameof(Role.User))]
    [SwaggerResponse(statusCode: 204, description: "Бронь успешно подтверждена.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя недостаточно прав доступа.")]
    [SwaggerResponse(statusCode: 404, description: "Бронь зоны не найдена.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> ConfirmBooking([FromBody] ConfirmBooking bookingDto)
    {
        try
        {
            var responseUserId = Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var responseUserRole = User.Claims.Single(c => c.Type == ClaimTypes.Role).Value;
            
            var booking = await _bookingService.GetBookingByIdAsync(bookingDto.Id);
            
            if (responseUserId != booking.UserId && responseUserRole != Role.Administrator.ToString())
            {
                _logger.LogError("Access Denied for cancelling booking {BookingId}", bookingDto.Id);
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    message = $"Access Denied for cancelling booking {bookingDto.Id}"
                });
            }
            
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
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(e));
        }
    }
    
    /// <summary>
    /// Отменить бронь зоны. 
    /// </summary>
    /// <param name="bookingId" example="f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6">Идентификатор брони зоны.</param>
    /// <response code="204">NoContent. Бронь успешно отменена.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="404">NotFound. Бронь зоны не найдена.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpDelete("{bookingId:guid}")]
    [Authorize]
    [SwaggerResponse(statusCode: 204, description: "Бронь успешно отменена.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя недостаточно прав доступа.")]
    [SwaggerResponse(statusCode: 404, description: "Бронь зоны не найдена.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> CancellBooking([FromRoute] Guid bookingId)
    {
        try
        {
            var responseUserId = Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var responseUserRole = User.Claims.Single(c => c.Type == ClaimTypes.Role).Value;

            var booking = await _bookingService.GetBookingByIdAsync(bookingId);
            
            if (responseUserId != booking.UserId && responseUserRole != Role.Administrator.ToString())
            {
                _logger.LogError("Access Denied for cancelling booking {BookingId}", bookingId);
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    message = $"Access Denied for cancelling booking {bookingId}"
                });
            }

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
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(e));
        }
    }
}