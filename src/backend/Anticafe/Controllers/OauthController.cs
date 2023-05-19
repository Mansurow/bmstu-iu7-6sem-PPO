using Anticafe.BL.Models;
using Anticafe.BL.Sevices.OauthService;
using Anticafe.BL.Sevices.UserService;
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

    /// <summary>
    /// Method for registrate user
    /// </summary>
    /// <returns>Created session Id (uint)</returns>
    /// <response code="200">User Data DTO.</response>
    /// <response code="500">Internal server error. Smth gone wrong.</response>

    [HttpPost("registrate")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Registrate(string login, string password) 
    {
        var userId = 0;
        try
        {
            var user = await _oauthService.Registrate(login, password);
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Internal server error | UserId: {userId}", userId);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Authorization(string login, string password) 
    {
        var userId = 0;
        try
        {
            var user = await _oauthService.LogIn(login, password);
            // var respinse = Converter....
            return Ok(user); // Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Internal server error | UserId: {userId}", userId);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
