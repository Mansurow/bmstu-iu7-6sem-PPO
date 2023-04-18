using Anticafe.BL.Enums;
using Anticafe.BL.Exceptions;
using Anticafe.BL.IRepositories;
using Anticafe.BL.Models;
using Anticafe.BL.Sevices.OauthService;
using Moq;
using Xunit;

namespace UnitTests.Service
{
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

            var user = new User(4, "Иванов", "Иван", "Иванович", new DateTime(2002, 03, 29), Gender.Male, login, "+92323232");
            user.CreateHash(password);

            _mockUserRepository
                .Setup(s => s.GetUserByEmailAsync(login))
                .ReturnsAsync(users.Find(e => e.Email == login));

            _mockUserRepository.Setup(s => s.AddUserAsync(It.IsAny<User>()))
                               .Callback((User u) => users.Add(u));

            // Act

            await _oauthService.Registrate(user, password);
            var actualUser = users.Last();

            // Assert
            Assert.Equal(user, actualUser);
        }

        [Fact]
        public async void RegistrateEmptyTest()
        {
            // Arrange
            var login = "login";
            var password = "password";
            var users = new List<User>();

            var user = new User(1, "Иванов", "Иван", "Иванович", new DateTime(2002, 03, 29), Gender.Male, login, "+92323232");
            user.CreateHash(password);

            _mockUserRepository
                .Setup(s => s.GetUserByEmailAsync(login))
                .ReturnsAsync(users.Find(e => e.Email == login));

            _mockUserRepository.Setup(s => s.AddUserAsync(It.IsAny<User>()))
                               .Callback((User u) => users.Add(u));

            // Act

            await _oauthService.Registrate(user, password);
            var actualUser = users.Last();

            // Assert
            Assert.Equal(user, actualUser);
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

            _mockUserRepository
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

            var expectedUser = users[0];
            _mockUserRepository
                .Setup(s => s.GetUserByEmailAsync(login))
                .ReturnsAsync(users.Find(e => e.Email == login));

            // Act

            var actualUser = await _oauthService.LogIn(login, password);

            // Assert
            Assert.Equal(expectedUser, actualUser);
        }

        private List<User> CreateMockUsers()
        {
            return new List<User>()
            {
                new User(1, "Иванов", "Иван", "Иванович",  new DateTime(2002, 03, 17), Gender.Male, "ivanov@mail.ru", "+7899999999", "$2a$11$4LWCwrIyxRti4BbYAj4ByudE.HdmBcjjSoO0Ih78oMUOTYP7qC.nC"),
                new User(2, "Петров", "Петр", "Петрович",  new DateTime(2003, 05, 18), Gender.Male, "petrov@mail.ru", "+7899909999", "$2y$10$z63ss0GPoOlfU8Ag9ggnC.hUm7Mg0GCxXVx9cAmAKrH3HgsEJQSje"),
                new User(3, "Cударь", "Елена", "Александровна",  new DateTime(1999, 09, 18), Gender.Female, "sudar@mail.ru", "$2y$10$vZLZTvq26bj5cyaqOrx0TO.5As4AnyhXMSzL4Ao4qhQn3bGuntSAG")
            };
        }
    }
}