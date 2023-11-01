using IntegrationalTests.Services.AccessObject;
using Microsoft.Extensions.Logging.Abstractions;
using Portal.Common.Core;
using Portal.Common.Models;
using Portal.Services.MenuService;
using Portal.Services.MenuService.Exceptions;
using Xunit;

namespace IntegrationalTests.Services.InMemory;

public class MenuServiceIntegrationTests: IDisposable
{
    private readonly IMenuService _menuService;
    private readonly AccessObjectInMemory _accessObject;

    public MenuServiceIntegrationTests()
    {
        _accessObject = new AccessObjectInMemory();
        _menuService = new MenuService(_accessObject.MenuRepository,
            NullLogger<MenuService>.Instance);
    }

    [Fact]
    public async Task GetMenuOkTest()
    {
        // Arrange
        var menu = _accessObject.CreateEmptyMockMenu();
        await _accessObject.InsertManyDishesAsync(menu);

        // Act
        var actualMenu = await _menuService.GetAllDishesAsync();

        // Asserts
        Assert.Equal(menu.Count, actualMenu.Count);
        Assert.Equal(menu, actualMenu);
    }

    [Fact]
    public async Task GetMenuEmptyOkTest()
    {
        // Arrange
        var menu = _accessObject.CreateEmptyMockMenu();
        await _accessObject.InsertManyDishesAsync(menu);

        // Act
        var actualMenu = await _menuService.GetAllDishesAsync();

        // Asserts
        Assert.Equal(menu.Count, actualMenu.Count);
        Assert.Equal(menu, actualMenu);
    }
    
    [Fact]
    public async Task GetDishOkTest()
    {
        // Arrange
        var menu = _accessObject.CreateMockMenu();
        await _accessObject.InsertManyDishesAsync(menu);

        var expectedCount = menu.Count;
        var expectedDish = menu.First();
        
        // Act
        var actualDish = await _menuService.GetDishByIdAsync(expectedDish.Id);
        var actualMenu = await _menuService.GetAllDishesAsync();
        var actualCount = actualMenu.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.Equal(expectedDish, actualDish);
    }
    
    [Fact]
    public async Task GetDishEmptyNotFoundTest()
    {
        // Arrange
        var menu = _accessObject.CreateEmptyMockMenu();
        var expectedCount = menu.Count;
        var dishId = Guid.NewGuid();
        
        // Act
        Task<Dish> Action() => _menuService.GetDishByIdAsync(dishId);
        
        var actualMenu = await _menuService.GetAllDishesAsync();
        var actualCount = actualMenu.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        await Assert.ThrowsAsync<DishNotFoundException>(Action);
    }
    
    [Fact]
    public async Task GetDishNotFoundTest()
    {
        // Arrange
        var menu = _accessObject.CreateMockMenu();
        
        await _accessObject.InsertManyDishesAsync(menu);
        
        var expectedCount = menu.Count;
        var dishId = Guid.NewGuid();
        
        // Act
        Task<Dish> Action() => _menuService.GetDishByIdAsync(dishId);
        
        var actualMenu = await _menuService.GetAllDishesAsync();
        var actualCount = actualMenu.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        await Assert.ThrowsAsync<DishNotFoundException>(Action);
    }
    
    [Fact]
    public async Task AddDishOkTest()
    {
        // Arrange
        var menu = _accessObject.CreateMockMenu();
        await _accessObject.InsertManyDishesAsync(menu);
        
        var expectedCount = menu.Count + 1;
        var expectedDish = _accessObject.CreateMockDish(Guid.NewGuid());
        
        // Act
        var actualDishId = await _menuService.AddDishAsync(expectedDish.Name, expectedDish.Type, 
            expectedDish.Price, expectedDish.Description);
        
        expectedDish.Id = actualDishId;
        
        var actualMenu = await _menuService.GetAllDishesAsync();
        var actualDish = actualMenu.FirstOrDefault(d => d.Id == actualDishId);
        var actualCount = actualMenu.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.Equal(expectedDish, actualDish);
    }
    
    [Fact]
    public async Task AddDishEmptyOkTest()
    {
        // Arrange
        var menu = _accessObject.CreateEmptyMockMenu();
        await _accessObject.InsertManyDishesAsync(menu);
        
        var expectedCount = menu.Count + 1;
        var expectedDish = _accessObject.CreateMockDish(Guid.NewGuid());
        
        // Act
        var actualDishId = await _menuService.AddDishAsync(expectedDish.Name, expectedDish.Type, 
            expectedDish.Price, expectedDish.Description);
        
        expectedDish.Id = actualDishId;
        
        var actualMenu = await _menuService.GetAllDishesAsync();
        var actualDish = actualMenu.FirstOrDefault(d => d.Id == actualDishId);
        var actualCount = actualMenu.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.Equal(expectedDish, actualDish);
    }
    
    [Fact]
    public async Task AddDishTest()
    {
        // Arrange
        var menu = _accessObject.CreateEmptyMockMenu();
        await _accessObject.InsertManyDishesAsync(menu);
        
        var expectedCount = menu.Count + 1;
        var expectedDish = _accessObject.CreateMockDish(Guid.NewGuid());
        
        // Act
        var actualDishId = await _menuService.AddDishAsync(expectedDish.Name, expectedDish.Type, 
            expectedDish.Price, expectedDish.Description);
        
        expectedDish.Id = actualDishId;
        
        var actualMenu = await _menuService.GetAllDishesAsync();
        var actualDish = actualMenu.FirstOrDefault(d => d.Id == actualDishId);
        var actualCount = actualMenu.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.Equal(expectedDish, actualDish);
    }
    
    [Fact]
    public async Task UpdateDishOkTest()
    {
        // Arrange
        var menu = _accessObject.CreateMockMenu();
        await _accessObject.InsertManyDishesAsync(menu);

        var updateDish = menu.First();
        updateDish.Name = "new name";
        var expectedCount = menu.Count;

        // Act
        await _menuService.UpdateDishAsync(updateDish);
        
        var actualMenu = await _menuService.GetAllDishesAsync();
        var actualDish = actualMenu.First(d => d.Id == updateDish.Id);
        var actualCount = actualMenu.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.Equal(updateDish, actualDish);
    }

    [Fact]
    public async Task UpdateDishNotFoundTest()
    {
        // Arrange
        var menu = _accessObject.CreateMockMenu();
        await _accessObject.InsertManyDishesAsync(menu);

        var dishId = Guid.NewGuid();
        var updateDish = _accessObject.CreateMockDish(dishId);
        var expectedCount = menu.Count;

        // Act
        Task Action() => _menuService.UpdateDishAsync(updateDish);

        var actualMenu = await _menuService.GetAllDishesAsync();
        var actualDish = actualMenu.FirstOrDefault(d => d.Id == updateDish.Id);
        var actualCount = actualMenu.Count;

        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.Null(actualDish);
        await Assert.ThrowsAsync<DishNotFoundException>(Action);
    }

    [Fact]
    public async Task RemoveDishOkTest()
    {
        // Arrange
        var menu = _accessObject.CreateMockMenu();
        await _accessObject.InsertManyDishesAsync(menu);
        
        var removeDish = menu.First();
        var expectedCount = menu.Count - 1;

        // Act
        await _menuService.RemoveDishAsync(removeDish.Id);

        var actualMenu = await _menuService.GetAllDishesAsync();
        var actualDish = actualMenu.FirstOrDefault(d => d.Id == removeDish.Id);
        var actualCount = actualMenu.Count;

        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.Null(actualDish);
    }
    
    [Fact]
    public async Task RemoveDishNotFoundTest()
    {
        // Arrange
        var menu = _accessObject.CreateEmptyMockMenu();
        await _accessObject.InsertManyDishesAsync(menu);
        
        var removeDishId = Guid.NewGuid();
        var expectedCount = menu.Count;

        // Act
        Task Action() => _menuService.RemoveDishAsync(removeDishId);

        var actualMenu = await _menuService.GetAllDishesAsync();
        var actualDish = actualMenu.FirstOrDefault(d => d.Id == removeDishId);
        var actualCount = actualMenu.Count;

        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.Null(actualDish);
        await Assert.ThrowsAsync<DishNotFoundException>(Action);
    }
    
    public void Dispose()
    {
        _accessObject.Dispose();
    }
}