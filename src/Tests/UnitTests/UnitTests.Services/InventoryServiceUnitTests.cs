using Moq;
using Portal.Common.Models;
using Portal.Database.Repositories.Interfaces;
using Portal.Services.InventoryServices;
using Portal.Services.InventoryServices.Exceptions;
using Xunit;

namespace UnitTests.Services;

/// <summary>
/// Тестирование Сервиса инвентаря
/// </summary>
public class InventoryServiceUnitTests
{
    private readonly IInventoryService _inventoryService;
    private readonly Mock<IInventoryRepository> _mockInventoryRepository = new();

    public InventoryServiceUnitTests()
    {
        _inventoryService = new InventoryService(_mockInventoryRepository.Object);
    }

    /// <summary>
    /// Тест на получение всего инвентаря
    /// </summary>
    [Fact]
    public async Task GetAllInventoriesTest()
    {
        // Arrange 
        var inventories = CreateMockInventories();

        _mockInventoryRepository.Setup(s => s.GetAllInventoryAsync())
            .ReturnsAsync(inventories);
        
        // Act
        var actualInventories = await _inventoryService.GetAllInventoriesAsync();

        // Assert
        Assert.Equal(inventories, actualInventories);
    }
    
    /// <summary>
    /// Тест на получение всего инвентаря
    /// </summary>
    [Fact]
    public async Task GetAllInventoriesEmptyTest()
    {
        // Arrange 
        var inventories = new List<Inventory>();

        _mockInventoryRepository.Setup(s => s.GetAllInventoryAsync())
            .ReturnsAsync(inventories);
        
        // Act
        var actualInventories = await _inventoryService.GetAllInventoriesAsync();

        // Assert
        Assert.Equal(inventories, actualInventories);
    }

    /// <summary>
    /// Тест на получение инвентаря по идентификатору
    /// </summary>
    [Fact]
    public async Task GetInventoryByIdTest()
    {
        // Arrange 
        var inventories = CreateMockInventories();
        var expectedInventory = inventories.First();
        
        _mockInventoryRepository.Setup(s => s.GetInventoryByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => inventories.First(i => i.Id == id));
        
        // Act
        var actualInventory = await _inventoryService.GetInventoryByIdAsync(expectedInventory.Id);

        // Assert
        Assert.Equal(expectedInventory, actualInventory);
    }
    
    /// <summary>
    /// Тест на получение инвентаря по идентификатору
    /// </summary>
    [Fact]
    public async Task GetInventoryByIdEmptyTest()
    {
        // Arrange 
        var inventories = new List<Inventory>();
        var expectedInventoryId = Guid.NewGuid();
        
        _mockInventoryRepository.Setup(s => s.GetInventoryByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => inventories.FirstOrDefault(i => i.Id == id));
        
        // Act
        async Task<Inventory> Action() => await _inventoryService.GetInventoryByIdAsync(expectedInventoryId);

        // Assert
        await Assert.ThrowsAsync<InventoryNotFoundException>(Action);
    }
    
    /// <summary>
    /// Тест на получение инвентаря по идентификатору
    /// </summary>
    [Fact]
    public async Task GetInventoryByIdNotFoundTest()
    {
        // Arrange 
        var inventories = CreateMockInventories();
        var expectedInventoryId = Guid.NewGuid();
        
        _mockInventoryRepository.Setup(s => s.GetInventoryByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => inventories.FirstOrDefault(i => i.Id == id));
        
        // Act
        async Task<Inventory> Action() => await _inventoryService.GetInventoryByIdAsync(expectedInventoryId);

        // Assert
        await Assert.ThrowsAsync<InventoryNotFoundException>(Action);
    }

    /// <summary>
    /// Тест на добавление инвентаря
    /// </summary>
    [Fact]
    public async Task AddInventoryTest()
    {
        // Arrange
        var inventories = CreateMockInventories();

        var zoneId = Guid.NewGuid();
        var name = "NewInventory";
        var date = DateOnly.Parse("08.12.1999");
        var description = "description";

        var expectedCount = inventories.Count + 1;
        
        _mockInventoryRepository.Setup(s => s.InsertInventoryAsync(It.IsAny<Inventory>()))
            .Callback((Inventory inventory) => inventories.Add(inventory));

        // Act
        var actualInventoryId = await _inventoryService.AddInventoryAsync(zoneId, name, date, description);
        var actualInventory = inventories.First(i => i.Id == actualInventoryId);
        var actualCount = inventories.Count;

        // Assert
        Assert.Equal(expectedCount, actualCount);
        Assert.NotEqual(Guid.Empty, actualInventoryId);
        Assert.Equal(zoneId, actualInventory.ZoneId);
        Assert.Equal(name, actualInventory.Name);
        Assert.Equal(date, actualInventory.YearOfProduction);
        Assert.Equal(description, actualInventory.Description);
    }
    
    /// <summary>
    /// Тест на добавление инвентаря
    /// </summary>
    [Fact]
    public async Task AddInventoryEmptyTest()
    {
        // Arrange
        var inventories = new List<Inventory>();

        var zoneId = Guid.NewGuid();
        var name = "NewInventory";
        var date = DateOnly.Parse("08.12.1999");
        var description = "description";

        var expectedCount = inventories.Count + 1;
        
        _mockInventoryRepository.Setup(s => s.InsertInventoryAsync(It.IsAny<Inventory>()))
            .Callback((Inventory inventory) => inventories.Add(inventory));

        // Act
        var actualInventoryId = await _inventoryService.AddInventoryAsync(zoneId, name, date, description);
        var actualInventory = inventories.First(i => i.Id == actualInventoryId);
        var actualCount = inventories.Count;

        // Assert
        Assert.Equal(expectedCount, actualCount);
        Assert.NotEqual(Guid.Empty, actualInventoryId);
        Assert.Equal(zoneId, actualInventory.ZoneId);
        Assert.Equal(name, actualInventory.Name);
        Assert.Equal(date, actualInventory.YearOfProduction);
        Assert.Equal(description, actualInventory.Description);
    }

    /// <summary>
    /// Тест на обновление инвентаря
    /// </summary>
    [Fact]
    public async Task UpdateInventoryTest()
    {
        // Arrange
        var inventories = CreateMockInventories();

        var updateInventory = new Inventory(inventories.First().Id, Guid.NewGuid(), 
            "update inventory", "description", 
            new DateOnly(2002, 12, 20));
        var expectedCount = inventories.Count;
        
        _mockInventoryRepository.Setup(s => s.GetInventoryByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => inventories.First(i => i.Id == id));
        
        _mockInventoryRepository.Setup(s => s.UpdateInventoryAsync(It.IsAny<Inventory>()))
            .Callback((Inventory ui) =>
            {
                var inventory = inventories.First(e => e.Id == ui.Id);
                inventory.Name = ui.Name;
                inventory.Description = ui.Description;
                inventory.YearOfProduction = ui.YearOfProduction;
            });

        // Act
        await _inventoryService.UpdateInventoryAsync(updateInventory);
        var actualInventory = inventories.First(i => i.Id == updateInventory.Id);
        var actualCount = inventories.Count;

        // Assert
        Assert.Equal(expectedCount, actualCount);
        Assert.Equal(updateInventory.Id, actualInventory.Id);
        Assert.Equal(updateInventory.Name, actualInventory.Name);
        Assert.Equal(updateInventory.YearOfProduction, actualInventory.YearOfProduction);
        Assert.Equal(updateInventory.Description, actualInventory.Description);
    }

    /// <summary>
    /// Тест на обновление инвентаря
    /// </summary>
    [Fact]
    public async Task UpdateInventoryNotFoundTest()
    {
        // Arrange
        var inventories = CreateMockInventories();

        var updateInventory = new Inventory(Guid.NewGuid(), Guid.NewGuid(),
            "update inventory", "description", 
            new DateOnly(2002, 12, 20));
        var expectedCount = inventories.Count;
        
        _mockInventoryRepository.Setup(s => s.GetInventoryByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => inventories.FirstOrDefault(i => i.Id == id));
        
        _mockInventoryRepository.Setup(s => s.UpdateInventoryAsync(It.IsAny<Inventory>()))
            .Callback((Inventory ui) =>
            {
                var inventory = inventories.First(e => e.Id == ui.Id);
                inventory.Name = ui.Name;
                inventory.Description = ui.Description;
                inventory.YearOfProduction = ui.YearOfProduction;
            });

        // Act
        async Task Action() => await _inventoryService.UpdateInventoryAsync(updateInventory);

        // Assert
        await Assert.ThrowsAsync<InventoryNotFoundException>(Action);
    }
    
    /// <summary>
    /// Тест на удаление инвентаря
    /// </summary>
    [Fact]
    public async Task RemoveInventoryTest()
    {
        // Arrange
        var inventories = CreateMockInventories();

        var notExpectedInventory = inventories.First();
        var removeInventoryId = notExpectedInventory.Id;
        var expectedCount = inventories.Count - 1;
        
        _mockInventoryRepository.Setup(s => s.GetInventoryByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => inventories.First(i => i.Id == id));
        
        _mockInventoryRepository.Setup(s => s.DeleteInventoryAsync(It.IsAny<Guid>()))
            .Callback((Guid id) =>
            {
                var inventory = inventories.First(e => e.Id == id);
                inventories.Remove(inventory);
            });

        // Act
        await _inventoryService.RemoveInventoryAsync(removeInventoryId);
        var actualCount = inventories.Count;
        var actualInventory = inventories.First();

        // Assert
        Assert.Equal(expectedCount, actualCount);
        Assert.NotEqual(notExpectedInventory, actualInventory);
    }
    
    /// <summary>
    /// Тест на удаление инвентаря
    /// </summary>
    [Fact]
    public async Task RemoveInventoryNotFoundTest()
    {
        // Arrange
        var inventories = CreateMockInventories();
        
        var removeInventoryId = Guid.NewGuid();
        var expectedCount = inventories.Count;
        
        _mockInventoryRepository.Setup(s => s.GetInventoryByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => inventories.FirstOrDefault(i => i.Id == id));
        
        _mockInventoryRepository.Setup(s => s.DeleteInventoryAsync(It.IsAny<Guid>()))
            .Callback((Guid id) =>
            {
                var inventory = inventories.First(e => e.Id == id);
                inventories.Remove(inventory);
            });

        // Act
        async Task Action() => await _inventoryService.RemoveInventoryAsync(removeInventoryId);
        var actualCount = inventories.Count;

        // Assert
        Assert.Equal(expectedCount, actualCount);
        await Assert.ThrowsAsync<InventoryNotFoundException>(Action);
    }
    
    /// <summary>
    /// Создать моковые данные про инвентарь
    /// </summary>
    /// <returns>Список инвентаря</returns>
    private List<Inventory> CreateMockInventories()
    {
        return new List<Inventory>()
        {
            new Inventory(Guid.NewGuid(), Guid.NewGuid(), "name1", "description1", DateOnly.Parse("2002.01.01")),
            new Inventory(Guid.NewGuid(), Guid.NewGuid(), "name2", "description2", DateOnly.Parse("2002.01.01")),
            new Inventory(Guid.NewGuid(), Guid.NewGuid(), "name3", "description3", DateOnly.Parse("2002.01.01")),
            new Inventory(Guid.NewGuid(), Guid.NewGuid(), "name4", "description4", DateOnly.Parse("2002.01.01")),
            new Inventory(Guid.NewGuid(), Guid.NewGuid(), "name5", "description5", DateOnly.Parse("2002.01.01")),
            new Inventory(Guid.NewGuid(), Guid.NewGuid(), "name6", "description6", DateOnly.Parse("2002.01.01")),
        };
    }
}