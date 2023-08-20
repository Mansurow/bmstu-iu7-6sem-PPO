using IntegrationalTests.Services.AccessObject;
using Microsoft.Extensions.Logging.Abstractions;
using Portal.Common.Models;
using Portal.Common.Models.Enums;
using Portal.Services.UserService;
using Portal.Services.UserService.Exceptions;
using Xunit;

namespace IntegrationalTests.Services.InMemory;

public class UserServiceIntegrationTests: IDisposable
{
    private readonly IUserService _userService;
    private readonly AccessObjectInMemory _accessObject;

    public UserServiceIntegrationTests()
    {
        _accessObject = new AccessObjectInMemory();
        _userService = new UserService(_accessObject.UserRepository,
            NullLogger<UserService>.Instance);
    }
    
    [Fact]
    public async Task GetUsersOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        await _accessObject.InsertManyUsersAsync(users);

        var expectedCount = users.Count;

        // Act
        var actualUsers = await _userService.GetAllUsersAsync();
        var actualCount = actualUsers.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.Equal(users, actualUsers);
    }
    
    [Fact]
    public async Task GetUsersEmptyOkTest()
    {
        // Arrange
        var users = _accessObject.CreatEmptyMockUsers();
        await _accessObject.InsertManyUsersAsync(users);

        var expectedCount = users.Count;

        // Act
        var actualUsers = await _userService.GetAllUsersAsync();
        var actualCount = actualUsers.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.Equal(users, actualUsers);
    }
    
    [Fact]
    public async Task GetUsersByIdOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        await _accessObject.InsertManyUsersAsync(users);

        var expectedCount = users.Count;
        var expectedUser = users.First();

        // Act
        var actualUser = await _userService.GetUserByIdAsync(expectedUser.Id);
        var actualUsers = await _userService.GetAllUsersAsync();
        var actualCount = actualUsers.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.Equal(expectedUser, actualUser);
    }
    
    [Fact]
    public async Task GetUsersByIdNotFoundTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        await _accessObject.InsertManyUsersAsync(users);

        var expectedCount = users.Count;
        var expectedUserId = Guid.NewGuid();

        // Act
        Task<User> Action() => _userService.GetUserByIdAsync(expectedUserId);
        var actualUsers = await _userService.GetAllUsersAsync();
        var actualCount = actualUsers.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        await Assert.ThrowsAsync<UserNotFoundException>(Action);
    }
    
    [Fact]
    public async Task ChangePermissionsUserOkTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        await _accessObject.InsertManyUsersAsync(users);

        var expectedCount = users.Count;
        var expectedUser = users.First();
        expectedUser.ChangePermission(Role.Administrator);

        // Act
        await _userService.ChangeUserPermissionsAsync(expectedUser.Id, Role.Administrator);
        var actualUsers = await _userService.GetAllUsersAsync();
        var actualUser = actualUsers.First(u => u.Id == expectedUser.Id);
        var actualCount = actualUsers.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.Equal(expectedUser, actualUser);
    }
    
    [Fact]
    public async Task ChangePermissionsUserNotFoundTest()
    {
        // Arrange
        var users = _accessObject.CreateMockUsers();
        await _accessObject.InsertManyUsersAsync(users);

        var expectedCount = users.Count;
        var expectedUserId = Guid.NewGuid();

        // Act
        Task Action() => _userService.ChangeUserPermissionsAsync(expectedUserId, Role.Administrator);
        var actualUsers = await _userService.GetAllUsersAsync();
        var actualUser = actualUsers.FirstOrDefault(u => u.Id == expectedUserId);
        var actualCount = actualUsers.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.Null(actualUser);
        await Assert.ThrowsAsync<UserNotFoundException>(Action);
    }
    
    public void Dispose()
    {
        _accessObject.Dispose();
    }
}