using System.Net.Http.Headers;
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
using Portal.Common.Models;
using Portal.Services.InventoryServices;
using Portal.Services.OauthService;
using Portal.Services.ZoneService;
using Xunit;

namespace IntegrationalTests.Controllers;

public class InventoryControllerIntegrationalTests: IDisposable
{
    private readonly IOauthService _oauthService;
    private readonly IInventoryService _inventoryService;
    
    private PortalAccessObject _accessObject = new();

    const string PathAuth = "api/v1/oauth";
    const string PathInventory = "api/v1/inventory";
    const string DefaultToken = "token";
    const string AuthorizationHeader = "Authorization";
    const string AuthorizationScheme = "Bearer";
    const string AdminLogin = "admin";
    const string AdminPassword = "admin";

    public InventoryControllerIntegrationalTests()
    {
        _oauthService = new OauthService(_accessObject.UserRepository,
            NullLogger<OauthService>.Instance);

        _inventoryService = new InventoryService(_accessObject.InventoryRepository,
            NullLogger<InventoryService>.Instance);
    }

    [Fact]
    public async Task GetInventoriesOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZonesWithInventory();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var content = JsonContent.Create(new LoginModel(AdminLogin, AdminPassword));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", content);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.GetAsync(PathInventory);
        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualInventories = JsonConvert.DeserializeObject<List<Inventory>>(stringResponse);
        
        // Assert
        Assert.Equal(0, actualInventories?.Count);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetInventoriesUnauthorizedTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZonesWithInventory();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.GetAsync(PathInventory);

        // Assert
        Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetInventoriesForbiddenTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZonesWithInventory();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var content = JsonContent.Create(new LoginModel("user1", "password123"));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", content);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.GetAsync(PathInventory);

        // Assert
        Assert.Equal(StatusCodes.Status403Forbidden, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task WriteOffOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZonesWithInventory();

        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.PatchAsync(PathInventory + $"/{Guid.NewGuid()}", null);

        // Assert
        // TODO: поправить
        Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task WriteOffNotFoundTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZonesWithInventory();

        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.PatchAsync(PathInventory + $"/{Guid.NewGuid()}", null);

        // Assert
        // TODO: поправить
        Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task WriteOffUnauthorizedTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZonesWithInventory();

        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.PatchAsync(PathInventory + $"/{Guid.NewGuid()}", null);

        // Assert
        Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task WriteOffForbiddenTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZonesWithInventory();

        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var content = JsonContent.Create(new LoginModel("user1", "password123"));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", content);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.PatchAsync(PathInventory + $"/{Guid.NewGuid()}", null);

        // Assert
        Assert.Equal(StatusCodes.Status403Forbidden, (int)response.StatusCode);
    }
    
    private WebApplicationFactory<Program> CreateFakeWebHost()
    {
        var webHost = new WebApplicationFactory<Program>();

        return webHost.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IInventoryService>();
                services.AddScoped(_ => _inventoryService);

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