using Anticafe.BL.Exceptions;
using Anticafe.BL.Models;
using Anticafe.BL.Sevices.OauthService;
using Anticafe.BL.Sevices.UserService;
using Anticafe.DataAccess.Converter;
using Common.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Anticafe.Controllers;

[ApiController]
[Route("api/oauth")]
public class OauthController: ControllerBase
{
    private readonly IOauthService _oauthService;
    private readonly IUserService _userService;

    private readonly ILogger<OauthController> _logger;

    public OauthController(IOauthService oauthService, 
                           IUserService userService, 
                           ILogger<OauthController> logger)
    {
        _oauthService = oauthService ?? throw new ArgumentNullException(nameof(oauthService));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("registrate/{login}&{password}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Registrate([FromRoute] string login, [FromRoute] string password) 
    {
        if (login.Length < 3 || password.Length < 8)
        {
            _logger.LogError("Login or password not corrent");
            return BadRequest("Login or password not corrent");
        }

        var userId = 0;
        try
        {
            var user = await _oauthService.Registrate(login, password);
            userId = user.Id;
            _logger.LogInformation("User registrate successfully. UserId: {userId}", userId);
            return Ok(UserConverter.ConvertAppModelToDto(user));
        }
        catch (UserLoginAlreadyExistsException ex)
        {
            _logger.LogError(ex, "{ex.Message}", ex.Message);
            return Unauthorized();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Internal server error | UserId: {userId}", userId);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

    }

    [HttpPost("login/{login}&{password}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Authorization([FromRoute] string login, [FromRoute] string password) 
    {
        if (login.Length < 3 || password.Length < 8) 
        {
            _logger.LogError("Login or password not corrent");
            return BadRequest("Login or password not corrent");
        }

        var userId = 0;
        try
        {
            var user = await _oauthService.LogIn(login, password);
            _logger.LogInformation("User login successfully. UserId: {userId}", userId);
            return Ok(UserConverter.ConvertAppModelToDto(user));
        }
        catch (UserLoginNotFoundException ex)
        {
            _logger.LogError(ex, "{ex.Message}", ex.Message);
            return Unauthorized();
        }
        catch (IncorrectPasswordException ex)
        {
            _logger.LogError(ex, "{ex.Message}", ex.Message);
            return Unauthorized();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Internal server error | UserId: {userId}", userId);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
