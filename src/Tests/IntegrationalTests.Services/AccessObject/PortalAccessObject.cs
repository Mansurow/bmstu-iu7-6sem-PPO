using Microsoft.Extensions.Options;
using Portal.Common.Core;
using Portal.Common.Enums;
using Portal.Database.Context;
using Portal.Database.Core.Repositories;
using Portal.Database.Repositories.NpgsqlRepositories;
using Portal.Services.BookingService.Configuration;

namespace IntegrationalTests.Services.AccessObject;

public class PortalAccessObject: IDisposable
{
    private PortalDbContext Context { get; set; }
    protected internal IMenuRepository MenuRepository { get; private set; }
    protected internal IUserRepository UserRepository { get; private set; }
    protected internal IFeedbackRepository FeedbackRepository { get; set; }
    protected internal IInventoryRepository InventoryRepository { get; private set; }
    protected internal IPackageRepository PackageRepository { get; private set; }
    protected internal IZoneRepository ZoneRepository { get; private set; }
    protected internal IBookingRepository BookingRepository { get; private set; }

    protected internal IOptions<BookingServiceConfiguration> BookingServiceConfiguration { get; }
    
    public PortalAccessObject ()
    {
        BookingServiceConfiguration = Options.Create(
        new BookingServiceConfiguration()
        {
            StartTimeWorking = "8:00:00",
            EndTimeWorking = "23:00:00"
        });
        
    }

    protected void InitRepos(PortalDbContext context)
    {
        MenuRepository = new MenuRepository(context);
        UserRepository = new UserRepository(context);
        FeedbackRepository = new FeedbackRepository(context);
        InventoryRepository = new InventoryRepository(context);
        PackageRepository = new PackageRepository(context);
        ZoneRepository = new ZoneRepository(context);
        BookingRepository = new BookingRepository(context);
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
        return new User(userId, "Иванов", "Иван", "Иванович", new DateOnly(1998, 05, 17), Gender.Male, "ivanovvv@mail.ru", "+78888889");
    }

    public List<User> CreateMockUsers()
    {
        return new List<User>()
        {
            new User(Guid.NewGuid(), "Иванов", "Иван", "Иванович",  new DateOnly(2002, 03, 17), Gender.Male, "ivanov@mail.ru", "+7899999999", "password12123434"),
            new User(Guid.NewGuid(), "Петров", "Петр", "Петрович",  new DateOnly(2003, 05, 18), Gender.Male, "petrov@mail.ru", "+7899909999", "password12122323"),
            new User(Guid.NewGuid(), "Cударь", "Елена", "Александровна",  new DateOnly(1999, 09, 18), Gender.Female, "sudar@mail.ru", "+781211111", "password12121212")
        };
    }
    
    public List<Package> CreateMockPackages()
    {
        return new List<Package>()
        {
            new Package(Guid.NewGuid(), "Почасовая аренда", PackageType.Usual, 350, 2,
                "Почасовая стоимость аренды зала для компании людей", new List<Zone>(), new List<Dish>()),
            new Package(Guid.NewGuid(), "Пакет \"Для своих\"", PackageType.Simple, 999, 3,
                "Почасовая стоимость аренды зала для компании людей", new List<Zone>(), new List<Dish>())
        };
    }

    public List<Zone> CreateMockZones(List<Package> packages)
    {
        return new List<Zone>
        {
            new Zone(Guid.NewGuid(), "Zone1", "address1", 10, 10, 0.0, new List<Inventory>(), new List<Package>()),
            new Zone(Guid.NewGuid(), "Zone2", "address2", 30, 10, 0.0, new List<Inventory>(), new List<Package>()),
            new Zone(Guid.NewGuid(), "Zone3", "address3", 25, 10, 0.0, new List<Inventory>(), new List<Package>()),
            new Zone(Guid.NewGuid(), "Zone3", "address3", 25, 10, 0.0, new List<Inventory>(), new List<Package>())
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
                new TimeOnly(12, 00),
                DateTime.UtcNow,
                false, 100),
            new Booking(Guid.NewGuid(), zones[2].Id, users[1].Id, packages[1].Id, 
                10, BookingStatus.Reserved, 
                DateOnly.FromDateTime(DateTime.UtcNow + new TimeSpan(1, 0, 0, 0)),
                new TimeOnly(13, 00, 00), 
                new TimeOnly(15, 00, 00),
                DateTime.UtcNow,
                false, 100),
            new Booking(Guid.NewGuid(), zones[2].Id, users[1].Id, packages[1].Id, 
                10, BookingStatus.TemporaryReserved, 
                DateOnly.FromDateTime(DateTime.UtcNow + new TimeSpan(5, 0, 0, 0)),
                new TimeOnly(13, 00, 00), 
                new TimeOnly(15, 00, 00),
                DateTime.UtcNow,
                false, 100),
            new Booking(Guid.NewGuid(), zones[0].Id, users[2].Id, packages[0].Id, 
                10, BookingStatus.Done,
                DateOnly.Parse("2023.05.20"),
                new TimeOnly(12, 00), 
                new TimeOnly(18, 00),
                DateTime.UtcNow,
                false, 100),
            new Booking(Guid.NewGuid(), zones[0].Id, users[0].Id, packages[0].Id, 
                10, BookingStatus.Reserved, 
                DateOnly.FromDateTime(DateTime.UtcNow  + new TimeSpan(2, 0, 0, 0)),
                new TimeOnly(12, 00), 
                new TimeOnly(18, 00),
                DateTime.UtcNow,
                false, 100),
            new Booking(Guid.NewGuid(), zones[0].Id, users[0].Id, packages[0].Id, 
                10, BookingStatus.Cancelled, 
                DateOnly.FromDateTime(DateTime.UtcNow  + new TimeSpan(6, 0, 0, 0)),
                new TimeOnly(12, 00), 
                new TimeOnly(18, 00),
                DateTime.UtcNow,
                false, 100),
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

    public void Dispose()
    {
        Context.Dispose();
    }
}