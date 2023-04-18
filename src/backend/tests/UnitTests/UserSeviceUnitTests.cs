using Anticafe.BL.Enums;
using Anticafe.BL.Exceptions;
using Anticafe.BL.IRepositories;
using Anticafe.BL.Models;
using Anticafe.BL.Sevices.UserService;
using Moq;
using Xunit;

namespace UnitTests.Service
{
    public class UserSeviceUnitTests
    {
        private readonly IUserService _userSevice;
        private readonly Mock<IUserRepository> _mockUserRepository = new();

        public UserSeviceUnitTests()
        {
            _userSevice = new UserService(_mockUserRepository.Object);
        }

        [Fact]
        public async void GetAllUserTest() 
        {
            // Arrange
            var users = CreateMockUsers();

            _mockUserRepository.Setup(s => s.GetAllUsersAsync())
                               .ReturnsAsync(users);

            // Act
            var actualUsers = await _userSevice.GetAllUsersAsync();

            // Assert
            Assert.Equal(users, actualUsers);
        }

        [Fact]
        public async void GetAllUserEmptyTest()
        {
            // Arrange
            var users = new List<User>();

            _mockUserRepository.Setup(s => s.GetAllUsersAsync())
                               .ReturnsAsync(users);

            // Act
            var actualUsers = await _userSevice.GetAllUsersAsync();

            // Assert
            Assert.Equal(users, actualUsers);
        }

        [Fact]
        public async void GetUserByIdTest()
        {
            // Arrange
            var userId = 4;
            var expectedUser = CreateUser(userId);
            var users = CreateMockUsers();
            users.Add(expectedUser);

            _mockUserRepository.Setup(s => s.GetUserByIdAsync(userId))
                               .ReturnsAsync(users.Find(e => e.Id == userId));

            // Act
            var actualUser = await _userSevice.GetUserByIdAsync(userId);

            // Assert
            Assert.Equal(expectedUser, actualUser);
        }

        [Fact]
        public void GetUserByIdEmptyNotFoundTest()
        {
            // Arrange
            var userId = 1;
            var users = new List<User>();

            _mockUserRepository.Setup(s => s.GetUserByIdAsync(userId))
                               .ReturnsAsync(users.Find(e => e.Id == userId));

            // Act
            var action = async () => await _userSevice.GetUserByIdAsync(userId);

            // Assert
            Assert.ThrowsAsync<UserNotFoundException>(action);
        }

        [Fact]
        public void GetUserByIdNotFoundTest()
        {
            // Arrange
            var userId = 100;
            var users = CreateMockUsers();

            _mockUserRepository.Setup(s => s.GetUserByIdAsync(userId))
                               .ReturnsAsync(users.Find(e => e.Id == userId));

            // Act
            var action = async () => await _userSevice.GetUserByIdAsync(userId);

            // Assert
            Assert.ThrowsAsync<UserNotFoundException>(action);
        }

        [Fact]
        public async void ChangePermissionTest()
        {
            // Arrange
            var userId = 1;
            var users = CreateMockUsers();

            var expectedUser = users.Find(e => e.Id == userId);
            expectedUser.ChangePermission(UserRole.Admin);

            _mockUserRepository.Setup(s => s.GetUserByIdAsync(userId))
                               .ReturnsAsync(users.Find(e => e.Id == userId));

            // Act
            await _userSevice.ChangeUserPermissionsAsync(userId);
            var actualUser = users.Find(e => e.Id == userId); 

            // Assert
            Assert.Equal(expectedUser, actualUser);
        }

        [Fact]
        public void ChangePermissionNotFoundUserTest()
        {
            // Arrange
            var userId = 100;
            var users = CreateMockUsers();

            _mockUserRepository.Setup(s => s.GetUserByIdAsync(userId))
                               .ReturnsAsync(users.Find(e => e.Id == userId));

            // Act
            var action = async () => await _userSevice.ChangeUserPermissionsAsync(userId);

            // Assert
            Assert.ThrowsAsync<UserNotFoundException>(action);
        }

        [Fact]
        public void ChangePermissionEmptyUserTest()
        {
            // Arrange
            var userId = 1;
            var users = new List<User>();

            _mockUserRepository.Setup(s => s.GetUserByIdAsync(userId))
                               .ReturnsAsync(users.Find(e => e.Id == userId));

            // Act
            var action = async () => await _userSevice.ChangeUserPermissionsAsync(userId);

            // Assert
            Assert.ThrowsAsync<UserNotFoundException>(action);
        }

        private User CreateUser(int userId) 
        {
            return new User(userId, "Иванов", "Иван", "Иванович", new DateTime(1998, 05, 17), Gender.Male, "ivanovvv@mail.ru", "+78888889");
        }

        private List<User> CreateMockUsers() 
        {
            return new List<User>()
            {
                new User(1, "Иванов", "Иван", "Иванович",  new DateTime(2002, 03, 17), Gender.Male, "ivanov@mail.ru", "+7899999999"),
                new User(2, "Петров", "Петр", "Петрович",  new DateTime(2003, 05, 18), Gender.Male, "petrov@mail.ru", "+7899909999"),
                new User(3, "Cударь", "Елена", "Александровна",  new DateTime(1999, 09, 18), Gender.Female, "sudar@mail.ru", "+781211111")
            };
        }
    }
}
