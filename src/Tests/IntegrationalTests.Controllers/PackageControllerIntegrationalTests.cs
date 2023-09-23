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
using Portal.Common.Models.Enums;
using Portal.Services.InventoryServices;
using Portal.Services.MenuService;
using Portal.Services.OauthService;
using Portal.Services.PackageService;
using Xunit;

namespace IntegrationalTests.Controllers;

public class PackageControllerIntegrationalTests: IDisposable
{
    private readonly IOauthService _oauthService;
    private readonly IPackageService _packageService;
    
    private PortalAccessObject _accessObject = new();

    const string PathAuth = "api/v1/oauth";
    const string PathPackage = "api/v1/packages/";
    const string DefaultToken = "token";
    const string AuthorizationHeader = "Authorization";
    const string AuthorizationScheme = "Bearer";
    const string AdminLogin = "admin";
    const string AdminPassword = "admin";

    public PackageControllerIntegrationalTests()
    {
        _oauthService = new OauthService(_accessObject.UserRepository,
            NullLogger<OauthService>.Instance);

        _packageService = new PackageService(_accessObject.PackageRepository,
            _accessObject.MenuRepository,
            NullLogger<PackageService>.Instance);
    }

    [Fact]
    public async Task GetPackagesOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        var zones = _accessObject.CreateMockZones();
        var packages = _accessObject.CreateMockPackages();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyPackages(packages);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.GetAsync(PathPackage);
        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualPackages = JsonConvert.DeserializeObject<List<Package>>(stringResponse);
        
        // Assert
        Assert.Equal(packages.Count, actualPackages?.Count);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetPackagesEmptyTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        var zones = _accessObject.CreateMockZones();
        var packages = new List<Package>();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyPackages(packages);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.GetAsync(PathPackage);
        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualPackages = JsonConvert.DeserializeObject<List<Package>>(stringResponse);
        
        // Assert
        Assert.Equal(packages.Count, actualPackages?.Count);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetPackagesWithAuthTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        var zones = _accessObject.CreateMockZones();
        var packages = new List<Package>();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyPackages(packages);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel("user1", "password123"));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.GetAsync(PathPackage);
        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualPackages = JsonConvert.DeserializeObject<List<Package>>(stringResponse);
        
        // Assert
        Assert.Equal(packages.Count, actualPackages?.Count);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetPackageOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        var zones = _accessObject.CreateMockZones();
        var packages = _accessObject.CreateMockPackages();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyPackages(packages);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();

        // Act
        var response = await client.GetAsync(PathPackage + $"{packages.First().Id}");
        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualPackage = JsonConvert.DeserializeObject<Package>(stringResponse);
        
        // Assert
        Assert.Equal(packages.First(), actualPackage);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetPackageWithAuthTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        var zones = _accessObject.CreateMockZones();
        var packages = _accessObject.CreateMockPackages();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyPackages(packages);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel("user1", "password123"));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.GetAsync(PathPackage + $"{packages.First().Id}");
        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualPackage = JsonConvert.DeserializeObject<Package>(stringResponse);
        
        // Assert
        Assert.Equal(packages.First(), actualPackage);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetPackageNotFoundTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        var zones = _accessObject.CreateMockZones();
        var packages = _accessObject.CreateMockPackages();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyPackages(packages);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.GetAsync(PathPackage + $"{Guid.NewGuid()}");

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task AddPackageOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        var zones = _accessObject.CreateMockZones();
        var packages = _accessObject.CreateMockPackages();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyPackages(packages);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel(AdminLogin, AdminPassword));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var content = JsonContent.Create(new CreatePackageDto("name1", PackageType.Holidays, 300, 5, "description1", new List<Guid>()));
        var response = await client.PostAsync(PathPackage, content);

        // Assert
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }

    [Fact]
    public async Task AddPackageUnauthorizedTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        var zones = _accessObject.CreateMockZones();
        var packages = _accessObject.CreateMockPackages();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyPackages(packages);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var content = JsonContent.Create(new CreatePackageDto("name1", PackageType.Holidays, 300, 5, "description1", new List<Guid>()));
        var response = await client.PostAsync(PathPackage, content);

        // Assert
        Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task AddPackageForbiddenTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        var zones = _accessObject.CreateMockZones();
        var packages = _accessObject.CreateMockPackages();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyPackages(packages);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel("user1", "password123"));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var content = JsonContent.Create(new CreatePackageDto("name1", PackageType.Holidays, 300, 5, "description1", new List<Guid>()));
        var response = await client.PostAsync(PathPackage, content);

        // Assert
        Assert.Equal(StatusCodes.Status403Forbidden, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeletePackageOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        var zones = _accessObject.CreateMockZones();
        var packages = _accessObject.CreateMockPackages();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyPackages(packages);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel(AdminLogin, AdminPassword));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.DeleteAsync(PathPackage + $"{packages.First().Id}");

        // Assert
        Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeletePackageNotFoundTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        var zones = _accessObject.CreateMockZones();
        var packages = _accessObject.CreateMockPackages();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyPackages(packages);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel(AdminLogin, AdminPassword));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.DeleteAsync(PathPackage + $"{Guid.NewGuid()}");

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeletePackageUnauthorizedTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        var zones = _accessObject.CreateMockZones();
        var packages = _accessObject.CreateMockPackages();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyPackages(packages);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.DeleteAsync(PathPackage + $"{packages.First().Id}");

        // Assert
        Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeletePackageForbiddenTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        var zones = _accessObject.CreateMockZones();
        var packages = _accessObject.CreateMockPackages();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyPackages(packages);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel("user1", "password123"));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.DeleteAsync(PathPackage + $"{packages.First().Id}");

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
                services.RemoveAll<IPackageService>();
                services.AddScoped(_ => _packageService);

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