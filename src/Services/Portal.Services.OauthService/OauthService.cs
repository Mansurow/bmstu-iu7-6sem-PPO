using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Portal.Common.Core;
using Portal.Database.Core.Repositories;
using Portal.Services.OauthService.Configuration;
using Portal.Services.OauthService.Exceptions;
using Portal.Services.UserService.Exceptions;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Portal.Services.OauthService
{
    /// <summary>
    /// Сервис авторизации
    /// </summary>
    public class OauthService: IOauthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOptions<AuthorizationConfiguration> _authOptions;
        private readonly ILogger<OauthService> _logger;

        public OauthService(IUserRepository userRepository, 
            IOptions<AuthorizationConfiguration> authOptions,
            ILogger<OauthService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
            _authOptions = authOptions;
        }

        public async Task Registrate(User user, string password) 
        {
            try
            {
                var userModel = await _userRepository.GetUserByEmailAsync(user.Email);
                
                _logger.LogError("User with login: {Login} already exists", userModel.Email);
                throw new UserLoginAlreadyExistsException($"User with login: {userModel.Email} already exists.");
            }
            catch (InvalidOperationException)
            {
                _logger.LogInformation("User with login: {Login} not found", user.Email);
                user.CreateHash(password);

                await _userRepository.InsertUserAsync(user);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error while creating user");
                throw new UserCreateException();
            }
        }

        public async Task<User> LogIn(string login, string password)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(login);
                
                if (!user.VerifyPassword(password))
                {
                    _logger.LogError("Password is incorrect for user: {UserId}", user.Id);
                    throw new IncorrectPasswordException($"Password is incorrect for user: {user.Id}");
                }

                return user;
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e, "User with login: {Login} not found", login);
                throw new UserLoginNotFoundException($"User with login: {login} not found.");
            }
        }
        
        public string GenerateJwt(User user)
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
}
