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
using Portal.Common.Models.Dto;
using Portal.Services.MenuService;
using Portal.Services.OauthService;
using Portal.Services.ZoneService;
using Xunit;

namespace IntegrationalTests.Controllers;

public class ZoneControllerIntegrationalTests: IDisposable
{
    private readonly IOauthService _oauthService;
    private readonly IZoneService _zoneService;
    
    private PortalAccessObject _accessObject = new();

    const string PathAuth = "api/v1/oauth";
    const string PathZone = "api/v1/zones";
    const string DefaultToken = "token";
    const string AuthorizationHeader = "Authorization";
    const string AuthorizationScheme = "Bearer";
    const string AdminLogin = "admin";
    const string AdminPassword = "admin";

    public ZoneControllerIntegrationalTests()
    {
        _oauthService = new OauthService(_accessObject.UserRepository,
            NullLogger<OauthService>.Instance);

        _zoneService = new ZoneService(_accessObject.ZoneRepository,
            _accessObject.InventoryRepository,
            _accessObject.PackageRepository,
            NullLogger<ZoneService>.Instance);
    }
    
    [Fact]
    public async Task GetZonesOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.GetAsync(PathZone);
        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualZones = JsonConvert.DeserializeObject<List<Zone>>(stringResponse);
        
        // Assert
        Assert.Equal(zones.Count, actualZones?.Count);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetZonesEmptyOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = new List<Zone>();
        
        await _accessObject.InsertManyUsers(users);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.GetAsync(PathZone);
        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualZones = JsonConvert.DeserializeObject<List<Zone>>(stringResponse);
        
        // Assert
        Assert.Equal(zones.Count, actualZones?.Count);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetZonesWithAuthOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        
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
        var response = await client.GetAsync(PathZone);
        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualZones = JsonConvert.DeserializeObject<List<Zone>>(stringResponse);
        
        // Assert
        Assert.Equal(zones.Count, actualZones?.Count);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetZoneOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.GetAsync(PathZone + $"/{zones.First().Id}");
        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualZone = JsonConvert.DeserializeObject<Zone>(stringResponse);
        
        // Assert
        Assert.Equal(zones.First(), actualZone);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetZoneNotFoundTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = new List<Zone>();
        
        await _accessObject.InsertManyUsers(users);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.GetAsync(PathZone + $"/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetZoneWithAuthOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        
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
        var response = await client.GetAsync(PathZone + $"/{zones.First().Id}");
        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualZone = JsonConvert.DeserializeObject<Zone>(stringResponse);
        
        // Assert
        Assert.Equal(zones.First(), actualZone);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }

    [Fact]
    public async Task AddZoneOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel(AdminLogin, AdminPassword));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var content = JsonContent.Create(new CreateZoneDto("new1", "address111", 15, 20, 
            new List<CreateInventoryDto>(), new List<Guid>()));
        var response = await client.GetAsync(PathZone + $"/{zones.First().Id}");
        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualZone = JsonConvert.DeserializeObject<Zone>(stringResponse);
        
        // Assert
        Assert.Equal(zones.First(), actualZone);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task AddZoneUnauthorizedTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var content = JsonContent.Create(new CreateZoneDto("new1", "address111", 15, 20, 
            new List<CreateInventoryDto>(), new List<Guid>()));
        var response = await client.GetAsync(PathZone + $"/{zones.First().Id}");
        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualZone = JsonConvert.DeserializeObject<Zone>(stringResponse);
        
        // Assert
        Assert.Equal(zones.First(), actualZone);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task AddZoneForbiddenTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel("user1", "password123"));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var content = JsonContent.Create(new CreateZoneDto("new1", "address111", 15, 20, 
            new List<CreateInventoryDto>(), new List<Guid>()));
        var response = await client.GetAsync(PathZone + $"/{zones.First().Id}");
        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualZone = JsonConvert.DeserializeObject<Zone>(stringResponse);
        
        // Assert
        Assert.Equal(zones.First(), actualZone);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteZoneOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel(AdminLogin, AdminPassword));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.DeleteAsync(PathZone + $"/{zones.First().Id}");

        // Assert
        Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteZoneNotFoundTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel(AdminLogin, AdminPassword));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.DeleteAsync(PathZone + $"/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteZoneUnauthorizedTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.DeleteAsync(PathZone + $"/{zones.First().Id}");

        // Assert
        Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteZoneForbiddenTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel("user1", "password123"));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.DeleteAsync(PathZone + $"/{zones.First().Id}");

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
                services.RemoveAll<IZoneService>();
                services.AddScoped(_ => _zoneService);

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