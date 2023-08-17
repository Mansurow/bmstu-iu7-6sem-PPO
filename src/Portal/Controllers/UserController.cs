using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Common.Converter;
using Portal.Common.Models.Dto;
using Portal.Common.Models.Enums;
using Portal.Services.UserService;
using Portal.Services.UserService.Exceptions;

namespace Portal.Controllers;

/// <summary>
/// Контроллер пользователей
/// </summary>
[ApiController]
[Route("api/v1/users/")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Получить всех пользователей
    /// </summary>
    /// <returns>Список всех пользователей</returns>
    /// <response code="200">OK. Возвращается список всех пользователей.</response>
    /// <response code="400">Bad request. Некорректные данные</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = nameof(Role.Administrator))]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();

            return Ok(users.Select(UserConverter.ConvertAppModelToUserDto));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    
    /// <summary>
    /// Получить пользователя
    /// </summary>
    /// <param name="userId" example="f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6">Идентификатор пользователя</param>
    /// <returns>Список всех пользователей</returns>
    /// <response code="200">OK. Возвращается список всех пользователей.</response>
    /// <response code="400">Bad request. Некорректные данные</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetUser([FromRoute] Guid userId)
    {
        try
        {
            var responseUserId = Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var responseUserRole = User.Claims.Single(c => c.Type == ClaimTypes.Role).Value;
            
            if (responseUserId != userId && responseUserRole != Role.Administrator.ToString())
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            
            var user = await _userService.GetUserByIdAsync(userId);
            
            return Ok(UserConverter.ConvertAppModelToUserDto(user));
        }
        catch (UserNotFoundException e)
        {
            _logger.LogError(e, "User: {UserId} not found", userId);
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}