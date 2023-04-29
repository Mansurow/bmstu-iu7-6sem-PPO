using Anticafe.BL.Exceptions;
using Anticafe.BL.Models;
using Anticafe.BL.Sevices.UserService;
using Anticafe.Common.Enums;
using Anticafe.DataAccess.Converter;
using Anticafe.DataAccess.DBModels;
using Anticafe.DataAccess.IRepositories;
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
            var getUsers = await _userSevice.GetAllUsersAsync();
            var actualUsers = getUsers.Select(u => UserConverter.ConvertAppModelToDbModel(u)).ToList();

            // Assert
            Assert.Equal(users.Count, actualUsers.Count);
        }

        [Fact]
        public async void GetAllUserEmptyTest()
        {
            // Arrange
            var users = new List<UserDbModel>();

            _mockUserRepository.Setup(s => s.GetAllUsersAsync())
                               .ReturnsAsync(users);

            // Act
            var getUsers = await _userSevice.GetAllUsersAsync();
            var actualUsers = getUsers.Select(u => UserConverter.ConvertAppModelToDbModel(u)).ToList();

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
            users.Add(UserConverter.ConvertAppModelToDbModel(expectedUser));

            _mockUserRepository.Setup(s => s.GetUserByIdAsync(userId))
                               .ReturnsAsync(users.Find(e => e.Id == userId));

            // Act
            var actualUser = await _userSevice.GetUserByIdAsync(userId);

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
        public void GetUserByIdEmptyNotFoundTest()
        {
            // Arrange
            var userId = 1;
            var users = new List<UserDbModel>();

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

            var expectedUser = UserConverter.ConvertDbModelToAppModel(users.Find(e => e.Id == userId));
            expectedUser.ChangePermission(UserRole.Admin);

            _mockUserRepository.Setup(s => s.GetUserByIdAsync(userId))
                               .ReturnsAsync(users.Find(e => e.Id == userId));

            // Act
            await _userSevice.ChangeUserPermissionsAsync(userId);
            var actualUser = UserConverter.ConvertDbModelToAppModel(users.Find(e => e.Id == userId));

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
            var users = new List<UserDbModel>();

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

        private List<UserDbModel> CreateMockUsers() 
        {
            return new List<UserDbModel>()
            {
                new UserDbModel(1, "Иванов", "Иван", "Иванович",  new DateTime(2002, 03, 17), Gender.Male, "ivanov@mail.ru", "+7899999999", "password12123434"),
                new UserDbModel(2, "Петров", "Петр", "Петрович",  new DateTime(2003, 05, 18), Gender.Male, "petrov@mail.ru", "+7899909999", "password12122323"),
                new UserDbModel(3, "Cударь", "Елена", "Александровна",  new DateTime(1999, 09, 18), Gender.Female, "sudar@mail.ru", "+781211111", "password12121212")
            };
        }
    }
}
