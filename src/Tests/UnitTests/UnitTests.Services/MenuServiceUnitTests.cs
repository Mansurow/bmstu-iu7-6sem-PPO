using Portal.Database.Repositories.Interfaces;
using Portal.Services.MenuService;
using Xunit;
using Moq;
using Portal.Common.Models;
using Portal.Common.Models.Enums;
using Portal.Services.MenuService.Exceptions;

namespace UnitTests.Services;

public class MenuServiceUnitTests
{
    private readonly IMenuService _menuService;
    private readonly Mock<IMenuRepository> _mockMenuRepository = new();

    public MenuServiceUnitTests()
    {
        _menuService = new MenuService(_mockMenuRepository.Object);
    }

    [Fact]
    public async Task GetAllDishesOkTest()
    {
        // Arrange
        var menu = CreateMockMenu();

        _mockMenuRepository.Setup(s => s.GetAllDishesAsync())
                           .ReturnsAsync(menu);

        // Act
        var actualMenu = await _menuService.GetAllDishesAsync();
        
        // Assert
        Assert.Equal(menu, actualMenu);
    }

    [Fact]
    public async void GetAllDishesEmptyTest()
    {
        // Arrange
        var menu = CreateEmptyMockMenu();

        _mockMenuRepository.Setup(s => s.GetAllDishesAsync())
                           .ReturnsAsync(menu);

        // Act
        var actualMenu = await _menuService.GetAllDishesAsync();

        // Assert
        Assert.Equal(menu.Count, actualMenu.Count);
        Assert.Equal(menu, actualMenu);
    }

    [Fact]
    public async void GetDishByIdOkTest()
    {
        // Arrange
        var menu = CreateMockMenu();
        var ecpectedDish = menu.First();

        _mockMenuRepository.Setup(s => s.GetDishByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync((Guid id) => menu.Find(d => d.Id == id));

        // Act
        var actualDish = await _menuService.GetDishByIdAsync(ecpectedDish.Id);

        // Assert
        Assert.Equal(ecpectedDish, actualDish);
    }

    [Fact]
    public void GetDishByIdEmptyTest()
    {
        // Arrange
        var menu = CreateEmptyMockMenu();
        var dishId = Guid.NewGuid();
        //var ecpectedDish = menu.First();

        _mockMenuRepository.Setup(s => s.GetDishByIdAsync(dishId))
                           .ReturnsAsync(menu.Find(d => d.Id == dishId)!);

        // Act
        var action = async () => await _menuService.GetDishByIdAsync(dishId);

        // Assert
        Assert.ThrowsAsync<MenuNotFoundException>(action);
    }

    [Fact]
    public async void AddDishOkTest()
    {
        // Arrange
        var menu = CreateMockMenu();
        var dishId = Guid.NewGuid();
        var dish = CreateMockDish(dishId);

        _mockMenuRepository.Setup(s => s.GetAllDishesAsync())
                           .ReturnsAsync(menu);

        _mockMenuRepository.Setup(s => s.InsertDishAsync(It.IsAny<Dish>()))
                           .Callback((Dish d) => menu.Add(d));

        // Act
        await _menuService.AddDishAsync(dish.Name, dish.Type, dish.Price, dish.Description);
        var actualDish = menu.Last();

        // Assert
        Assert.Equal(actualDish.Name, dish.Name);
        Assert.Equal(actualDish.Type, dish.Type);
        Assert.Equal(actualDish.Price, dish.Price);
        Assert.Equal(actualDish.Description, dish.Description);
    }

    [Fact]
    public async void UpdateDishOkTest()
    {
        // Arrange
        var menu = CreateMockMenu();
        var dishId = menu[^1].Id;
        var expectedDish = CreateMockDish(dishId);

        _mockMenuRepository.Setup(s => s.GetDishByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync((Guid id) => menu.Find(d => d.Id == id));

        _mockMenuRepository.Setup(s => s.UpdateDishAsync(It.IsAny<Dish>()))
                           .Callback((Dish d) =>
                           {
                               menu.FindAll(e => e.Id == d.Id).ForEach
                               (e =>
                               {
                                   e.Name = d.Name;
                                   e.Type = d.Type;
                                   e.Price = d.Price;
                                   e.Description = d.Description;
                               });

                           });

        // Act
        await _menuService.UpdateDishAsync(expectedDish);
        var actualDish = menu.Last();

        // Assert
        Assert.Equal(expectedDish.Id, actualDish.Id);
        Assert.Equal(expectedDish.Name, actualDish.Name);
        Assert.Equal(expectedDish.Type, actualDish.Type);
        Assert.Equal(expectedDish.Price, actualDish.Price);
        Assert.Equal(expectedDish.Description, actualDish.Description);

    }

    [Fact]
    public void UpdateDishNotExistsTest()
    {
        // Arrange
        var menu = CreateMockMenu();
        var dishId = Guid.NewGuid();
        var expectedDish = CreateMockDish(dishId);

        _mockMenuRepository.Setup(s => s.GetDishByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync((Guid id) => menu.Find(d => d.Id == id));

        _mockMenuRepository.Setup(s => s.UpdateDishAsync(It.IsAny<Dish>()))
                           .Callback((Dish d) =>
                           {
                               menu.FindAll(e => e.Id == d.Id).ForEach
                               (e =>
                               {
                                   e.Name = d.Name;
                                   e.Type = d.Type;
                                   e.Price = d.Price;
                                   e.Description = d.Description;
                               });

                           });

        // Act
        var action = async () => await _menuService.UpdateDishAsync(expectedDish);

        // Assert
        Assert.ThrowsAsync<MenuNotFoundException>(action);
    }

    [Fact]
    public void UpdateDishEmptyTest()
    {
        // Arrange
        var menu = CreateEmptyMockMenu();
        var dishId = Guid.NewGuid();
        var expectedDish = CreateMockDish(dishId);

        _mockMenuRepository.Setup(s => s.GetDishByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync((Guid id) => menu.Find(d => d.Id == id));

        _mockMenuRepository.Setup(s => s.UpdateDishAsync(It.IsAny<Dish>()))
                           .Callback((Dish d) =>
                           {
                               menu.FindAll(e => e.Id == d.Id).ForEach
                               (e =>
                               {
                                   e.Name = d.Name;
                                   e.Type = d.Type;
                                   e.Price = d.Price;
                                   e.Description = d.Description;
                               });

                           });

        // Act
        var action = async () => await _menuService.UpdateDishAsync(expectedDish);

        // Assert
        Assert.ThrowsAsync<MenuNotFoundException>(action);
    }

    [Fact]
    public async void DeleteDishOkTest()
    {
        // Arrange
        var menu = CreateMockMenu();
        var dishId = menu[0].Id;
        var excpectedCount = menu.Count - 1;
        _mockMenuRepository.Setup(s => s.GetDishByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync(menu.Find(m => m.Id == dishId));

        _mockMenuRepository.Setup(s => s.DeleteDishAsync(It.IsAny<Guid>()))
                           .Callback((Guid id) =>
                           {
                               var dish = menu.Find(m => m.Id == id);
                               menu.Remove(dish!);
                           });

        // Act
        await _menuService.DeleteDishAsync(dishId);
        var actualCount = menu.Count;

        // Assert
        Assert.Equal(excpectedCount, actualCount);
    }

    [Fact]
    public void DeleteDishNotExistsTest()
    {
        // Arrange
        var menu = CreateMockMenu();
        var dishId = Guid.NewGuid();

        _mockMenuRepository.Setup(s => s.GetDishByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync(menu.Find(m => m.Id == dishId));

        _mockMenuRepository.Setup(s => s.DeleteDishAsync(It.IsAny<Guid>()))
                           .Callback((Guid id) =>
                           {
                               var dish = menu.Find(m => m.Id == id);
                               menu.Remove(dish!);
                           });

        // Act
        var action = async () => await _menuService.DeleteDishAsync(dishId); ;

        // Assert
        Assert.ThrowsAsync<MenuNotFoundException>(action);
    }

    [Fact]
    public void DeleteDishEmptyTest()
    {
        // Arrange
        var menu = CreateEmptyMockMenu();
        var dishId = Guid.NewGuid();

        _mockMenuRepository.Setup(s => s.GetDishByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync(menu.Find(m => m.Id == dishId));

        _mockMenuRepository.Setup(s => s.DeleteDishAsync(It.IsAny<Guid>()))
                           .Callback((Guid id) =>
                           {
                               var dish = menu.Find(m => m.Id == id);
                               menu.Remove(dish!);
                           });

        // Act
        var action = async () => await _menuService.DeleteDishAsync(dishId); ;

        // Assert
        Assert.ThrowsAsync<MenuNotFoundException>(action);
    }

    private Dish CreateMockDish(Guid id)
    {
        return new Dish(id, "name+1", DishType.Salat, 130, "bigfoot");
    }

    private List<Dish> CreateMockMenu()
    {
        return new List<Dish>()
        {
            new Dish(Guid.NewGuid(), "Dish1", DishType.FirstCourse, 350, "description 1"),
            new Dish(Guid.NewGuid(), "Dish2", DishType.SecondCourse, 250, "description 2"),
            new Dish(Guid.NewGuid(), "Dish3", DishType.FirstCourse, 120, "description 3")
        };
    }

    private List<Dish> CreateEmptyMockMenu()
    {
        return new List<Dish>();
    }
}
