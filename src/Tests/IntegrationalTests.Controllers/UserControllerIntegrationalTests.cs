using System.Net.Http.Headers;
using IntegrationalTests.Controllers.AccessObject;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Abstractions;
using Portal.Services.UserService;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Portal;
using Portal.Common.Converter;
using Portal.Common.Dto;
using Portal.Common.Dto.User;
using Portal.Services.OauthService;

namespace IntegrationalTests.Controllers;

public class UserControllerIntegrationalTests: IDisposable
{
    private readonly IUserService _userService;
    private readonly IOauthService _oauthService;

    private readonly PortalAccessObject _accessObject = new();

    const string PathAuth = "api/v1/users/";
    const string PathUser = "api/v1/users";
    const string DefaultToken = "token";
    const string AuthorizationHeader = "Authorization";
    const string AuthorizationScheme = "Bearer";
    const string AdminLogin = "admin";
    const string AdminPassword = "admin";
    
    public UserControllerIntegrationalTests()
    {
        _userService = new UserService(_accessObject.UserRepository,
            NullLogger<UserService>.Instance);
        _oauthService = new OauthService(_accessObject.UserRepository,
            NullLogger<OauthService>.Instance);
    }

    [Fact]
    public async Task GetAllUsersUnauthorizedTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();

        await _accessObject.InsertManyUsers(users);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.GetAsync(PathUser);

        // Assert
        Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetAllUsersForbiddenTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();

        await _accessObject.InsertManyUsers(users);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var responseAuth = await client.PostAsync(PathAuth + "user1&password123", null);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.GetAsync(PathUser);

        // Assert
        Assert.Equal(StatusCodes.Status403Forbidden, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetAllUsersOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var expectedUsers = users.Select(UserConverter.ConvertCoreToDtoModel).ToList();
        await _accessObject.InsertManyUsers(users);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var responseAuth = await client.PostAsync(PathAuth + $"{AdminLogin}&{AdminPassword}", null);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.GetAsync(PathUser);

        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualUsers = JsonConvert.DeserializeObject<List<User>>(stringResponse);

        // Assert
        // Assert.Equal(expectedUsers, actualUsers);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }

    [Fact]
    public async Task GetUserUnauthorizedOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var expectedUsers = users.Select(UserConverter.ConvertCoreToDtoModel).ToList();
        await _accessObject.InsertManyUsers(users);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.GetAsync(PathUser + $"/{users.First().Id}");

        // Assert
        Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetUserForbiddenTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var expectedUsers = users.Select(UserConverter.ConvertCoreToDtoModel).ToList();
        await _accessObject.InsertManyUsers(users);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var responseAuth = await client.PostAsync(PathAuth + "user1&password123", null);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.GetAsync(PathUser + $"/{users[0].Id}");
        
        // Assert
        Assert.Equal(StatusCodes.Status403Forbidden, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetUserOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var expectedUsers = users.Select(UserConverter.ConvertCoreToDtoModel).ToList();
        await _accessObject.InsertManyUsers(users);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var responseAuth = await client.PostAsync(PathAuth + "user1&password123", null);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.GetAsync(PathUser + $"/{data?.UserId}");

        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualUser = JsonConvert.DeserializeObject<User>(stringResponse);

        // Assert
        // Assert.Equal(expectedUsers[1], actualUser);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetUserAdminOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var expectedUsers = users.Select(UserConverter.ConvertCoreToDtoModel).ToList();
        await _accessObject.InsertManyUsers(users);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var responseAuth = await client.PostAsync(PathAuth + $"{AdminLogin}&{AdminPassword}", null);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.GetAsync(PathUser + $"/{users[1].Id}");

        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualUser = JsonConvert.DeserializeObject<User>(stringResponse);

        // Assert
        // Assert.Equal(expectedUsers[1], actualUser);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetUserNotFoundTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var expectedUsers = users.Select(UserConverter.ConvertCoreToDtoModel).ToList();
        await _accessObject.InsertManyUsers(users);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var responseAuth = await client.PostAsync(PathAuth + $"{AdminLogin}&{AdminPassword}", null);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.GetAsync(PathUser + $"/{Guid.NewGuid()}");
        
        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
    }
    
    private WebApplicationFactory<Program> CreateFakeWebHost()
    {
        var webHost = new WebApplicationFactory<Program>();

        return webHost.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IUserService>();
                services.AddScoped(_ => _userService);
                
                services.RemoveAll<IOauthService>();
                services.AddScoped(_ => _oauthService);
            });
        });
    }
    
    public void Dispose()
    {
        _accessObject.Dispose();
    }
}
