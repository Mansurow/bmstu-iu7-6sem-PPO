using System.Net.Http.Json;
using IntegrationalTests.Controllers.AccessObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Portal;
using Portal.Common.Converter;
using Portal.Common.Models;
using Portal.Common.Models.Dto;
using Portal.Common.Models.Enums;
using Portal.Services.OauthService;
using Xunit;

namespace IntegrationalTests.Controllers;

public class OauthControllerIntegrationalTests: IDisposable
{
    private readonly IOauthService _oauthService;
    
    private PortalAccessObject _accessObject = new();

    const string PathAuth = "api/v1/oauth";
    const string DefaultToken = "token";
    const string AuthorizationHeader = "Authorization";
    const string AuthorizationScheme = "Bearer";
    const string AdminLogin = "admin";
    const string AdminPassword = "admin";
    
    public OauthControllerIntegrationalTests()
    {
        _oauthService = new OauthService(_accessObject.UserRepository,
            NullLogger<OauthService>.Instance);
    }

    [Fact]
    public async Task RegistrateOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var expectedUsers = users.Select(UserConverter.ConvertAppModelToUserDto).ToList();
        await _accessObject.InsertManyUsers(users);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var content = JsonConvert.SerializeObject(new CreateUserDto("Сидоров", "Иван", "Александрович", 
            new DateOnly(2002, 08, 12), Gender.Male, "new-email@gmail.com","89990099902", "password123"));

        var httContent = JsonContent.Create(content);
        
        var response = await client.PostAsync(PathAuth + "/signup", httContent);
        var stringResponse = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponse);
        
        // Assert
        // TODO: исправить
        Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
    }

    [Fact]
    public async Task SignInOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var expectedUsers = users.Select(UserConverter.ConvertAppModelToUserDto).ToList();
        await _accessObject.InsertManyUsers(users);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var httContent = JsonContent.Create(new LoginModel("user1", "password123"));
        
        var response = await client.PostAsync(PathAuth + "/signin", httContent);
        var stringResponse = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponse);
        
        // Assert
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }

    [Fact] 
    public async Task SignInEmailIncorrectTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var expectedUsers = users.Select(UserConverter.ConvertAppModelToUserDto).ToList();
        await _accessObject.InsertManyUsers(users);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var httContent = JsonContent.Create(new LoginModel("user1111", "password123"));
        
        var response = await client.PostAsync(PathAuth + "/signin", httContent);
        var stringResponse = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponse);
        
        // Assert
        Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
    }
    
    [Fact] 
    public async Task SignInPasswordIncorrectTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var expectedUsers = users.Select(UserConverter.ConvertAppModelToUserDto).ToList();
        await _accessObject.InsertManyUsers(users);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var httContent = JsonContent.Create(new LoginModel("user1", "password123111"));
        
        var response = await client.PostAsync(PathAuth + "/signin", httContent);
        var stringResponse = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponse);
        
        // Assert
        Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
    }
    
    private WebApplicationFactory<Program> CreateFakeWebHost()
    {
        var webHost = new WebApplicationFactory<Program>();

        return webHost.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
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