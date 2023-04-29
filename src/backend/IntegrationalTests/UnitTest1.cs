using Anticafe.BL.Enums;
using Anticafe.BL.IRepositories;
using DataAccess;
using DataAccess.DBModels;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IntegrationalTests
{
    public class UnitTest1
    {
        private readonly IUserRepository _userRepository;

        public UnitTest1(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Fact]
        public void Test1()
        {
            var optionsBuilder = new AppDbContext();
            var db = optionsBuilder;
            var menu = new MenuDbModel(100, "1111", DishType.Drinks, 250.0, "sasasaas");

            // добавляем их в бд
            db.Menu.AddRange(menu);
            db.SaveChanges();

            var getmenu = db.Menu;
            Assert.NotNull(getmenu);

        }
    }
}