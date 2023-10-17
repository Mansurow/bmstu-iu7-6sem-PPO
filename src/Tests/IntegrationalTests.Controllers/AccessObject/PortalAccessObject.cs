using Microsoft.Extensions.Options;
using Portal.Common.Core;
using Portal.Common.Enums;
using Portal.Database.Core.Repositories;
using Portal.Database.Repositories.NpgsqlRepositories;
using Portal.Services.BookingService.Configuration;
using Portal.Services.OauthService.Configuration;
using Portal.Swagger;

namespace IntegrationalTests.Controllers.AccessObject;

public class PortalAccessObject: NpgsqlAccessObject
{
    public IUserRepository UserRepository { get; }
    public IZoneRepository ZoneRepository { get; }
    public IPackageRepository PackageRepository { get; }
    public IBookingRepository BookingRepository { get; }
    public IFeedbackRepository FeedbackRepository { get; }
    public IInventoryRepository InventoryRepository { get; }
    public IMenuRepository MenuRepository { get; }

    public IOptions<BookingServiceConfiguration> BookingServiceConfiguration { get; }

    public IOptions<AuthorizationConfiguration> AuthorizationConfiguration { get; }

    public PortalAccessObject()
    {
        BookingServiceConfiguration = Options.Create(
            new BookingServiceConfiguration()
            {
                StartTimeWorking = "8:00:00",
                EndTimeWorking = "23:00:00",
                TemporaryReservedBookingTime = new TimeSpan(0, 30, 0)
            }
        );

        AuthorizationConfiguration = Options.Create(
            new AuthorizationConfiguration()
            {
                SecretKey = "7iMdnuwf7XMMKGXGSMHKcs+qicGCinCJONLPrhGOX94=",
                TokenLifeTime = 3600
            }
        );

        UserRepository = new UserRepository(_context);
        ZoneRepository = new ZoneRepository(_context);
        PackageRepository = new PackageRepository(_context);
        BookingRepository = new BookingRepository(_context);
        FeedbackRepository = new FeedbackRepository(_context);
        InventoryRepository = new InventoryRepository(_context);
        MenuRepository = new MenuRepository(_context);
    }

    public async Task InsertManyUsers(List<User> users)
    {
        foreach (var user in users)
        {
            await UserRepository.InsertUserAsync(user);
        }
    }

    public async Task InsertManyMenu(List<Dish> menu)
    {
        foreach (var dish in menu)
        {
            await MenuRepository.InsertDishAsync(dish);
        }
    }
    
    public async Task InsertManyZones(List<Zone> zones)
    {
        foreach (var zone in zones)
        {
            await ZoneRepository.InsertZoneAsync(zone);
        }
    }
    
    public async Task InsertManyPackages(List<Package> packages)
    {
        foreach (var package in packages)
        {
            await PackageRepository.InsertPackageAsync(package);
        }
    }
    
    public async Task InsertManyFeedbacks(List<Feedback> feedbacks)
    {
        foreach (var feedback in feedbacks)
        {
            await FeedbackRepository.InsertFeedbackAsync(feedback);
        }
    }
    
    public List<User> CreateMockUsers()
    {
        return new List<User>()
        {
            CreateMockUser("admin", "admin", Role.Administrator),
            CreateMockUser("user1", "password123", Role.User),
            CreateMockUser("user2", "password123", Role.User),
            CreateMockUser("user3", "password123", Role.User),
        };
    }

    public List<Zone> CreateMockZones()
    {
        return new List<Zone>()
        {
            new Zone(Guid.NewGuid(), "name1", "address1", 10, 20, 0, new List<Inventory>(), new List<Package>()),
            new Zone(Guid.NewGuid(), "name2", "address2", 10, 11, 0, new List<Inventory>(), new List<Package>()),
            new Zone(Guid.NewGuid(), "name3", "address3", 10, 12, 0, new List<Inventory>(), new List<Package>()),
            new Zone(Guid.NewGuid(), "name4", "address4", 10, 6, 0, new List<Inventory>(), new List<Package>()),
            new Zone(Guid.NewGuid(), "name5", "address5", 10, 8, 0, new List<Inventory>(), new List<Package>())
        };
    }
    
    public List<Dish> CreateMockMenu()
    {
        return new List<Dish>()
        {
            new Dish(Guid.NewGuid(), "name1", DishType.FirstCourse, 350, "descriprion1"),
            new Dish(Guid.NewGuid(), "name2", DishType.FirstCourse, 350, "descriprion2"),
            new Dish(Guid.NewGuid(), "name3", DishType.FirstCourse, 350, "descriprion3"),
            new Dish(Guid.NewGuid(), "name4", DishType.FirstCourse, 350, "descriprion4"),
            new Dish(Guid.NewGuid(), "name5", DishType.FirstCourse, 350, "descriprion5")
        };
    }

    public User CreateMockUser(string email, string password, Role permissions)
    {
        var user = new User(Guid.NewGuid(), "Иванов", "Иван", "Иванович",
            DateOnly.Parse("31.12.1999"), Gender.Male, email, "9999999999", null, permissions);
        
        user.CreateHash(password);

        return user;
    }
    
    public List<Zone> CreateMockZonesWithInventory()
    {
        return new List<Zone>()
        {
            new Zone(Guid.NewGuid(), "name1", "address1", 10, 20, 0, new List<Inventory>(), new List<Package>()),
            new Zone(Guid.NewGuid(), "name2", "address2", 10, 11, 0, new List<Inventory>(), new List<Package>()),
            new Zone(Guid.NewGuid(), "name3", "address3", 10, 12, 0, new List<Inventory>(), new List<Package>()),
            new Zone(Guid.NewGuid(), "name4", "address4", 10, 6, 0, new List<Inventory>(), new List<Package>()),
            new Zone(Guid.NewGuid(), "name5", "address5", 10, 8, 0, new List<Inventory>(), new List<Package>())
        };
    }

    public List<Feedback> CreateMockFeedbacks(List<User> users, List<Zone> zones)
    {
        return new List<Feedback>()
        {
            new Feedback(Guid.NewGuid(), users.First().Id, zones.First().Id, DateTime.UtcNow, 5, "Круто!"),
            new Feedback(Guid.NewGuid(), users[1].Id, zones[1].Id, DateTime.UtcNow, 2, "Круто!"),
            new Feedback(Guid.NewGuid(), users[2].Id, zones[2].Id, DateTime.UtcNow, 3, "Круто!"),
            new Feedback(Guid.NewGuid(), users[3].Id, zones.Last().Id, DateTime.UtcNow, 4, "Круто!"),
        };
    }

    public List<Package> CreateMockPackages()
    {
        return new List<Package>()
        {
            new Package(Guid.NewGuid(), "name1", PackageType.Holidays, 250, 6, "description1",
                new List<Zone>(), new List<Dish>()),
            new Package(Guid.NewGuid(), "name2", PackageType.Holidays, 250, 6, "description2",
                new List<Zone>(), new List<Dish>()),
            new Package(Guid.NewGuid(), "name2", PackageType.Holidays, 250, 6, "description3",
                new List<Zone>(), new List<Dish>()),
            new Package(Guid.NewGuid(), "name3", PackageType.Holidays, 250, 6, "description4",
                new List<Zone>(), new List<Dish>())
        };
    }
}


