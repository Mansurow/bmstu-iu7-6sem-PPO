using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Common.Core;
using Portal.Common.Dto;
using Portal.Services.BookingService;
using Portal.Services.ZoneService.Exceptions;
using Swashbuckle.AspNetCore.Annotations;

namespace Portal.Controllers.v1;

[ApiController]
[Route("api/v1/freetime")]
public class FreeTimeController: ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<FreeTimeController> _logger;
    
    public FreeTimeController(IBookingService bookingService, ILogger<FreeTimeController> logegr)
    {
        _bookingService = bookingService;
        _logger = logegr;
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
    [HttpGet]
    [Authorize]
    [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<FreeTime>), description: "Список свободного времени для бронирования зоны.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя недостаточно прав доступа.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> GetFreeTime([FromQuery][Required] Guid zoneId, [FromQuery] string? date)
    {
        try
        {
            date ??= DateTime.Today.ToString();

            var freeTime = await _bookingService.GetFreeTimeAsync(zoneId, DateOnly.Parse(date!));

            return Ok(freeTime);
        }
        catch (ZoneNotFoundException e)
        {
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