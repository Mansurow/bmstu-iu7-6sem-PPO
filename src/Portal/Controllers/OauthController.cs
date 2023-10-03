using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Portal.Common.Converter;
using Portal.Common.Models;
using Portal.Common.Models.Dto;
using Portal.Services.OauthService;
using Portal.Services.OauthService.Exceptions;
using Portal.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace Portal.Controllers;

/// <summary>
/// Контроллер авторизации и аутентификация.
/// </summary>
[ApiController]
[Route("api/v1/users")]
public class OauthController : ControllerBase
{
    private readonly IOauthService _oauthService;
    private readonly ILogger<OauthController> _logger;
    private readonly IOptions<AuthorizationConfiguration> _authOptions;
    
    /// <summary>
    /// Конструктор контроллера авторизации и аутентификация.
    /// </summary>
    /// <param name="oauthService">Сервис авторизации и аутентификация.</param>
    /// <param name="logger">Инструмент логирования.</param>
    /// <param name="authOptions">Конфигурация авторизации.</param>
    public OauthController(IOauthService oauthService, ILogger<OauthController> logger, IOptions<AuthorizationConfiguration> authOptions)
    {
        _oauthService = oauthService;
        _logger = logger;
        _authOptions = authOptions;
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
    [HttpPost()]
    [SwaggerResponse(statusCode: 200, type: typeof(AuthorizationResponse), description: "Возвращает идентификатор зарегистрированного пользователя и токен JWT.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp([FromBody] CreateUserDto userDto)
    {
        try
        {
            var user = UserConverter.ConvertDtoModelToAppModel(userDto);

            await _oauthService.Registrate(user, userDto.Password);

            var token = GenerateJwt(user);
            
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
    [HttpPost("{login}&{password}")]
    [SwaggerResponse(statusCode: 200, type: typeof(AuthorizationResponse), description: "Возвращает идентификатор зарегистрированного пользователя и токен JWT.")]
    [SwaggerResponse(statusCode: 400, description: "Некорректные данные.")]
    [SwaggerResponse(statusCode: 401, description: "Пользователь неавторизован.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn([FromRoute] string login, [FromRoute] string password)
    {
        var userId = Guid.Empty;
        try
        {
            var user = await _oauthService.LogIn(login, password);
            userId = user.Id;

            var token = GenerateJwt(user);
            
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

    private string GenerateJwt(User user)
    {
        var authParams = _authOptions.Value;
        
        var securityKey = authParams.GetSymmetricSecurityKey();
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim("role", user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddSeconds(authParams.TokenLifeTime),
            signingCredentials: credentials);

        // return "Bearer " + new JwtSecurityTokenHandler().WriteToken(token);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}