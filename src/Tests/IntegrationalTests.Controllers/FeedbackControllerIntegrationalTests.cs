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
using Portal.Services.FeedbackService;
using Portal.Services.OauthService;
using Portal.Services.ZoneService;
using Xunit;

namespace IntegrationalTests.Controllers;

public class FeedbackControllerIntegrationalTests: IDisposable
{
    private readonly IOauthService _oauthService;
    private readonly IFeedbackService _feedbackService;
    
    private PortalAccessObject _accessObject = new();

    const string PathAuth = "api/v1/oauth";
    const string PathFeedback = "api/v1/feedbacks/";
    const string DefaultToken = "token";
    const string AuthorizationHeader = "Authorization";
    const string AuthorizationScheme = "Bearer";
    const string AdminLogin = "admin";
    const string AdminPassword = "admin";

    public FeedbackControllerIntegrationalTests()
    {
        _oauthService = new OauthService(_accessObject.UserRepository,
            NullLogger<OauthService>.Instance);

        _feedbackService = new FeedbackService(_accessObject.FeedbackRepository,
            _accessObject.ZoneRepository,
            _accessObject.UserRepository,
            NullLogger<FeedbackService>.Instance);
    }

    [Fact]
    public async Task GetFeedbacksOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        var feedbacks = _accessObject.CreateMockFeedbacks(users, zones);
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyFeedbacks(feedbacks);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel(AdminLogin, AdminPassword));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.GetAsync(PathFeedback);
        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualFeedbacks = JsonConvert.DeserializeObject<List<Feedback>>(stringResponse);
        
        // Assert
        Assert.Equal(feedbacks, actualFeedbacks);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetFeedbacksUnauthorizedTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        var feedbacks = _accessObject.CreateMockFeedbacks(users, zones);
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyFeedbacks(feedbacks);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.GetAsync(PathFeedback);

        // Assert
        Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetFeedbacksForbiddenTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        var feedbacks = _accessObject.CreateMockFeedbacks(users, zones);
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyFeedbacks(feedbacks);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel("user1", "password123"));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.GetAsync(PathFeedback);

        // Assert
        Assert.Equal(StatusCodes.Status403Forbidden, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetZoneFeedbacksOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        var feedbacks = _accessObject.CreateMockFeedbacks(users, zones);
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyFeedbacks(feedbacks);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.GetAsync(PathFeedback + $"{zones.First().Id}");
        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualFeedbacks = JsonConvert.DeserializeObject<List<Feedback>>(stringResponse);
        
        // Assert
        Assert.Equal(feedbacks.FindAll(f => f.ZoneId == zones.First().Id), actualFeedbacks);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetZoneFeedbacksNotFoundTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        var feedbacks = _accessObject.CreateMockFeedbacks(users, zones);
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyFeedbacks(feedbacks);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.GetAsync(PathFeedback + $"{Guid.NewGuid()}");

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetZoneFeedbacksOkWithAuthTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        var feedbacks = _accessObject.CreateMockFeedbacks(users, zones);
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyFeedbacks(feedbacks);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel("user1", "password123"));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.GetAsync(PathFeedback + $"{zones.First().Id}");
        var stringResponse = await response.Content.ReadAsStringAsync();
        var actualFeedbacks = JsonConvert.DeserializeObject<List<Feedback>>(stringResponse);
        
        // Assert
        Assert.Equal(feedbacks.FindAll(f => f.ZoneId == zones.First().Id), actualFeedbacks);
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task AddFeedbacksOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        var feedbacks = _accessObject.CreateMockFeedbacks(users, zones);
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyFeedbacks(feedbacks);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel("user1", "password123"));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var content = JsonContent.Create(new CreateFeedbackDto(data!.UserId, zones.First().Id, 5, "Круто"));
        var response = await client.PostAsync(PathFeedback, content);

        // Assert
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task AddFeedbacksUnauthorizedTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        var feedbacks = _accessObject.CreateMockFeedbacks(users, zones);
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyFeedbacks(feedbacks);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var content = JsonContent.Create(new CreateFeedbackDto(Guid.NewGuid(), zones.First().Id, 5, "Круто"));
        var response = await client.PostAsync(PathFeedback, content);

        // Assert
        Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task AddFeedbacksBadRequestUserTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        var feedbacks = _accessObject.CreateMockFeedbacks(users, zones);
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyFeedbacks(feedbacks);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel("user1", "password123"));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var content = JsonContent.Create(new CreateFeedbackDto(Guid.NewGuid(), zones.First().Id, 5, "Круто"));
        var response = await client.PostAsync(PathFeedback, content);

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task AddFeedbacksBadRequestZoneTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        var feedbacks = _accessObject.CreateMockFeedbacks(users, zones);
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyFeedbacks(feedbacks);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel("user1", "password123"));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var content = JsonContent.Create(new CreateFeedbackDto(data!.UserId, Guid.NewGuid(), 5, "Круто"));
        var response = await client.PostAsync(PathFeedback, content);

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteFeedbackOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        var feedbacks = _accessObject.CreateMockFeedbacks(users, zones);
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyFeedbacks(feedbacks);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel("user1", "password123"));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.DeleteAsync(PathFeedback + $"{feedbacks.First().Id}");

        // Assert
        Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteFeedbackNotFoundTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        var feedbacks = _accessObject.CreateMockFeedbacks(users, zones);
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyFeedbacks(feedbacks);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        var contentAuth = JsonContent.Create(new LoginModel("user1", "password123"));
        var responseAuth = await client.PostAsync(PathAuth + "/signin", contentAuth);
        var stringResponseAuth = await responseAuth.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<AuthorizationResponse>(stringResponseAuth);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, data?.AccessToken);
        
        // Act
        var response = await client.DeleteAsync(PathFeedback + $"{Guid.NewGuid()}");

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteFeedbackUnauthorizedTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones();
        var feedbacks = _accessObject.CreateMockFeedbacks(users, zones);
        
        await _accessObject.InsertManyUsers(users);
        await _accessObject.InsertManyZones(zones);
        await _accessObject.InsertManyFeedbacks(feedbacks);
        
        var webHost = CreateFakeWebHost();
        var client = webHost.CreateClient();
        
        // Act
        var response = await client.DeleteAsync(PathFeedback + $"{feedbacks.First().Id}");

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
                services.RemoveAll<IFeedbackService>();
                services.AddScoped(_ => _feedbackService);

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