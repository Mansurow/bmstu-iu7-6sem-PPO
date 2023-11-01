using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Common.Dto;
using Portal.Common.Dto.Inventory;
using Portal.Common.Enums;
using Portal.Services.InventoryServices;
using Portal.Services.InventoryServices.Exceptions;
using Swashbuckle.AspNetCore.Annotations;

namespace Portal.Controllers.v1;

/// <summary>
/// Контроллер инвентаря 
/// </summary>
[ApiController]
[Route("api/v1/inventory/")]
public class InventoryController: ControllerBase
{
    private readonly IInventoryService _inventoryService;
    private readonly ILogger<InventoryController> _logger;
    
    public InventoryController(IInventoryService inventoryService, ILogger<InventoryController> logger)
    {
        _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Получить информации о инвентаре.
    /// </summary>
    /// <returns>Список инвентаря.</returns>
    /// <response code="200">Ok. Список инвентаря.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpGet]
    [Authorize(Roles = nameof(Role.Administrator))]
    [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<Inventory>), description: "Список инвентаря.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя недостаточно прав доступа.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> GetInventories()
    {
        try
        {
            var inventories = await _inventoryService.GetAllInventoriesAsync();

            return Ok(inventories);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(e));
        }
    }
    
    /// <summary>
    /// Списать инвентарь.
    /// </summary>
    /// <param name="inventoryId" example="f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6">Идентификатор инвентаря.</param>
    /// <response code="204">NotContent. Инвентарь успешно списан.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="404">NotFound. Инвентарь не найден.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpPatch("{inventoryId:guid}")]
    [Authorize(Roles = nameof(Role.Administrator))]
    [SwaggerResponse(statusCode: 204, description: "Инвентарь успешно списан.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя недостаточно прав доступа.")]
    [SwaggerResponse(statusCode: 404, description: "Инвентарь не найден.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> WriteOffInventory(Guid inventoryId)
    {
        try
        {
            var inventory = await _inventoryService.GetInventoryByIdAsync(inventoryId);
            inventory.IsWrittenOff = true;
            await _inventoryService.UpdateInventoryAsync(inventory);

            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (InventoryNotFoundException e)
        {
            _logger.LogError(e, "Inventory {InventoryId} not found", inventoryId);
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