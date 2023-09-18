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
using Portal.Services.MenuService;
using Portal.Services.OauthService;
using Portal.Services.UserService;
using Xunit;

namespace IntegrationalTests.Controllers;

public class MenuControllerIntegrationalTests: IDisposable
{
    private readonly IOauthService _oauthService;
    private readonly IMenuService _menuService;
    
    private PortalAccessObject _accessObject = new();

    const string PathAuth = "api/v1/oauth";
    const string PathMenu = "api/v1/menu";
    const string DefaultToken = "token";
    const string AuthorizationHeader = "Authorization";
    const string AuthorizationScheme = "Bearer";
    const string AdminLogin = "admin";
    const string AdminPassword = "admin";
    
    public MenuControllerIntegrationalTests()
    {
        _oauthService = new OauthService(_accessObject.UserRepository,
            NullLogger<OauthService>.Instance);

        _menuService = new MenuService(_accessObject.MenuRepository,
            NullLogger<MenuService>.Instance);
    }

    [Fact]
    public async Task GetMenuOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.GetAsync(PathMenu);
        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualMenu = JsonConvert.DeserializeObject<List<Dish>>(stringResponse);
        
        // Assert
        Assert.Equal(menu, actualMenu);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetMenuEmptyOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = new List<Dish>();
        
        await _accessObject.InsertManyUsers(users);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.GetAsync(PathMenu);
        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualMenu = JsonConvert.DeserializeObject<List<Dish>>(stringResponse);
        
        // Assert
        Assert.Equal(menu, actualMenu);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetMenuWithAuthOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = new List<Dish>();
        
        await _accessObject.InsertManyUsers(users);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var content = JsonContent.Create(new LoginModel(AdminLogin, AdminPassword));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", content);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.GetAsync(PathMenu);
        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualMenu = JsonConvert.DeserializeObject<List<Dish>>(stringResponse);
        
        // Assert
        Assert.Equal(menu, actualMenu);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }

    [Fact]
    public async Task GetDishOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.GetAsync(PathMenu + $"/{menu.First().Id}");
        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualDish = JsonConvert.DeserializeObject<Dish>(stringResponse);
        
        // Assert
        Assert.Equal(menu.First(), actualDish);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetDishNotFoundOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = new List<Dish>();
        
        await _accessObject.InsertManyUsers(users);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.GetAsync(PathMenu + $"/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetDishWithAuthOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var content = JsonContent.Create(new LoginModel(AdminLogin, AdminPassword));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", content);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.GetAsync(PathMenu + $"/{menu.First().Id}");
        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualDish = JsonConvert.DeserializeObject<Dish>(stringResponse);
        
        // Assert
        Assert.Equal(menu.First(), actualDish);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }

    [Fact]
    public async Task AddDishOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel(AdminLogin, AdminPassword));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var content = JsonContent.Create(new CreateDishDto("name1", DishType.Drinks, 250, "description111"));
        var response = await client.PostAsync(PathMenu, content);

        // Assert
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task AddDishUnauthorizedTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var content = JsonContent.Create(new CreateDishDto("name1", DishType.Drinks, 250, "description111"));
        var response = await client.PostAsync(PathMenu, content);

        // Assert
        Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task AddDishForbiddenTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel("user1", "password123"));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var content = JsonContent.Create(new CreateDishDto("name1", DishType.Drinks, 250, "description111"));
        var response = await client.PostAsync(PathMenu, content);

        // Assert
        Assert.Equal(StatusCodes.Status403Forbidden, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteDishOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel(AdminLogin, AdminPassword));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.DeleteAsync(PathMenu + $"/{menu.First().Id}");

        // Assert
        Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteDishNotFoundTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel(AdminLogin, AdminPassword));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.DeleteAsync(PathMenu + $"/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteDishBadRequestTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel(AdminLogin, AdminPassword));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.DeleteAsync(PathMenu + $"/{Guid.NewGuid()}1");

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteDishUnauthorizedTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.DeleteAsync(PathMenu + $"/{menu.First().Id}");

        // Assert
        Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteDishForbiddenTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var menu = _accessObject.CreateMockMenu();
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyMenu(menu);

        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel("user1", "password123"));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.DeleteAsync(PathMenu + $"/{menu.First().Id}");

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
                services.RemoveAll<IMenuService>();
                services.AddScoped(_ => _menuService);

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