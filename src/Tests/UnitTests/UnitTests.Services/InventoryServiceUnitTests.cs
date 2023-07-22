using Moq;
using Portal.Database.Repositories.Interfaces;
using Portal.Services.InventoryServices;
using Xunit;

namespace UnitTests.Services;

public class InventoryServiceUnitTests
{
    private readonly IInventoryService _inventoryService;
    private readonly Mock<IInventoryRepository> _mockInventoryRepository = new();

    public InventoryServiceUnitTests()
    {
        _inventoryService = new InventoryService(_mockInventoryRepository.Object);
    }

    [Fact]
    public async Task GetAllInventoriesTest()
    {
        // Arrange 
        
        // Act
        
        // Assert
    }
}