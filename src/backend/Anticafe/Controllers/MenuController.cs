using Anticafe.BL.Exceptions;
using Anticafe.BL.Services.MenuService;
using Anticafe.DataAccess.Converter;
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
}
