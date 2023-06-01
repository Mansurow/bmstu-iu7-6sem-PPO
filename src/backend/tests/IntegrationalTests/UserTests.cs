using Anticafe.Common.Enums;
using Anticafe.DataAccess.DBModels;
using Anticafe.DataAccess.IRepositories;
using Anticafe.PostgreSQL;
using Anticafe.PostgreSQL.Repositories;

using Xunit;

namespace IntegrationalTests.DataAccess
{
    public class UserTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IDbContextFactory<PgSQLDbContext> _dbContextFactory;

        public UserTests()
        {
            _dbContextFactory = new InMemoryDbContextFactory();
            _userRepository = new UserRepository(_dbContextFactory);
        }

        [Fact]
        public async Task GetAllUserTest()
        {
            // Arrange
            var user = new UserDbModel(1, "1", "1", "1", DateTime.UtcNow, Gender.Male, "12", "qwqw", "asas");

            using (var dbContext = _dbContextFactory.getDbContext())
            {
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
            }
            
            // Act
            var users = await _userRepository.GetAllUsersAsync();

            // Assert
            Assert.Equal(1, users?.Count);
            // Assert.Equal(user, users?.Last());
        }
    }
}
