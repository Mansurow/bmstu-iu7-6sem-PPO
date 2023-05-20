using Anticafe.BL.Exceptions;
using Anticafe.BL.Models;
using Anticafe.BL.Services.MenuService;
using Anticafe.DataAccess.Converter;
using Anticafe.DataAccess.Exceptions;
using Common.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Anticafe.Controllers;

[Route("api/menu")]
[ApiController]
public class MenuController : ControllerBase
{
    private readonly IMenuService _menuService;

    private readonly ILogger<MenuController> _logger;

    public MenuController (IMenuService menuService, ILogger<MenuController> logger) 
    {
        _menuService = menuService ?? throw new ArgumentNullException(nameof(menuService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MenuDto[]))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetMenuInfo() 
    { 
        try 
        {
            var menu = await _menuService.GetAllDishesAsync();
            _logger.LogInformation("Menu information get successfully");
            return Ok(menu.Select(d => MenuConverter.ConvertAppModelToDto(d)).ToList());
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("{dishId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MenuDto))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetDish([FromRoute] int dishId)
    {
        try
        {
            var dish = await _menuService.GetDishByIdAsync(dishId);
            _logger.LogInformation("Dish information get successfully with {dishId}", dishId);
            return Ok(MenuConverter.ConvertAppModelToDto(dish));
        }
        catch (MenuNotFoundException ex) 
        {
            _logger.LogError(ex, "Dish not exist: {ex.Message}", ex.Message);
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
    public async Task<IActionResult> AddDish([FromBody] MenuDto dish)
    {
        try
        {
            await _menuService.AddDishAsync(dish.Name, dish.Type, dish.Price, dish.Description);
            _logger.LogInformation("Dish add successfully.");
            return Ok();
        }
        catch (RoomNameExistException ex)
        {
            _logger.LogError(ex, "Dish name exist: {ex.Message}", ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPut("{dishId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> UpdateDish([FromRoute] int dishId, [FromBody] MenuDto dishDto)
    {
        try
        {
            dishDto.Id = dishId;
            var dish = MenuConverter.ConvertDtoToAppModel(dishDto);
            await _menuService.UpdateDishAsync(dish);
            _logger.LogInformation("Dish successfully update.");
            return Ok();
        }
        catch (DishNotFoundException ex)
        {
            _logger.LogError(ex, "Dish not exist: {ex.Message}", ex.Message);
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete("{dishId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> DeleteDish([FromRoute] int dishId)
    {
        try
        {
            await _menuService.DeleteDishAsync(dishId);
            _logger.LogInformation("Dish successfully delete.");
            return Ok();
        }
        catch (DishNotFoundException ex)
        {
            _logger.LogError(ex, "Dish not exist: {ex.Message}", ex.Message);
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
