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
    
    public async Task InsertManyZonesAsync(List<Zone> zones)
    {
        foreach (var zone in zones)
        {
            await ZoneRepository.InsertZoneAsync(zone);
        }
    }
    
    public async Task InsertManyPackagesAsync(List<Package> packages)
    {
        foreach (var package in packages)
        {
            await PackageRepository.InsertPackageAsync(package);
        }
    }
    
    public async Task InsertManyBookingsAsync(List<Booking> bookings)
    {
        foreach (var booking in bookings)
        {
            await BookingRepository.InsertBookingAsync(booking);
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
    
    public List<Package> CreateMockPackages()
    {
        return new List<Package>()
        {
            new Package(Guid.NewGuid(), "Почасовая аренда", PackageType.Usual, 350, 2,
                "Почасовая стоимость аренды зала для компании людей"),
            new Package(Guid.NewGuid(), "Пакет \"Для своих\"", PackageType.Simple, 999, 3,
                "Почасовая стоимость аренды зала для компании людей")
        };
    }

    public List<Zone> CreateMockZones(List<Package> packages)
    {
        return new List<Zone>
        {
            new Zone(Guid.NewGuid(), "Zone1", "address1", 10, 10, 250, 0.0),
            new Zone(Guid.NewGuid(), "Zone2", "address2", 30, 10, 350, 0.0),
            new Zone(Guid.NewGuid(), "Zone3", "address3", 25, 10, 300, 0.0),
            new Zone(Guid.NewGuid(), "Zone3", "address3", 25, 10, 300, 0.0)
        };
    }

    public List<Booking> CreateEmptyMockBookings()
    {
        return new List<Booking>();
    }
    
    public List<Booking> CreateMockBookings(List<User> users, List<Zone> zones, List<Package> packages)
    {
        return new List<Booking>()
        {
            new Booking(Guid.NewGuid(), zones[0].Id, users[0].Id, packages[0].Id,
                10, BookingStatus.Reserved,
                DateOnly.FromDateTime(DateTime.Today),
                new TimeOnly(8, 00), 
                new TimeOnly(12, 00)),
            new Booking(Guid.NewGuid(), zones[2].Id, users[1].Id, packages[1].Id, 
                10, BookingStatus.Reserved, 
                DateOnly.FromDateTime(DateTime.UtcNow + new TimeSpan(1, 0, 0, 0)),
                new TimeOnly(13, 00, 00), 
                new TimeOnly(15, 00, 00)),
            new Booking(Guid.NewGuid(), zones[2].Id, users[1].Id, packages[1].Id, 
                10, BookingStatus.TemporaryReserved, 
                DateOnly.FromDateTime(DateTime.UtcNow + new TimeSpan(5, 0, 0, 0)),
                new TimeOnly(13, 00, 00), 
                new TimeOnly(15, 00, 00)),
            new Booking(Guid.NewGuid(), zones[0].Id, users[2].Id, packages[0].Id, 
                10, BookingStatus.NoActual,
                DateOnly.Parse("2023.05.20"),
                new TimeOnly(12, 00), 
                new TimeOnly(18, 00)),
            new Booking(Guid.NewGuid(), zones[0].Id, users[0].Id, packages[0].Id, 
                10, BookingStatus.Reserved, 
                DateOnly.FromDateTime(DateTime.UtcNow  + new TimeSpan(2, 0, 0, 0)),
                new TimeOnly(12, 00), 
                new TimeOnly(18, 00)),
            new Booking(Guid.NewGuid(), zones[0].Id, users[0].Id, packages[0].Id, 
                10, BookingStatus.Cancelled, 
                DateOnly.FromDateTime(DateTime.UtcNow  + new TimeSpan(6, 0, 0, 0)),
                new TimeOnly(12, 00), 
                new TimeOnly(18, 00)),
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