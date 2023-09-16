using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Portal.Common.Models;
using Portal.Common.Models.Enums;
using Portal.Database.Core.Repositories;
using Portal.Services.PackageService;
using Portal.Services.PackageService.Exceptions;
using Xunit;

namespace UnitTests.Services;

/// <summary>
/// Класс тестов PackageService
/// </summary>
public class PackageServiceUnitTests
{
    private readonly IPackageService _packageService;
    private readonly Mock<IPackageRepository> _mockPackageRepository = new();
    private readonly Mock<IMenuRepository> _mockMenuRepository = new();
    
    public PackageServiceUnitTests()
    {
        _packageService = new PackageService(_mockPackageRepository.Object,
            NullLogger<PackageService>.Instance,
            _mockMenuRepository.Object);
    }

    /// <summary>
    /// Тест на получение всех пакетов
    /// </summary>
    [Fact]
    public async Task GetAllPackagesTest()
    {
        // Arrange
        var packages = CreateMockPackages();

        _mockPackageRepository.Setup(s => s.GetAllPackagesAsync())
            .ReturnsAsync(packages);

        // Act
        var actualPackages = await _packageService.GetPackagesAsync();

        // Assert
        Assert.Equal(packages, actualPackages);
    }
    
    /// <summary>
    /// Тест на получение всех пакетов
    /// </summary>
    [Fact]
    public async Task GetAllPackagesEmptyTest()
    {
        // Arrange
        var packages = new List<Package>();

        _mockPackageRepository.Setup(s => s.GetAllPackagesAsync())
            .ReturnsAsync(packages);

        // Act
        var actualPackages = await _packageService.GetPackagesAsync();

        // Assert
        Assert.Equal(packages, actualPackages);
    }

    /// <summary>
    /// Тест на получение пакета
    /// </summary>
    [Fact]
    public async Task GetPackageByIdTest()
    {
        // Arrange
        var packages = CreateMockPackages();
        var expectedPackage = packages.First();

        _mockPackageRepository.Setup(s => s.GetPackageByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid packageId) => packages.First(p => p.Id == packageId));

        // Act
        var actualPackage = await _packageService.GetPackageById(expectedPackage.Id);

        // Assert
        Assert.Equal(expectedPackage, actualPackage);
    }
    
    /// <summary>
    /// Тест на получение пакета
    /// </summary>
    [Fact]
    public async Task GetPackageByIdNotFoundTest()
    {
        // Arrange
        var packages = new List<Package>();
        if (packages == null) throw new ArgumentNullException(nameof(packages));
        var expectedPackageId = Guid.NewGuid();

        _mockPackageRepository.Setup(s => s.GetPackageByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid packageId) => packages.First(p => p.Id == packageId));

        // Act
        async Task<Package> Action() => await _packageService.GetPackageById(expectedPackageId);

        // Assert
        await Assert.ThrowsAsync<PackageNotFoundException>(Action);
    }
    
    /// <summary>
    /// Тест на добавления пакета
    /// </summary>
    [Fact]
    public async Task AddPackageTest()
    {
        // Arrange
        var packages = CreateMockPackages();
        var expectedPackage = new Package(Guid.NewGuid(), "Пакет \"День рождение\"", PackageType.Holidays,  
            9399, 10, "Отпразднуй свой день рождение", new List<Zone>(), new List<Dish>());

        _mockPackageRepository.Setup(s => s.InsertPackageAsync(It.IsAny<Package>()))
            .Callback((Package package) => packages.Add(package));

        // Act
        var actualPackageId = await _packageService.AddPackageAsync(expectedPackage.Name, expectedPackage.Type, 
            expectedPackage.Price, expectedPackage.RentalTime, expectedPackage.Description, new List<Guid>());
        var actualPackage = packages.First(p => p.Id == actualPackageId);
            
        // Assert
        Assert.NotEqual(Guid.Empty,actualPackageId);
        Assert.Equal(expectedPackage.Name, actualPackage.Name);
        Assert.Equal(expectedPackage.Type, actualPackage.Type);
        Assert.Equal(expectedPackage.Price, actualPackage.Price);
        Assert.Equal(expectedPackage.Description, actualPackage.Description);
        Assert.Equal(expectedPackage.Zones, actualPackage.Zones);
        Assert.Equal(expectedPackage.Dishes,actualPackage.Dishes);
        
    }
    
    /// <summary>
    /// Тест на добавления пакета
    /// </summary>
    [Fact]
    public async Task AddPackageEmptyTest()
    {
        // Arrange
        var packages = new List<Package>();
        var expectedPackage = new Package(Guid.NewGuid(), "Пакет \"День рождение\"", PackageType.Holidays,  
            9399, 10, "Отпразднуй свой день рождение", new List<Zone>(), new List<Dish>());

        _mockPackageRepository.Setup(s => s.InsertPackageAsync(It.IsAny<Package>()))
            .Callback((Package package) => packages.Add(package));

        // Act
        var actualPackageId = await _packageService.AddPackageAsync(expectedPackage.Name, expectedPackage.Type, 
            expectedPackage.Price, expectedPackage.RentalTime, expectedPackage.Description, new List<Guid>());
        var actualPackage = packages.First(p => p.Id == actualPackageId);
            
        // Assert
        Assert.NotEqual(Guid.Empty,actualPackageId);
        Assert.Equal(expectedPackage.Name, actualPackage.Name);
        Assert.Equal(expectedPackage.Type, actualPackage.Type);
        Assert.Equal(expectedPackage.Price, actualPackage.Price);
        Assert.Equal(expectedPackage.RentalTime, actualPackage.RentalTime);
        Assert.Equal(expectedPackage.Description, actualPackage.Description);
        Assert.Equal(expectedPackage.Zones, actualPackage.Zones);
        Assert.Equal(expectedPackage.Dishes,actualPackage.Dishes);
        
    }
    
    /// <summary>
    /// Тест на обновление пакета
    /// </summary>
    [Fact]
    public async Task UpdatePackageTest()
    {
        // Arrange
        var packages = CreateMockPackages();
        var expectedPackage = new Package(packages.First().Id, "Пакет \"День рождение\"", PackageType.Holidays,  
            9399, 10, "Отпразднуй свой день рождение", new List<Zone>(), new List<Dish>());

        _mockPackageRepository.Setup(s => s.GetPackageByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid packageId) => packages.First(e => e.Id == packageId));
        
        _mockPackageRepository.Setup(s => s.UpdatePackageAsync(It.IsAny<Package>()))
            .Callback((Package p) =>
            {
                var package = packages.First(e => e.Id == p.Id);
                package.Name = p.Name;
                package.Type = p.Type;
                package.Price = p.Price;
                package.Description = p.Description;
                package.RentalTime = p.RentalTime;
            });

        // Act
        await _packageService.UpdatePackageAsync(expectedPackage);
        var actualPackage = packages.First();
            
        // Assert
        Assert.Equal(expectedPackage.Name, actualPackage.Name);
        Assert.Equal(expectedPackage.Type, actualPackage.Type);
        Assert.Equal(expectedPackage.Price, actualPackage.Price);
        Assert.Equal(expectedPackage.RentalTime, actualPackage.RentalTime);
        Assert.Equal(expectedPackage.Description, actualPackage.Description);
        Assert.Equal(expectedPackage.Zones, actualPackage.Zones);
        Assert.Equal(expectedPackage.Dishes,actualPackage.Dishes);
    }

    /// <summary>
    /// Тест на обновление пакета
    /// </summary>
    [Fact]
    public async Task UpdatePackageNotFoundTest()
    {
        // Arrange
        var packages = CreateMockPackages();
        var expectedPackage = new Package(Guid.NewGuid(), "Пакет \"День рождение\"", PackageType.Holidays,  
            9399, 10, "Отпразднуй свой день рождение", new List<Zone>(), new List<Dish>());

        // _mockPackageRepository.Setup(s => s.GetPackageByIdAsync(It.IsAny<Guid>()))
        //     .ReturnsAsync((Guid packageId) => packages.FirstOrDefault(e => e.Id == packageId));
        
        _mockPackageRepository.Setup(s => s.UpdatePackageAsync(It.IsAny<Package>()))
            .Callback((Package p) =>
            {
                var package = packages.First(e => e.Id == p.Id);
                package.Name = p.Name;
                package.Type = p.Type;
                package.Price = p.Price;
                package.Description = p.Description;
                package.RentalTime = p.RentalTime;
            });

        // Act
        async Task Action() => await _packageService.UpdatePackageAsync(expectedPackage);
        // var actualPackage = packages.First();
            
        // Assert
        await Assert.ThrowsAsync<PackageNotFoundException>(Action);
    }

    /// <summary>
    /// Тест на удаление пакета
    /// </summary>
    [Fact]
    public async Task RemovePackageTest()
    {
        // Arrange
        var packages = CreateMockPackages();
        var expectedPackage = packages.First();
        var expectedPackageId = packages.First().Id;
        var expectedCount = packages.Count - 1;

        _mockPackageRepository.Setup(s => s.GetPackageByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid packageId) => packages.First(e => e.Id == packageId));
        
        _mockPackageRepository.Setup(s => s.DeletePackageAsync(It.IsAny<Guid>()))
            .Callback((Guid id) =>
            {
                var package = packages.First(p => p.Id == id);
                packages.Remove(package);
            });

        // Act
        await _packageService.RemovePackageAsync(expectedPackageId);
        var actualCount = packages.Count;
        var actualPackage = packages.First();    
        
        // Assert
        Assert.Equal(expectedCount, actualCount);
        Assert.NotEqual(expectedPackage, actualPackage);
    }

    /// <summary>
    /// Тест на удаление пакета
    /// </summary>
    [Fact]
    public async Task RemovePackageNotFoundTest()
    {
        // Arrange
        var packages = CreateMockPackages();
        var expectedPackageId = Guid.NewGuid();
        var expectedCount = packages.Count;

        // _mockPackageRepository.Setup(s => s.GetPackageByIdAsync(It.IsAny<Guid>()))
        //     .ReturnsAsync((Guid packageId) => packages.FirstOrDefault(e => e.Id == packageId));
        
        _mockPackageRepository.Setup(s => s.DeletePackageAsync(It.IsAny<Guid>()))
            .Callback((Guid id) =>
            {
                var package = packages.First(p => p.Id == id);
                packages.Remove(package);
            });

        // Act
        async Task Action() => await _packageService.RemovePackageAsync(expectedPackageId);
        var actualCount = packages.Count;
            
        // Assert
        Assert.Equal(expectedCount, actualCount);
        await Assert.ThrowsAsync<PackageNotFoundException>(Action);
    }
    
    /// <summary>
    /// Создание моковых данных о пакетах
    /// </summary>
    /// <returns>Список пакетов</returns>
    private List<Package> CreateMockPackages()
    {
        return new List<Package>()
        {
            new Package(Guid.NewGuid(), "Почасовая аренда", PackageType.Usual, 350, 2,
                "Почасовая стоимость аренды зала для компании людей", new List<Zone>(), new List<Dish>()),
            new Package(Guid.NewGuid(), "Пакет \"Для своих\"", PackageType.Simple, 999, 3,
                "Почасовая стоимость аренды зала для компании людей", new List<Zone>(), new List<Dish>())
        };
    }

    // private List<User> CreateMockUsers()
    // {
    //     return new List<User>()
    //     {
    //         new User(Guid.NewGuid(), "Иванов", "Иван", "Иванович",  new DateTime(2002, 03, 17), Gender.Male, "ivanov@mail.ru", "+7899999999", "password12123434"),
    //         new User(Guid.NewGuid(), "Петров", "Петр", "Петрович",  new DateTime(2003, 05, 18), Gender.Male, "petrov@mail.ru", "+7899909999", "password12122323"),
    //         new User(Guid.NewGuid(), "Cударь", "Елена", "Александровна",  new DateTime(1999, 09, 18), Gender.Female, "sudar@mail.ru", "+781211111", "password12121212")
    //     };
    // }
}