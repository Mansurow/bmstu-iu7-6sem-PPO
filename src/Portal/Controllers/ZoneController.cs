using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Portal.Common.Models.Dto;
using Portal.Common.Models.Enums;
using Portal.Services.ZoneService;
using Portal.Services.ZoneService.Exceptions;

namespace Portal.Controllers;

[ApiController]
[Route("api/v1/zones/")]
public class ZoneController : ControllerBase
{
    private readonly IZoneService _zoneService;
    private readonly ILogger<ZoneController> _logger;


    public ZoneController(IZoneService zoneService, ILogger<ZoneController> logger)
    {
        _zoneService = zoneService ?? throw new ArgumentNullException(nameof(zoneService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Получить все зоны
    /// </summary>
    /// <returns>Список всех пользователей</returns>
    /// <response code="200">OK. Возвращается список всех зон.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpGet]
    public async Task<IActionResult> GetZones()
    {
        try
        {
            var zones = await _zoneService.GetAllZonesAsync();

            return Ok(zones);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    
    /// <summary>
    /// Получить зону
    /// </summary>
    /// <param name="zoneId" example="f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6">Идентификатор зоны</param>
    /// <returns>Список всех пользователей</returns>
    /// <response code="200">OK. Возвращается список всех пользователей.</response>
    /// <response code="400">Bad request. Некорректные данные</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="404">NotFound. Пользователь не найден.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpGet("{zoneId:guid}")]
    public async Task<IActionResult> GetZone([FromRoute] Guid zoneId)
    {
        try
        {
            var zone = await _zoneService.GetZoneByIdAsync(zoneId);

            return Ok(zone);
        }
        catch (ZoneNotFoundException e)
        {
            _logger.LogError(e, "Zone not found");
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
    
    [HttpPost]
    [Authorize(Roles = nameof(Role.Administrator))]
    public async Task<IActionResult> PostZone([FromBody] CreateZoneDto zoneDto)
    {
        try
        {
            var zoneId = await _zoneService.AddZoneAsync(zoneDto.Name, zoneDto.Address, zoneDto.Size, zoneDto.Limit, zoneDto.Price);

            return Ok(zoneId);
        }
        catch (ZoneNotFoundException e)
        {
            _logger.LogError(e, "Zone not found");
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
    
    [HttpDelete]
    [Authorize(Roles = nameof(Role.Administrator))]
    public async Task<IActionResult> DeleteZone([FromRoute] Guid zoneId)
    {
        try
        {
            await _zoneService.RemoveZoneAsync(zoneId);

            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (ZoneNotFoundException e)
        {
            _logger.LogError(e, "Zone not found");
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