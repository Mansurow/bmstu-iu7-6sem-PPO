using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Portal.Common.Core;
using Portal.Common.Enums;
using Xunit;
using Portal.Services.UserService;
using Portal.Services.UserService.Exceptions;
using Portal.Database.Core.Repositories;

namespace UnitTests.Services;

public class UserServiceUnitTests
{
    private readonly IUserService _userService;
    private readonly Mock<IUserRepository> _mockUserRepository = new();

    public UserServiceUnitTests()
    {
        _userService = new UserService(_mockUserRepository.Object,
            NullLogger<UserService>.Instance);
    }

    [Fact]
    public async Task GetAllUserTest()
    {
        // Arrange
        var users = CreateMockUsers();

        _mockUserRepository.Setup(s => s.GetAllUsersAsync())
                           .ReturnsAsync(users);

        // Act
        var actualUsers = await _userService.GetAllUsersAsync();

        // Assert
        Assert.Equal(users.Count, actualUsers.Count);
    }

    [Fact]
    public async Task GetAllUserEmptyTest()
    {
        // Arrange
        var users = CreatEmptyMockUser();

        _mockUserRepository.Setup(s => s.GetAllUsersAsync())
                           .ReturnsAsync(users);

        // Act
        var actualUsers = await _userService.GetAllUsersAsync();

        // Assert
        Assert.Equal(users, actualUsers);
    }

    [Fact]
    public async Task GetUserByIdTest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedUser = CreateUser(userId);
        var users = CreateMockUsers();
        users.Add(expectedUser);

        _mockUserRepository.Setup(s => s.GetUserByIdAsync(userId))
                           .ReturnsAsync(users.Find(e => e.Id == userId)!);

        // Act
        var actualUser = await _userService.GetUserByIdAsync(userId);

        // Assert
        Assert.Equal(expectedUser, actualUser);
    }

    [Fact]
    public async Task GetUserByIdEmptyNotFoundTest()
    {
        // Arrange
        var users = CreateMockUsers();
        var userId = Guid.NewGuid();
        // var users = CreatEmptyMockUser();

        _mockUserRepository.Setup(s => s.GetUserByIdAsync(userId))
            .ThrowsAsync(new InvalidOperationException());

        // Act
        async Task<User> Action() => await _userService.GetUserByIdAsync(userId);

        // Assert
        await Assert.ThrowsAsync<UserNotFoundException>(Action);
    }

    [Fact]
    public async Task GetUserByIdNotFoundTest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var users = CreateMockUsers();

        _mockUserRepository.Setup(s => s.GetUserByIdAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new InvalidOperationException());

        // Act
        Task<User> Action() => _userService.GetUserByIdAsync(userId);

        // Assert
        await Assert.ThrowsAsync<UserNotFoundException>(Action);
    }

    [Fact]
    public async Task ChangePermissionTest()
    {
        // Arrange
        var users = CreateMockUsers();
        var userId = users[0].Id;

        var expectedUser = users.Find(e => e.Id == userId);
        expectedUser?.ChangePermission(Role.Administrator);

        _mockUserRepository.Setup(s => s.GetUserByIdAsync(userId))
                           .ReturnsAsync(users.Find(e => e.Id == userId)!);

        // Act
        await _userService.ChangeUserPermissionsAsync(userId, Role.Administrator);
        var actualUser = users.Find(e => e.Id == userId);

        // Assert
        Assert.Equal(expectedUser, actualUser);
    }

    [Fact]
    public async Task ChangePermissionNotFoundUserTest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var users = CreateMockUsers();

        _mockUserRepository.Setup(s => s.GetUserByIdAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new InvalidOperationException());

        // Act
        var action = async () => await _userService.ChangeUserPermissionsAsync(userId, Role.Administrator);

        // Assert
        await Assert.ThrowsAsync<UserNotFoundException>(action);
    }

    [Fact]
    public async Task ChangePermissionEmptyUserTest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var users = CreateMockUsers();

        _mockUserRepository.Setup(s => s.GetUserByIdAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new InvalidOperationException());

        // Act
        async Task Action() => await _userService.ChangeUserPermissionsAsync(userId, Role.Administrator);

        // Assert
        await Assert.ThrowsAsync<UserNotFoundException>(Action);
    }

    private List<User> CreatEmptyMockUser()
    {
        return new List<User>();
    }

    private User CreateUser(Guid userId)
    {
        return new User(userId, "Иванов", "Иван", "Иванович", new DateOnly(1998, 05, 17), Gender.Male, "ivanovvv@mail.ru", "+78888889");
    }

    private List<User> CreateMockUsers()
    {
        return new List<User>()
        {
            new User(Guid.NewGuid(), "Иванов", "Иван", "Иванович",  new DateOnly(2002, 03, 17), Gender.Male, "ivanov@mail.ru", "+7899999999", "password12123434"),
            new User(Guid.NewGuid(), "Петров", "Петр", "Петрович",  new DateOnly(2003, 05, 18), Gender.Male, "petrov@mail.ru", "+7899909999", "password12122323"),
            new User(Guid.NewGuid(), "Cударь", "Елена", "Александровна",  new DateOnly(1999, 09, 18), Gender.Female, "sudar@mail.ru", "+781211111", "password12121212")
        };
    }
}
