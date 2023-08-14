using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Portal.Common.Models;
using Portal.Common.Models.Enums;
using Xunit;
using Portal.Database.Repositories.Interfaces;
using Portal.Services.PackageService.Exceptions;
using Portal.Services.ZoneService;
using Portal.Services.ZoneService.Exceptions;

namespace UnitTests.Services;

/// <summary>
/// Тестирование ZoneService
/// </summary>
public class ZoneServiceUnitTests
{
    private readonly IZoneService _zoneService;
    private readonly Mock<IZoneRepository> _mockZoneRepository = new();
    private readonly Mock<IInventoryRepository> _mockInventoryRepository = new();
    private readonly Mock<IPackageRepository> _mockPackageRepository = new();

    public ZoneServiceUnitTests()
    {
        _zoneService = new ZoneService(_mockZoneRepository.Object,
            _mockInventoryRepository.Object,
            _mockPackageRepository.Object,
            NullLogger<ZoneService>.Instance);
    }
    
    /// <summary>
    /// Тест на получение всего инвентаря
    /// </summary>
    [Fact]
    public async Task GetAllZonesTest()
    {
        // Arrange 
        var zones = CreateMockZones(); 

        _mockZoneRepository.Setup(s => s.GetAllZonesAsync())
            .ReturnsAsync(zones);
        
        // Act
        var actualZones = await _zoneService.GetAllZonesAsync();

        // Asserts
        Assert.Equal(zones, actualZones);
    }
    
    /// <summary>
    /// Тест на получение всего инвентаря
    /// </summary>
    [Fact]
    public async Task GetAllZonesEmptyTest()
    {
        // Arrange 
        var zones = new List<Zone>(); 

        _mockZoneRepository.Setup(s => s.GetAllZonesAsync())
            .ReturnsAsync(zones);
        
        // Act
        var actualZones = await _zoneService.GetAllZonesAsync();

        // Asserts
        Assert.Equal(zones, actualZones);
    }
    
    /// <summary>
    /// Тест на получение инвентаря по идентификатору
    /// </summary>
    [Fact]
    public async Task GetZoneByIdTest()
    {
        // Arrange 
        var zones = CreateMockZones();
        var expectedZone = zones.First();
        
        _mockZoneRepository.Setup(s => s.GetZoneByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => zones.First(z => z.Id == id));
        
        // Act
        var actualZone = await _zoneService.GetZoneByIdAsync(expectedZone.Id);

        // Asserts
        Assert.Equal(expectedZone, actualZone);
    }
    
    /// <summary>
    /// Тест на получение инвентаря по идентификатору
    /// </summary>
    [Fact]
    public async Task GetZoneByIdNotFoundTest()
    {
        // Arrange 
        var zones = CreateMockZones();
        var expectedZoneId = Guid.NewGuid();
        var expectedCount = zones.Count;
        
        _mockZoneRepository.Setup(s => s.GetZoneByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => zones.First(z => z.Id == id));
        
        // Act
        async Task<Zone> Action() => await _zoneService.GetZoneByIdAsync(expectedZoneId);
        var actualCount = zones.Count;
        
        // Assert
        Assert.Equal(expectedCount, actualCount);
        await Assert.ThrowsAsync<ZoneNotFoundException>(Action);
    }

    /// <summary>
    /// Тест на добавление зоны
    /// </summary>
    [Fact]
    public async Task AddZoneTest()
    {
        // Arrange 
        var zones = CreateMockZones();
        var expectedCount = zones.Count + 1;
        
        var name = "new zone";
        var address = "address new zone";
        var size = 10.0;
        var limit = 15;
        var price = 449;
        
        _mockZoneRepository.Setup(s => s.GetZoneByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((string zoneName) => zones.First(z => z.Name == zoneName));

        _mockZoneRepository.Setup(s => s.InsertZoneAsync(It.IsAny<Zone>()))
            .Callback((Zone zone) => zones.Add(zone));
        
        // Act
        var actualZoneId = await _zoneService.AddZoneAsync(name, address, size, limit, price);
        var actualCount = zones.Count;
        var actualZone = zones.First(e => e.Id == actualZoneId);
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.NotEqual(Guid.Empty, actualZoneId);
        Assert.Equal(name, actualZone.Name);
        Assert.Equal(address, actualZone.Address);
        Assert.Equal(size, actualZone.Size);
        Assert.Equal(limit, actualZone.Limit);
        Assert.Equal(price, actualZone.Price);
        Assert.Equal(0, actualZone.Inventories.Count);
        Assert.Equal(0, actualZone.Packages.Count);
    }

    /// <summary>
    /// Тест на добавление зоны
    /// </summary>
    [Fact]
    public async Task AddZoneEmptyTest()
    {
        // Arrange 
        var zones = new List<Zone>();
        var expectedCount = zones.Count + 1;
        
        var name = "new zone";
        var address = "address new zone";
        var size = 10.0;
        var limit = 15;
        var price = 449;
        
        _mockZoneRepository.Setup(s => s.GetZoneByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((string zoneName) => zones.First(z => z.Name == zoneName));

        _mockZoneRepository.Setup(s => s.InsertZoneAsync(It.IsAny<Zone>()))
            .Callback((Zone zone) => zones.Add(zone));
        
        // Act
        var actualZoneId = await _zoneService.AddZoneAsync(name, address, size, limit, price);
        var actualCount = zones.Count;
        var actualZone = zones.First(e => e.Id == actualZoneId);
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.NotEqual(Guid.Empty, actualZoneId);
        Assert.Equal(name, actualZone.Name);
        Assert.Equal(address, actualZone.Address);
        Assert.Equal(size, actualZone.Size);
        Assert.Equal(limit, actualZone.Limit);
        Assert.Equal(price, actualZone.Price);
        Assert.Equal(0, actualZone.Inventories.Count);
        Assert.Equal(0, actualZone.Packages.Count);
    }
    
    /// <summary>
    /// Тест на добавление зоны
    /// </summary>
    [Fact]
    public async Task AddZoneExistsNameTest()
    {
        // Arrange 
        var zones = CreateMockZones();
        var expectedZone = zones.First();
        var expectedCount = zones.Count;
        
        var name = "Zone1";
        var address = "address Zone1"!;
        var size = 10.0;
        var limit = 15;
        var price = 449;
        
        _mockZoneRepository.Setup(s => s.GetZoneByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((string zoneName) => zones.First(z => z.Name == zoneName));

        // _mockZoneRepository.Setup(s => s.InsertZoneAsync(It.IsAny<Zone>()))
        //     .Callback((Zone zone) => zones.Add(zone));
        
        // Act
        async Task<Guid> Action() => await _zoneService.AddZoneAsync(name!, address!, size, limit, price);
        var actualCount = zones.Count;
        var actualZone = zones.First();
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.Equal(expectedZone, actualZone);
        await Assert.ThrowsAsync<ZoneNameExistException>(Action);
    }
    
    /// <summary>
    /// Тест на обновление зоны
    /// </summary>
    [Fact]
    public async Task UpdateZoneTest()
    {
        // Arrange 
        var zones = CreateMockZones();
        var expectedCount = zones.Count;

        var beforeUpdateZone = zones.First();
        var updateZone = new Zone(beforeUpdateZone.Id, "update zone", "address", 
            10.0, 15, 350.99, 0.0);
        
        _mockZoneRepository.Setup(s => s.GetZoneByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => zones.First(z => z.Id == id));
        
        _mockZoneRepository.Setup(s => s.GetZoneByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((string zoneName) => zones.FirstOrDefault(z => z.Name == zoneName));

        _mockZoneRepository.Setup(s => s.UpdateZoneAsync(It.IsAny<Zone>()))
            .Callback((Zone uzone) =>
            {
                var zone = zones.First(e => e.Id == uzone.Id);
                zone.Name = uzone.Name;
                zone.Address = uzone.Address;
                zone.Limit = uzone.Limit;
                zone.Size = uzone.Size;
                zone.Price = uzone.Price;
                zone.Inventories = uzone.Inventories;
                zone.Packages = uzone.Packages;
            });
        
        // Act
        await _zoneService.UpdateZoneAsync(updateZone);
        var actualCount = zones.Count;
        var actualZone = zones.First(e => e.Id == beforeUpdateZone.Id);
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.Equal(updateZone.Name, actualZone.Name);
        Assert.Equal(updateZone.Address, actualZone.Address);
        Assert.Equal(updateZone.Size, actualZone.Size);
        Assert.Equal(updateZone.Limit, actualZone.Limit);
        Assert.Equal(updateZone.Price, actualZone.Price);
        Assert.Equal(updateZone.Inventories, actualZone.Inventories);
        Assert.Equal(updateZone.Packages, actualZone.Packages);
    }
    
    /// <summary>
    /// Тест на обновление зоны
    /// </summary>
    // [Fact]
    // public async Task UpdateZoneNameExistsTest()
    // {
    //     // Arrange 
    //     var zones = CreateMockZones();
    //     var expectedCount = zones.Count;
    //     
    //     var beforeUpdateZone = zones.First();
    //     var updateZone = new Zone(beforeUpdateZone.Id, "Zone1", "address", 
    //         10.0, 15, 350.99, 0.0);
    //     
    //     _mockZoneRepository.Setup(s => s.GetZoneByIdAsync(It.IsAny<Guid>()))
    //         .ReturnsAsync((Guid id) => zones.First(z => z.Id == id));
    //     
    //     _mockZoneRepository.Setup(s => s.GetZoneByNameAsync(It.IsAny<string>()))
    //         .ReturnsAsync((string zoneName) => zones.First(z => z.Name == zoneName));
    //     
    //     _mockZoneRepository.Setup(s => s.UpdateZoneAsync(It.IsAny<Zone>()))
    //         .Callback((Zone uzone) =>
    //         {
    //             var zone = zones.First(e => e.Id == uzone.Id);
    //             zone.Name = uzone.Name;
    //             zone.Address = uzone.Address;
    //             zone.Limit = uzone.Limit;
    //             zone.Size = uzone.Size;
    //             zone.Price = uzone.Price;
    //             zone.Inventories = uzone.Inventories;
    //             zone.Packages = uzone.Packages;
    //         });
    //     
    //     // Act
    //     async Task Action() => await _zoneService.UpdateZoneAsync(updateZone);
    //     var actualCount = zones.Count;
    //
    //     // Asserts
    //     Assert.Equal(expectedCount, actualCount);
    //     await Assert.ThrowsAsync<ZoneNameExistException>(Action);
    // }
    
    /// <summary>
    /// Тест на обновление зоны
    /// </summary>
    [Fact]
    public async Task UpdateZoneNotFoundTest()
    {
        // Arrange 
        var zones = CreateMockZones();
        var expectedCount = zones.Count;
        
        var updateZone = new Zone(Guid.NewGuid(), "update zone", "address", 
            10.0, 15, 350.99, 0.0);
        
        _mockZoneRepository.Setup(s => s.GetZoneByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => zones.First(z => z.Id == id));
        
        _mockZoneRepository.Setup(s => s.GetZoneByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((string zoneName) => zones.First(z => z.Name == zoneName));
        
        _mockZoneRepository.Setup(s => s.UpdateZoneAsync(It.IsAny<Zone>()))
            .Callback((Zone uzone) =>
            {
                var zone = zones.First(e => e.Id == uzone.Id);
                zone.Name = uzone.Name;
                zone.Address = uzone.Address;
                zone.Limit = uzone.Limit;
                zone.Size = uzone.Size;
                zone.Price = uzone.Price;
                zone.Inventories = uzone.Inventories;
                zone.Packages = uzone.Packages;
            });
        
        // Act
        async Task Action() => await _zoneService.UpdateZoneAsync(updateZone);
        var actualCount = zones.Count;

        // Asserts
        Assert.Equal(expectedCount, actualCount);
        await Assert.ThrowsAsync<ZoneNotFoundException>(Action);
    }

    /// <summary>
    /// Тест на добавление к зоне инвентаря
    /// </summary>
    [Fact]
    public async Task AddInventoryTest()
    {
        // Arrange
        var packages = CreateMockPackages();
        var zones = CreateMockZones();
        
        zones[0].AddPackage(packages.First());
        zones[0].AddPackage(packages.Last());
        zones[1].AddPackage(packages.First());
        zones[2].AddPackage(packages.First());

        zones[0].Inventories = CreateMockInventories(zones[0].Id);
        zones[1].Inventories = CreateMockInventories(zones[1].Id);
        
        var expectedZone = zones[2];
        var expectedCount = expectedZone.Inventories.Count + 1;
        var inventory = new Inventory(Guid.NewGuid(), expectedZone.Id, 
            "new inventory", "description", new DateOnly(2000, 10, 08));
        
        _mockZoneRepository.Setup(s => s.GetZoneByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => zones.First(z => z.Id == id));
        
        _mockZoneRepository.Setup(s => s.UpdateZoneAsync(It.IsAny<Zone>()))
        .Callback((Zone uzone) =>
        {
            var zone = zones.First(e => e.Id == uzone.Id);
            zone.Name = uzone.Name;
            zone.Address = uzone.Address;
            zone.Limit = uzone.Limit;
            zone.Size = uzone.Size;
            zone.Price = uzone.Price;
            zone.Inventories = uzone.Inventories;
            zone.Packages = uzone.Packages;
        });
        
        // Act
        await _zoneService.AddInventoryAsync(expectedZone.Id, inventory);
        var actualZone = zones.First(e => e.Id == expectedZone.Id);
        var actualCount = actualZone.Inventories.Count;
        var newActualInventory = actualZone.Inventories.First(e => e.Id == inventory.Id);
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.Equal(inventory, newActualInventory);
    }
    
    /// <summary>
    /// Тест на добавление к зоне инвентаря
    /// </summary>
    [Fact]
    public async Task AddInventoryNotFoundTest()
    {
        // Arrange
        var packages = CreateMockPackages();
        var zones = CreateMockZones();
        
        zones[0].AddPackage(packages.First());
        zones[0].AddPackage(packages.Last());
        zones[1].AddPackage(packages.First());
        zones[2].AddPackage(packages.First());

        zones[0].Inventories = CreateMockInventories(zones[0].Id);
        zones[1].Inventories = CreateMockInventories(zones[1].Id);
        
        var expectedZoneId = Guid.NewGuid();
        var inventory = new Inventory(Guid.NewGuid(), expectedZoneId, 
            "new inventory", "description", new DateOnly(2000, 10, 08));
        
        _mockZoneRepository.Setup(s => s.GetZoneByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => zones.First(z => z.Id == id));
        
        _mockZoneRepository.Setup(s => s.UpdateZoneAsync(It.IsAny<Zone>()))
            .Callback((Zone uzone) =>
            {
                var zone = zones.First(e => e.Id == uzone.Id);
                zone.Name = uzone.Name;
                zone.Address = uzone.Address;
                zone.Limit = uzone.Limit;
                zone.Size = uzone.Size;
                zone.Price = uzone.Price;
                zone.Inventories = uzone.Inventories;
                zone.Packages = uzone.Packages;
            });
        
        // Act
        async Task Action() => await _zoneService.AddInventoryAsync(expectedZoneId, inventory);
        var actualZone = zones.FirstOrDefault(e => e.Id == expectedZoneId);

        // Asserts
        await Assert.ThrowsAsync<ZoneNotFoundException>(Action);
    }
    
    /// <summary>
    /// Тест на добавление к зоне пакета
    /// </summary>
    [Fact]
    public async Task AddPackageTest()
    {
        // Arrange
        var packages = CreateMockPackages();
        var zones = CreateMockZones();
        
        zones[0].AddPackage(packages.First());
        zones[0].AddPackage(packages.Last());
        zones[1].AddPackage(packages.First());
        zones[2].AddPackage(packages.First());

        zones[0].Inventories = CreateMockInventories(zones[0].Id);
        zones[1].Inventories = CreateMockInventories(zones[1].Id);

        var expectedPackage = packages.First();
        var expectedZone = zones[3];
        var expectedCount = expectedZone.Packages.Count + 1;

        _mockPackageRepository.Setup(s => s.GetPackageByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid packageId) => packages.First(p => p.Id == packageId));
        
        _mockZoneRepository.Setup(s => s.GetZoneByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => zones.First(z => z.Id == id));
        
        _mockZoneRepository.Setup(s => s.UpdateZoneAsync(It.IsAny<Zone>()))
        .Callback((Zone uzone) =>
        {
            var zone = zones.First(e => e.Id == uzone.Id);
            zone.Name = uzone.Name;
            zone.Address = uzone.Address;
            zone.Limit = uzone.Limit;
            zone.Size = uzone.Size;
            zone.Price = uzone.Price;
            zone.Inventories = uzone.Inventories;
            zone.Packages = uzone.Packages;
        });
        
        // Act
        await _zoneService.AddPackageAsync(expectedZone.Id, expectedPackage.Id);
        var actualZone = zones.First(e => e.Id == expectedZone.Id);
        var actualCount = actualZone.Packages.Count;
        var newActualPackage = actualZone.Packages.First(e => e.Id == expectedPackage.Id);
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.Equal(expectedPackage, newActualPackage);
    }
    
    /// <summary>
    /// Тест на добавление к зоне инвентаря
    /// </summary>
    [Fact]
    public async Task AddPackageNotFoundZoneTest()
    {
        // Arrange
        var packages = CreateMockPackages();
        var zones = CreateMockZones();
        
        zones[0].AddPackage(packages.First());
        zones[0].AddPackage(packages.Last());
        zones[1].AddPackage(packages.First());
        zones[2].AddPackage(packages.First());

        zones[0].Inventories = CreateMockInventories(zones[0].Id);
        zones[1].Inventories = CreateMockInventories(zones[1].Id);

        var expectedPackage = packages.First();
        var expectedZone = zones[3];
        var expectedCount = expectedZone.Packages.Count + 1;

        _mockPackageRepository.Setup(s => s.GetPackageByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid packageId) => packages.First(p => p.Id == packageId));
        
        _mockZoneRepository.Setup(s => s.GetZoneByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => zones.First(z => z.Id == id));
        
        _mockZoneRepository.Setup(s => s.UpdateZoneAsync(It.IsAny<Zone>()))
            .Callback((Zone uzone) =>
            {
                var zone = zones.First(e => e.Id == uzone.Id);
                zone.Name = uzone.Name;
                zone.Address = uzone.Address;
                zone.Limit = uzone.Limit;
                zone.Size = uzone.Size;
                zone.Price = uzone.Price;
                zone.Inventories = uzone.Inventories;
                zone.Packages = uzone.Packages;
            });
        
        // Act
        async Task Action() => await _zoneService.AddPackageAsync(Guid.NewGuid(), expectedPackage.Id);

        // Asserts
        await Assert.ThrowsAsync<ZoneNotFoundException>(Action);
    }
    
    /// <summary>
    /// Тест на добавление к зоне инвентаря
    /// </summary>
    [Fact]
    public async Task AddPackageNotFoundPackageTest()
    {
        // Arrange
        var packages = CreateMockPackages();
        var zones = CreateMockZones();
        
        zones[0].AddPackage(packages.First());
        zones[0].AddPackage(packages.Last());
        zones[1].AddPackage(packages.First());
        zones[2].AddPackage(packages.First());

        zones[0].Inventories = CreateMockInventories(zones[0].Id);
        zones[1].Inventories = CreateMockInventories(zones[1].Id);

        var expectedPackage = packages.First();
        var expectedZone = zones[3];
        var expectedCount = expectedZone.Packages.Count + 1;

        _mockPackageRepository.Setup(s => s.GetPackageByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid packageId) => packages.First(p => p.Id == packageId));
        
        _mockZoneRepository.Setup(s => s.GetZoneByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => zones.First(z => z.Id == id));
        
        _mockZoneRepository.Setup(s => s.UpdateZoneAsync(It.IsAny<Zone>()))
            .Callback((Zone uzone) =>
            {
                var zone = zones.First(e => e.Id == uzone.Id);
                zone.Name = uzone.Name;
                zone.Address = uzone.Address;
                zone.Limit = uzone.Limit;
                zone.Size = uzone.Size;
                zone.Price = uzone.Price;
                zone.Inventories = uzone.Inventories;
                zone.Packages = uzone.Packages;
            });
        
        // Act
        async Task Action() => await _zoneService.AddPackageAsync(expectedZone.Id, Guid.NewGuid());

        // Asserts
        await Assert.ThrowsAsync<PackageNotFoundException>(Action);
    }
    
    /// <summary>
    /// Тест на добавление к зоне пакета
    /// </summary>
    [Fact]
    public async Task AddPackageExistsInZoneTest()
    {
        // Arrange
        var packages = CreateMockPackages();
        var zones = CreateMockZones();
        
        zones[0].AddPackage(packages.First());
        zones[0].AddPackage(packages.Last());
        zones[1].AddPackage(packages.First());
        zones[2].AddPackage(packages.First());

        zones[0].Inventories = CreateMockInventories(zones[0].Id);
        zones[1].Inventories = CreateMockInventories(zones[1].Id);

        var expectedPackage = packages.First();
        var expectedZone = zones[1];
        var expectedCount = expectedZone.Packages.Count;

        _mockPackageRepository.Setup(s => s.GetPackageByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid packageId) => packages.First(p => p.Id == packageId));
        
        _mockZoneRepository.Setup(s => s.GetZoneByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => zones.First(z => z.Id == id));
        
        _mockZoneRepository.Setup(s => s.UpdateZoneAsync(It.IsAny<Zone>()))
            .Callback((Zone uzone) =>
            {
                var zone = zones.First(e => e.Id == uzone.Id);
                zone.Name = uzone.Name;
                zone.Address = uzone.Address;
                zone.Limit = uzone.Limit;
                zone.Size = uzone.Size;
                zone.Price = uzone.Price;
                zone.Inventories = uzone.Inventories;
                zone.Packages = uzone.Packages;
            });
        
        // Act
        async Task Action() => await _zoneService.AddPackageAsync(expectedZone.Id, expectedPackage.Id);
        var actualZone = zones.First(e => e.Id == expectedZone.Id);
        var actualCount = actualZone.Packages.Count;

        // Asserts
        Assert.Equal(expectedCount, actualCount);
        await Assert.ThrowsAsync<ZonePackageExistsException>(Action);
    }

    /// <summary>
    ///  Тест на удаление зоны
    /// </summary>
    [Fact]
    public async Task RemoveZoneTest()
    {
        // Arrange
        var zones = CreateMockZones();
    
        var removedZone = zones.First();
        var expectedCount = zones.Count - 1;
        
        _mockZoneRepository.Setup(s => s.GetZoneByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid zoneId) => zones.First(z => z.Id == zoneId));

        _mockZoneRepository.Setup(s => s.DeleteZoneAsync(It.IsAny<Guid>()))
            .Callback((Guid zoneId) =>
            {
                var zone = zones.First(z => z.Id == zoneId);
                zones.Remove(zone);
            });

        // Act
        await _zoneService.RemoveZoneAsync(removedZone.Id);
        var actualCount = zones.Count;
        var actualZone = zones.First();

        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.NotEqual(removedZone, actualZone);
    }
    
    /// <summary>
    ///  Тест на удаление зоны
    /// </summary>
    [Fact]
    public async Task RemoveZoneNotFoundTest()
    {
        // Arrange
        var zones = CreateMockZones();
        
        var expectedCount = zones.Count;
        
        _mockZoneRepository.Setup(s => s.GetZoneByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid zoneId) => zones.First(z => z.Id == zoneId));

        _mockZoneRepository.Setup(s => s.DeleteZoneAsync(It.IsAny<Guid>()))
            .Callback((Guid zoneId) =>
            {
                var zone = zones.First(z => z.Id == zoneId);
                zones.Remove(zone);
            });

        // Act
        async Task Action() => await _zoneService.RemoveZoneAsync(Guid.NewGuid());
        var actualCount = zones.Count;

        // Asserts
        Assert.Equal(expectedCount, actualCount);
        await Assert.ThrowsAsync<ZoneNotFoundException>(Action);
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
                "Почасовая стоимость аренды зала для компании людей"),
            new Package(Guid.NewGuid(), "Пакет \"Для своих\"", PackageType.Simple, 999, 3,
                "Почасовая стоимость аренды зала для компании людей")
        };
    }
    
    /// <summary>
    /// Создать моковые данные про инвентарь
    /// </summary>
    /// <param name="zoneId">Идентификатор зоны</param>
    /// <returns>Список инвентаря</returns>
    private List<Inventory> CreateMockInventories(Guid zoneId)
    {
        return new List<Inventory>()
        {
            new Inventory(Guid.NewGuid(), zoneId, "name1", "description1", DateOnly.Parse("2002.01.01")),
            new Inventory(Guid.NewGuid(), zoneId, "name2", "description2", DateOnly.Parse("2002.01.01")),
            new Inventory(Guid.NewGuid(), zoneId, "name3", "description3", DateOnly.Parse("2002.01.01")),
            new Inventory(Guid.NewGuid(), zoneId, "name4", "description4", DateOnly.Parse("2002.01.01")),
            new Inventory(Guid.NewGuid(), zoneId, "name5", "description5", DateOnly.Parse("2002.01.01")),
            new Inventory(Guid.NewGuid(), zoneId, "name6", "description6", DateOnly.Parse("2002.01.01")),
        };
    }
    
    /// <summary>
    /// Создание моковых данных о зонах
    /// </summary>
    /// <returns>Список зон</returns>
    private List<Zone> CreateMockZones()
    {
        return new List<Zone>
        {
            new Zone(Guid.NewGuid(), "Zone1", "address1", 10, 6, 2500, 4),
            new Zone(Guid.NewGuid(), "Zone2", "address2", 30, 6, 3500, 0.0),
            new Zone(Guid.NewGuid(), "Zone3", "address3", 25, 10, 3000, 0.0),
            new Zone(Guid.NewGuid(), "Zone3", "address3", 25, 10, 3000, 0.0)
        };
    }
}