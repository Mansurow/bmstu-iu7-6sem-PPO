using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Common.Converter;
using Portal.Common.Dto;
using Portal.Common.Dto.Dish;
using Portal.Common.Enums;
using Portal.Common.Models.Dto;
using Portal.Services.MenuService;
using Portal.Services.MenuService.Exceptions;
using Swashbuckle.AspNetCore.Annotations;

namespace Portal.Controllers.v1;

/// <summary>
/// Контроллер меню блюд.
/// </summary>
[ApiController]
[Route("api/v1/menu")]
public class MenuController: ControllerBase
{
    private readonly IMenuService _menuService;
    private readonly ILogger<MenuController> _logger;

    /// <summary>
    /// Конструктор контроллера меню блюд.
    /// </summary>
    /// <param name="menuService">Сервис меню блюд.</param>
    /// <param name="logger">Инструмент логгирования.</param>
    public MenuController(IMenuService menuService, ILogger<MenuController> logger)
    {
        _menuService = menuService;
        _logger = logger;
    }

    /// <summary>
    /// Получить меню блюд.
    /// </summary>
    /// <returns>Возвращает список блюд меню.</returns>
    /// <response code="200">OK. Возвращает список блюд меню.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpGet]
    [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<Dish>), description: "Возвращает список блюд меню.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> GetMenu()
    {
        try
        {
            var menu = await _menuService.GetAllDishesAsync();

            return Ok(menu);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(e));
        }
    }
    
    /// <summary>
    /// Получить блюдо.
    /// </summary>
    /// <returns>Возвращает данные блюда.</returns>
    /// <response code="200">OK. Возвращает данные блюда.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="404">NotFound. Блюдо не найдено.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpGet("{dishId:guid}")]
    [SwaggerResponse(statusCode: 200, type: typeof(Dish), description: "Возвращает данные блюда.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 404, description: "Блюдо не найдено.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> GetDish(Guid dishId)
    {
        try
        {
            var dish = await _menuService.GetDishByIdAsync(dishId);

            return Ok(dish);
        }
        catch (DishNotFoundException e)
        {
            _logger.LogError(e, "Dish not found: {DishId}", dishId);
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
    /// Добавить блюдо.
    /// </summary>
    /// <returns>Возвращается идентификатор добавляемого блюда.</returns>
    /// <response code="200">OK. Возвращается идентификатор добавляемого блюда.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpPost]
    [Authorize(Roles = nameof(Role.Administrator))]
    [SwaggerResponse(statusCode: 200, type: typeof(IdResponse), description: "Возвращается идентификатор добавляемого блюда.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя недостаточно прав доступа.")]
    [SwaggerResponse(statusCode: 404, description: "Блюдо не найдено.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> PostDish([FromBody] CreateDish dish)
    {
        try
        {
            var dishId = await _menuService.AddDishAsync(dish.Name, dish.Type, dish.Price, dish.Description);

            return Ok(new
            {
                dishId
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(e));
        }
    }
    
    /// <summary>
    /// Обновить данные о блюде.
    /// </summary>
    /// <param name="dishDto">Данные для обновления блюда.</param>
    /// <response code="204">NoContent. Обновление успешно произошло.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="404">NotFound. Блюдо не найдено.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpPut]
    [Authorize(Roles = nameof(Role.Administrator))]
    [SwaggerResponse(statusCode: 204, description: "Обновление успешно произошло.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя недостаточно прав доступа.")]
    [SwaggerResponse(statusCode: 404, description: "Блюдо не найдено.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> PutDish([FromBody] Dish dishDto)
    {
        try
        {
            var dish = MenuConverter.ConvertDtoToCoreModel(dishDto);
            
            await _menuService.UpdateDishAsync(dish);

            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (DishNotFoundException e)
        {
            _logger.LogError(e, "Dish not found: {DishId}", dishDto.Id);
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
    /// Удалить блюдо.
    /// </summary>
    /// <param name="dishId" example="f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6">Идентификатор блюда.</param>
    /// <response code="204">NoContent. Удаление успешно произошло.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="404">NotFound. Блюдо не найдено.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [Authorize(Roles = nameof(Role.Administrator))]
    [HttpDelete("{dishId:guid}")]
    [SwaggerResponse(statusCode: 204, description: "Удаление успешно произошло.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя недостаточно прав доступа.")]
    [SwaggerResponse(statusCode: 404, description: "Блюдо не найдено.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> DeleteDish([FromRoute] Guid dishId)
    {
        try
        {
            await _menuService.RemoveDishAsync(dishId);

            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (DishNotFoundException e)
        {
            _logger.LogError(e, "Dish not found: {DishId}", dishId);
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