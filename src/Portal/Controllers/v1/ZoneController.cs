using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Common.Converter;
using Portal.Common.Dto;
using Portal.Common.Dto.Zone;
using Portal.Common.Enums;
using Portal.Common.Models.Dto;
using Portal.Services.PackageService.Exceptions;
using Portal.Services.ZoneService;
using Portal.Services.ZoneService.Exceptions;
using Swashbuckle.AspNetCore.Annotations;

namespace Portal.Controllers.v1;

/// <summary>
/// Контроллер зон.
/// </summary>
[ApiController]
[Route("api/v1/zones/")]
public class ZoneController : ControllerBase
{
    private readonly IZoneService _zoneService;
    private readonly ILogger<ZoneController> _logger;
    
    /// <summary>
    ///  Конструктор контроллера зон.
    /// </summary>
    /// <param name="zoneService">Сервис зон.</param>
    /// <param name="logger">Инструмент логгирования.</param>
    public ZoneController(IZoneService zoneService, ILogger<ZoneController> logger)
    {
        _zoneService = zoneService;
        _logger = logger;
    }

    /// <summary>
    /// Получить все зоны.
    /// </summary>
    /// <returns>Список всех зон.</returns>
    /// <response code="200">OK. Возвращается список всех зон.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpGet]
    [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<Zone>), description: "Возвращается список всех зон.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
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
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(e));
        }
    }
    
    /// <summary>
    /// Получить зону.
    /// </summary>
    /// <param name="zoneId" example="f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6">Идентификатор зоны.</param>
    /// <returns>Данные зоны.</returns>
    /// <response code="200">OK. Возвращается зона/зал/комната.</response>
    /// <response code="400">Bad request. Некорректные данные</response>
    /// <response code="404">NotFound. Зона не найден.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpGet("{zoneId:guid}")]
    [SwaggerResponse(statusCode: 200, type: typeof(Zone), description: "Возвращается зона/зал/комната.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 404, description: "Зона не найден.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
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
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(e));
        }
    }

    /// <summary>
    /// Обновить зону.
    /// </summary>
    /// <param name="zoneDto">Данные для обновления зоны.</param>
    /// <response code="204">NoContent. Зона успешно обновлена.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="404">NotFound. Зона не найдена.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpPut]
    [Authorize(Roles = nameof(Role.Administrator))]
    [SwaggerResponse(statusCode: 204, description: "Зона успешно обновлена.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя недостаточно прав доступа.")]
    [SwaggerResponse(statusCode: 404, description: "Зона не найден.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> PutZone([FromBody] UpdateZone zoneDto)
    {
        try
        {
            if (zoneDto.Inventories.Any(inv => inv.ZoneId != zoneDto.Id))
            {
                _logger.LogError("One or more inventory.ZoneId is different from zoneId {ZoneId}", zoneDto.Id);
                return BadRequest(new
                {
                    message = $"One or more inventory.ZoneId is different from zoneId {zoneDto.Id}"
                });
            }

            var updateZone = ZoneConverter.ConvertDtoToCoreModel(zoneDto);
            
            await _zoneService.UpdateZoneAsync(updateZone);

            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (ZoneNotFoundException e)
        {
            _logger.LogError(e, "Zone {ZoneId} not found", zoneDto.Id);
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
    /// Добавить зону.
    /// </summary>
    /// <param name="zoneDto">Данные о зоне.</param>
    /// <returns>Идентификатор зоны.</returns>
    /// <response code="200">Ok. Идентификатор зоны.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpPost]
    [Authorize(Roles = nameof(Role.Administrator))]
    [SwaggerResponse(statusCode: 200, type: typeof(IdResponse), description: "Идентификатор зоны.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя недостаточно прав доступа.")]
    [SwaggerResponse(statusCode: 404, description: "Зона не найден.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> PostZone([FromBody] CreateZone zoneDto)
    {
        try
        {
            var zoneId = await _zoneService.AddZoneAsync(zoneDto.Name, zoneDto.Address, zoneDto.Size, zoneDto.Limit);

            await _zoneService.AddInventoryAsync(zoneId, zoneDto.Inventories);
            await _zoneService.AddPackageAsync(zoneId, zoneDto.Packages);

            return Ok(new IdResponse() { Id = zoneId });
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
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(e));
        }
    }
    
    /// <summary>
    /// Удалить зону.
    /// </summary>
    /// <param name="zoneId" example="f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6">Идентификатор зоны.</param>
    /// <response code="204">NoContent. Зона успешно удалена.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="404">NotFound. Зона не найдена.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpDelete("{zoneId:guid}")]
    [Authorize(Roles = nameof(Role.Administrator))]
    [SwaggerResponse(statusCode: 204, description: "Зона успешно удалена.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя недостаточно прав доступа.")]
    [SwaggerResponse(statusCode: 404, description: "Зона не найден.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
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
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(e));
        }
    }
}