using Portal.Common.Models;
using Portal.Common.Models.Enums;
using Portal.Database.Context;
using Portal.Database.Repositories.Interfaces;
using Portal.Database.Repositories.NpgsqlRepositories;

namespace IntegrationalTests.Services.AccessObject;

public class AccessObjectInMemory
{
    public IMenuRepository MenuRepository { get; }
    public IUserRepository UserRepository { get; }
    public IFeedbackRepository FeedbackRepository { get; }
    public IInventoryRepository InventoryRepository { get; }
    public IPackageRepository PackageRepository { get; }
    public IZoneRepository ZoneRepository { get; }
    public IBookingRepository BookingRepository { get;  }

    private readonly PortalDbContext _context;

    public AccessObjectInMemory()
    {
        var factory = new InMemoryDbContextFactory();
        _context = factory.GetDbContext();
        
        MenuRepository = new MenuRepository(_context);
        UserRepository = new UserRepository(_context);
        FeedbackRepository = new FeedbackRepository(_context);
        InventoryRepository = new InventoryRepository(_context);
        PackageRepository = new PackageRepository(_context);
        ZoneRepository = new ZoneRepository(_context);
        BookingRepository = new BookingRepository(_context);
    }
    
    public async Task InsertManyDishesAsync(List<Dish> menu)
    {
        foreach (var dish in menu)
        {
            await MenuRepository.InsertDishAsync(dish);
        }
    }
    
    public async Task InsertManyUsersAsync(List<User> users)
    {
        foreach (var user in users)
        {
            await UserRepository.InsertUserAsync(user);
        }
    }
    
    public List<User> CreatEmptyMockUsers()
    {
        return new List<User>();
    }

    public User CreateUser(Guid userId)
    {
        return new User(userId, "Иванов", "Иван", "Иванович", new DateTime(1998, 05, 17), Gender.Male, "ivanovvv@mail.ru", "+78888889");
    }

    public List<User> CreateMockUsers()
    {
        return new List<User>()
        {
            new User(Guid.NewGuid(), "Иванов", "Иван", "Иванович",  new DateTime(2002, 03, 17), Gender.Male, "ivanov@mail.ru", "+7899999999", "password12123434"),
            new User(Guid.NewGuid(), "Петров", "Петр", "Петрович",  new DateTime(2003, 05, 18), Gender.Male, "petrov@mail.ru", "+7899909999", "password12122323"),
            new User(Guid.NewGuid(), "Cударь", "Елена", "Александровна",  new DateTime(1999, 09, 18), Gender.Female, "sudar@mail.ru", "+781211111", "password12121212")
        };
    }
    
    public Dish CreateMockDish(Guid id)
    {
        return new Dish(id, "name+1", DishType.Salat, 130, "bigfoot");
    }

    public List<Dish> CreateMockMenu()
    {
        return new List<Dish>()
        {
            new Dish(Guid.NewGuid(), "Dish1", DishType.FirstCourse, 350, "description 1"),
            new Dish(Guid.NewGuid(), "Dish2", DishType.SecondCourse, 250, "description 2"),
            new Dish(Guid.NewGuid(), "Dish3", DishType.FirstCourse, 120, "description 3")
        };
    }

    public List<Dish> CreateEmptyMockMenu()
    {
        return new List<Dish>();
    }
    
    public void DateBaseCleanup()
    {
        _context.Dispose();
    }
}