using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Common.Models;
using Portal.Common.Models.Dto;
using Portal.Common.Models.Enums;
using Portal.Services.PackageService.Exceptions;
using Portal.Services.ZoneService;
using Portal.Services.ZoneService.Exceptions;

namespace Portal.Controllers;

/// <summary>
/// Контроллер зон
/// </summary>
[ApiController]
[Route("api/v1/zones/")]
public class ZoneController : ControllerBase
{
    private readonly IZoneService _zoneService;
    private readonly ILogger<ZoneController> _logger;
    
    /// <summary>
    ///  Конструктор контроллера зон
    /// </summary>
    /// <param name="zoneService">Сервис зон</param>
    /// <param name="logger">Инструмент логгирования</param>
    /// <exception cref="ArgumentNullException">Ошибка происходит, если парметры переданы неверно</exception>
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Zone>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
    /// <response code="200">OK. Возвращается зона/зал/комната.</response>
    /// <response code="400">Bad request. Некорректные данные</response>
    /// <response code="404">NotFound. Пользователь не найден.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpGet("{zoneId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Zone))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Обновить зон
    /// </summary>
    /// <param name="zone">Данные для обновления зоны</param>
    /// <response code="204">NoContent. Зоны успешно обновлена.</response>
    /// <response code="400">Bad request. Некорректные данные</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="404">NotFound. Зона не найдена.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpPut]
    [Authorize(Roles = nameof(Role.Administrator))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutZone([FromBody] Zone zone)
    {
        try
        {
            // TODO: пофиксить
            await _zoneService.UpdateZoneAsync(zone);

            return StatusCode(StatusCodes.Status204NoContent);
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
    
    /// <summary>
    /// Добавить зон
    /// </summary>
    /// <param name="zoneDto">Данные о зоне</param>
    /// <returns>Идентификатор зоны</returns>
    /// <response code="200">Ok. Идентификатор зоны.</response>
    /// <response code="400">Bad request. Некорректные данные</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpPost]
    [Authorize(Roles = nameof(Role.Administrator))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostZone([FromBody] CreateZoneDto zoneDto)
    {
        try
        {
            var zoneId = await _zoneService.AddZoneAsync(zoneDto.Name, zoneDto.Address, zoneDto.Size, zoneDto.Limit,
                zoneDto.Price);

            await _zoneService.AddInventoryAsync(zoneId, zoneDto.Inventories);
            await _zoneService.AddPackageAsync(zoneId, zoneDto.Packages);

            return Ok(new { zoneId });
        }
        catch (PackageNotFoundException e)
        {
            _logger.LogError(e, "One or more packages id not found");
            return BadRequest(new
            {
                message = e.Message
            });
        }
        catch (ZonePackageExistsException e)
        {
            _logger.LogError(e, "One or more packages already include in zone");
            return BadRequest(new
            {
                message = e.Message
            });
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
    
    /// <summary>
    /// Удалить зону
    /// </summary>
    /// <param name="zoneId" example="f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6">Идентификатор зоны</param>
    /// <response code="204">NoContent. Зоны успешно обновлена.</response>
    /// <response code="400">Bad request. Некорректные данные</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="404">NotFound. Зона не найдена.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpDelete("{zoneId:guid}")]
    [Authorize(Roles = nameof(Role.Administrator))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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