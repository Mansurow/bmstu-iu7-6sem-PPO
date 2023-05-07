using Anticafe.BL.Exceptions;
using Anticafe.BL.Models;
using Anticafe.BL.Sevices.OauthService;
using Anticafe.Common.Enums;
using Anticafe.DataAccess.Converter;
using Anticafe.DataAccess.DBModels;
using Anticafe.DataAccess.IRepositories;
using Moq;
using Xunit;

namespace UnitTests.Services;

public class OauthServiceUnitTests
{
    private readonly IOauthService _oauthService;
    private readonly Mock<IUserRepository> _mockUserRepository = new();

    public OauthServiceUnitTests()
    {
        _oauthService = new Oauthservice(_mockUserRepository.Object);
    }

    [Fact]
    public async void RegistrateTest()
    {
        // Arrange
        var login = "login";
        var password = "password";
        var users = CreateMockUsers();

        var expectedUser = new User(4, "Иванов", "Иван", "Иванович", new DateTime(2002, 03, 29), Gender.Male, login, "+92323232");
        expectedUser.CreateHash(password);

        _mockUserRepository
            .Setup(s => s.GetUserByEmailAsync(login))
            .ReturnsAsync(users.Find(e => e.Email == login));

        _mockUserRepository.Setup(s => s.InsertUserAsync(It.IsAny<UserDbModel>()))
                           .Callback((UserDbModel u) => users.Add(u));

        // Act

        await _oauthService.Registrate(expectedUser, password);
        var actualUser = UserConverter.ConvertDbModelToAppModel(users.Last());

        // Assert
        Assert.Equal(expectedUser.Id, actualUser.Id);
        Assert.Equal(expectedUser.Name, actualUser.Name);
        Assert.Equal(expectedUser.Surname, actualUser.Surname);
        Assert.Equal(expectedUser.FirstName, actualUser.FirstName);
        Assert.Equal(expectedUser.Birthday, actualUser.Birthday);
        Assert.Equal(expectedUser.Email, actualUser.Email);
        Assert.Equal(expectedUser.Gender, actualUser.Gender);
    }

    [Fact]
    public async void RegistrateEmptyTest()
    {
        // Arrange
        var login = "login";
        var password = "password";
        var users = new List<UserDbModel>();

        var expectedUser = new User(1, "Иванов", "Иван", "Иванович", new DateTime(2002, 03, 29), Gender.Male, login, "+92323232");
        expectedUser.CreateHash(password);

        _mockUserRepository
            .Setup(s => s.GetUserByEmailAsync(login))
            .ReturnsAsync(users.Find(e => e.Email == login));

        _mockUserRepository.Setup(s => s.InsertUserAsync(It.IsAny<UserDbModel>()))
                           .Callback((UserDbModel u) => users.Add(u));

        // Act

        await _oauthService.Registrate(expectedUser, password);
        var actualUser = UserConverter.ConvertDbModelToAppModel(users.Last());

        // Assert
        Assert.Equal(expectedUser.Id, actualUser.Id);
        Assert.Equal(expectedUser.Name, actualUser.Name);
        Assert.Equal(expectedUser.Surname, actualUser.Surname);
        Assert.Equal(expectedUser.FirstName, actualUser.FirstName);
        Assert.Equal(expectedUser.Birthday, actualUser.Birthday);
        Assert.Equal(expectedUser.Email, actualUser.Email);
        Assert.Equal(expectedUser.Gender, actualUser.Gender);
    }

    [Fact]
    public void RegistrateExistsLoginTest()
    {
        // Arrange
        var login = "ivanov@mail.ru";
        var password = "password";
        var users = CreateMockUsers();

        var user = new User(4, "Иванов", "Иван", "Иванович", new DateTime(2002, 03, 29), Gender.Male, login, "+92323232");
        user.CreateHash(password);

        _ = _mockUserRepository
            .Setup(s => s.GetUserByEmailAsync(login))
            .ReturnsAsync(users.Find(e => e.Email == login));

        // Act

        var action = async () => await _oauthService.Registrate(user, password);
        var actualUser = users.Last();

        // Assert
        Assert.ThrowsAsync<UserLoginAlreadyExistsException>(action);
    }

    [Fact]
    public void LogInIncorrectPasswordTest()
    {
        // Arrange
        var login = "ivanov@mail.ru";
        var password = "password123";
        var users = CreateMockUsers();
        
        var expectedUser = users[0];
        _mockUserRepository
            .Setup(s => s.GetUserByEmailAsync(login))
            .ReturnsAsync(users.Find(e => e.Email == login));

        // Act
        var action = async () => await _oauthService.LogIn(login, password);

        // Assert
        Assert.ThrowsAsync<IncorrectPasswordException>(action);
    }

    [Fact]
    public void LogInNotFoundUserTest()
    {
        // Arrange
        var login = "login";
        var password = "password";
        var users = CreateMockUsers();

        var expectedUser = users[0];
        _mockUserRepository
            .Setup(s => s.GetUserByEmailAsync(login))
            .ReturnsAsync(users.Find(e => e.Email == login));

        // Act
        var action = async () => await _oauthService.LogIn(login, password);

        // Assert
        Assert.ThrowsAsync<IncorrectPasswordException>(action);
    }

    [Fact]
    public async void LogInTest()
    {
        // Arrange
        var login = "ivanov@mail.ru";
        var password = "password";
        var users = CreateMockUsers();

        var expectedUser = UserConverter.ConvertDbModelToAppModel(users[0]);
        _mockUserRepository
            .Setup(s => s.GetUserByEmailAsync(login))
            .ReturnsAsync(users.Find(e => e.Email == login));

        // Act

        var actualUser = await _oauthService.LogIn(login, password);

        // Assert
        Assert.Equal(expectedUser.Id, actualUser.Id);
        Assert.Equal(expectedUser.Name, actualUser.Name);
        Assert.Equal(expectedUser.Surname, actualUser.Surname);
        Assert.Equal(expectedUser.FirstName, actualUser.FirstName);
        Assert.Equal(expectedUser.Birthday, actualUser.Birthday);
        Assert.Equal(expectedUser.Email, actualUser.Email);
        Assert.Equal(expectedUser.Gender, actualUser.Gender);
    }

    private List<UserDbModel> CreateMockUsers()
    {
        return new List<UserDbModel>()
        {
            new UserDbModel(1, "Иванов", "Иван", "Иванович",  new DateTime(2002, 03, 17), Gender.Male, "ivanov@mail.ru", "+7899999999", "$2a$11$4LWCwrIyxRti4BbYAj4ByudE.HdmBcjjSoO0Ih78oMUOTYP7qC.nC"),
            new UserDbModel(2, "Петров", "Петр", "Петрович",  new DateTime(2003, 05, 18), Gender.Male, "petrov@mail.ru", "+7899909999", "$2y$10$z63ss0GPoOlfU8Ag9ggnC.hUm7Mg0GCxXVx9cAmAKrH3HgsEJQSje"),
            new UserDbModel(3, "Cударь", "Елена", "Александровна",  new DateTime(1999, 09, 18), Gender.Female, "sudar@mail.ru", "+7999999999", "$2y$10$vZLZTvq26bj5cyaqOrx0TO.5As4AnyhXMSzL4Ao4qhQn3bGuntSAG")
        };
    }
}