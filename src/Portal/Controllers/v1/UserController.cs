using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Portal.Common.Converter;
using Portal.Common.Dto;
using Portal.Common.Dto.User;
using Portal.Common.Enums;
using Portal.Services.BookingService;
using Portal.Services.OauthService;
using Portal.Services.OauthService.Exceptions;
using Portal.Services.UserService;
using Portal.Services.UserService.Exceptions;
using Portal.Swagger;
using Swashbuckle.AspNetCore.Annotations;
using UserDto = Portal.Common.Dto.User.User;
using BookingDto = Portal.Common.Dto.Booking.Booking;

namespace Portal.Controllers.v1;

/// <summary>
/// Контроллер пользователей.
/// </summary>
[ApiController]
[Route("api/v1/")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;
    private readonly IOauthService _oauthService;
    private readonly IBookingService _bookingService;
    
    public UserController(IUserService userService, 
        ILogger<UserController> logger, 
        IOauthService oauthService,
        IBookingService bookingService)
    {
        _userService = userService;
        _logger = logger;
        _oauthService = oauthService;
        _bookingService = bookingService;
    }
    
    /// <summary>
    /// Зарегистрировать пользователя.
    /// </summary>
    /// <param name="userDto">Данные пользователя.</param>
    /// <returns>Возвращает идентификатор зарегистрированного пользователя и токен JWT.</returns>
    /// <response code="200">OK. Возвращает идентификатор зарегистрированного пользователя и токен JWT.</response>
    /// <response code="400">Bad request. Некорректные данные</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpPost("auth")]
    [SwaggerResponse(statusCode: 200, type: typeof(AuthorizationResponse), description: "Возвращает идентификатор зарегистрированного пользователя и токен JWT.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp([FromBody] CreateUser userDto)
    {
        try
        {
            var user = UserConverter.ConvertDtoToCoreModel(userDto);

            await _oauthService.Registrate(user, userDto.Password);
            
            var token = _oauthService.GenerateJwt(user);
            
            return Ok(new AuthorizationResponse(user.Id, token));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError,new ErrorResponse(e));
        }
    }

    /// <summary>
    /// Авторизоваться.
    /// </summary>
    /// <param name="login" example="user@gmail.com">Логин.</param>
    /// <param name="password" example="password123">Пароль.</param>
    /// <returns>Возвращает идентификатор зарегистрированного пользователя и токен JWT.</returns>
    /// <response code="200">OK. Возвращает идентификатор зарегистрированного пользователя и токен JWT.</response>
    /// <response code="400">Bad request. Некорректные данные</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpGet("auth")]
    [SwaggerResponse(statusCode: 200, type: typeof(AuthorizationResponse), description: "Возвращает идентификатор зарегистрированного пользователя и токен JWT.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn([FromQuery][Required] string login, [FromQuery][Required] string password)
    {
        var userId = Guid.Empty;
        try
        {
            var user = await _oauthService.LogIn(login, password);
            userId = user.Id;
            
            var token = _oauthService.GenerateJwt(user);
            
            return Ok(new AuthorizationResponse(user.Id, token));
        }
        catch (UserLoginNotFoundException e)
        {
            _logger.LogError(e, "Invalid user login: {Login}", login);
            return Unauthorized(new
            {
                message = e.Message
            });
        }
        catch (IncorrectPasswordException e)
        {
            _logger.LogError(e, "Invalid password for user: {UserId}", userId);
            return Unauthorized(new
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
    /// Получить всех пользователей.
    /// </summary>
    /// <returns>Список всех пользователей.</returns>
    /// <response code="200">OK. Возвращается список всех пользователей.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpGet("users")]
    [Authorize(Roles = nameof(Role.Administrator))]
    [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<User>), description: "Возвращается список всех пользователей.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя недостаточно прав доступа.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();

            return Ok(users.Select(UserConverter.ConvertCoreToDtoModel));
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
    [HttpGet("users/{userId:guid}/bookings")]
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
    /// Получить пользователя.
    /// </summary>
    /// <param name="userId" example="f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6">Идентификатор пользователя.</param>
    /// <returns>Данные пользователя.</returns>
    /// <response code="200">OK. Данные пользователя.</response>
    /// <response code="400">Bad request. Некорректные данные.</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="403">Forbidden. У пользователя недостаточно прав доступа.</response>
    /// <response code="404">NotFound. Пользователь не найден.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpGet("users/{userId:guid}")]
    [Authorize]
    [SwaggerResponse(statusCode: 200, type: typeof(User), description: "Данные пользователя.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 403, description: "У пользователя недостаточно прав доступа.")]
    [SwaggerResponse(statusCode: 404, description: "Пользователь не найден.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> GetUser([FromRoute] Guid userId)
    {
        try
        {
            var responseUserId = Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var responseUserRole = User.Claims.Single(c => c.Type == ClaimTypes.Role).Value;
            
            if (responseUserId != userId && responseUserRole != Role.Administrator.ToString())
            {
                _logger.LogError("Access Denied for getting user {UserId}", userId);
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    message = $"Access Denied for getting user {userId}"
                });
            }
            
            var user = await _userService.GetUserByIdAsync(userId);
            
            return Ok(UserConverter.ConvertCoreToDtoModel(user));
        }
        catch (UserNotFoundException e)
        {
            _logger.LogError(e, "User: {UserId} not found", userId);
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(e));
        }
    }
}