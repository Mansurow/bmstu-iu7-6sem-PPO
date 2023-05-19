using Anticafe.BL.Models;
using Anticafe.BL.Sevices.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Anticafe.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly ILogger<OauthController> _logger;

        public UsersController(IUserService userService, ILogger<OauthController> logger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Authorize(Roles="Administrators")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User[]))]
        public async Task<IActionResult> GetAllUsers() 
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        public async Task<IActionResult> GetUserInfo([FromRoute] int userId)
        {
            try
            {
                var users = await _userService.GetUserByIdAsync(userId);
                _logger.LogInformation("Data successfully get for user with {userId}", userId);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
