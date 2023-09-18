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

namespace Portal.Controllers;

/// <summary>
/// Контроллер авторизации и аутентификация
/// </summary>
[ApiController]
[Route("api/v1/oauth")]
public class OauthController : ControllerBase
{
    private readonly IOauthService _oauthService;
    private readonly ILogger<OauthController> _logger;
    private readonly IOptions<AuthorizationConfiguration> _authOptions;
    
    /// <summary>
    /// Конструктор контроллера авторизации и аутентификация
    /// </summary>
    /// <param name="oauthService">Сервис авторизации и аутентификация</param>
    /// <param name="logger">Инструмент логирования</param>
    /// <param name="authOptions">Конфигурация авторизации</param>
    /// <exception cref="ArgumentNullException">Ошибка происходит, если парметры переданы неверно</exception>
    public OauthController(IOauthService oauthService, ILogger<OauthController> logger, IOptions<AuthorizationConfiguration> authOptions)
    {
        _oauthService = oauthService ?? throw new ArgumentNullException(nameof(oauthService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _authOptions = authOptions;
    }

    /// <summary>
    /// Зарегистрировать пользователя
    /// </summary>
    /// <param name="userDto">Данные пользователя</param>
    /// <returns>Возвращает идентификатор зарегистрированного пользователя и токен JWT</returns>
    /// <response code="200">OK. Возвращает идентификатор зарегистрированного пользователя и токен JWT.</response>
    /// <response code="400">Bad request. Некорректные данные</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpPost("signup")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorizationResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp([FromBody] CreateUserDto userDto)
    {
        try
        {
            var user = UserConverter.ConvertDtoModelToAppModel(userDto);

            await _oauthService.Registrate(user, userDto.Password);

            var token = GenerateJwt(user);
            
            return Ok(new
            {
                userId = user.Id,
                accessToken = token,
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error");
            return StatusCode(StatusCodes.Status500InternalServerError,new
            {
                message = e.Message
            });
        }
    }

    /// <summary>
    /// Авторизоваться
    /// </summary>
    /// <param name="logins">Данные для авторизации</param>
    /// <returns>Возвращает идентификатор зарегистрированного пользователя и токен JWT.</returns>
    /// <response code="200">OK. Возвращает идентификатор зарегистрированного пользователя и токен JWT.</response>
    /// <response code="400">Bad request. Некорректные данные</response>
    /// <response code="401">Unauthorized. Пользователь неавторизован.</response>
    /// <response code="500">Internal server error. Ошибка на стороне сервера.</response>
    [HttpPost("signin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorizationResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn([FromBody] LoginModel logins)
    {
        var userId = Guid.Empty;
        try
        {
            var user = await _oauthService.LogIn(logins.Login, logins.Password);
            userId = user.Id;

            var token = GenerateJwt(user);
            
            return Ok(new
            {   
                userId = user.Id,
                accessToken = token
            });
        }
        catch (UserLoginNotFoundException e)
        {
            _logger.LogError(e, "Invalid user login: {Login}", logins.Login);
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
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                message = e.Message
            });
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