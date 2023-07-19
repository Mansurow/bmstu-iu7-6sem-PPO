using Anticafe.BL.Exceptions;
using Anticafe.BL.Models;
using Anticafe.BL.Services.MenuService;
using Anticafe.Common.Enums;
using Anticafe.DataAccess.Converter;
using Anticafe.DataAccess.DBModels;
using Anticafe.DataAccess.IRepositories;
using Moq;
using Xunit;

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
    public async Task GetAllDishesTest()
    {
        // Arrange
        var menu = CreateMockMenu();

        _mockMenuRepository.Setup(s => s.GetAllDishesAsync())
                           .ReturnsAsync(menu);

        // Act
        var getMenu = await _menuService.GetAllDishesAsync();
        var actualMenu = getMenu.Select(d => MenuConverter.ConvertAppModelToDbModel(d)).ToList();

        // Assert
        Assert.Equal(menu.Count, actualMenu.Count);
        // Assert.Equal(menu, actualMenu);
    }

    [Fact]
    public async void GetAllDishesEmptyTest()
    {
        // Arrange
        var menu = new List<MenuDbModel>();

        _mockMenuRepository.Setup(s => s.GetAllDishesAsync())
                           .ReturnsAsync(menu);

        // Act
        var getMenu = await _menuService.GetAllDishesAsync();
        var actualMenu = getMenu.Select(d => MenuConverter.ConvertAppModelToDbModel(d)).ToList();

        // Assert
        Assert.Equal(menu.Count, actualMenu.Count);
        Assert.Equal(menu, actualMenu);
    }

    [Fact]
    public async void GetDishByIdTest()
    {
        // Arrange
        var menu = CreateMockMenu();
        var menuId = 1;
        var ecpectedDish = menu.First();

        _mockMenuRepository.Setup(s => s.GetDishByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync((int id) => menu.Find(d => d.Id == id));

        // Act
        var actualDish = await _menuService.GetDishByIdAsync(menuId);

        // Assert
        Assert.Equal(ecpectedDish.Id, actualDish.Id);
        Assert.Equal(ecpectedDish.Name, actualDish.Name);
        Assert.Equal(ecpectedDish.Type, actualDish.Type);
        Assert.Equal(ecpectedDish.Price, actualDish.Price);
        Assert.Equal(ecpectedDish.Description, actualDish.Description);
    }

    [Fact]
    public void GetDishByIdEmptyTest()
    {
        // Arrange
        var menu = new List<MenuDbModel>();
        var menuId = 1;
        //var ecpectedDish = menu.First();

        _mockMenuRepository.Setup(s => s.GetDishByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync((int id) => menu.Find(d => d.Id == id));

        // Act
        var action = async () => await _menuService.GetDishByIdAsync(menuId);

        // Assert
        Assert.ThrowsAsync<MenuNotFoundException>(action);
    }

    [Fact]
    public async void AddDishOkTest()
    {
        // Arrange
        var menu = CreateMockMenu();
        var dish = new Menu(4, "name+1", DishType.Salat, 130, "bigfoot");

        _mockMenuRepository.Setup(s => s.GetAllDishesAsync())
                           .ReturnsAsync(menu);

        _mockMenuRepository.Setup(s => s.InsertDishAsync(It.IsAny<MenuDbModel>()))
                           .Callback((MenuDbModel d) => menu.Add(d));

        // Act
        await _menuService.AddDishAsync(dish.Name, dish.Type, dish.Price, dish.Description);
        var actualDish = menu.Last();

        // Assert
        Assert.Equal(actualDish.Id, dish.Id);
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
        var expectedDish = new Menu(3, "name+1", DishType.Salat, 130, "bigfoot");

        _mockMenuRepository.Setup(s => s.GetDishByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync((int id) => menu.Find(d => d.Id == id));

        _mockMenuRepository.Setup(s => s.UpdateDishAsync(It.IsAny<MenuDbModel>()))
                           .Callback((MenuDbModel d) =>
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
        var expectedDish = new Menu(4, "name+1", DishType.Salat, 130, "bigfoot");

        _mockMenuRepository.Setup(s => s.GetDishByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync((int id) => menu.Find(d => d.Id == id));

        _mockMenuRepository.Setup(s => s.UpdateDishAsync(It.IsAny<MenuDbModel>()))
                           .Callback((MenuDbModel d) =>
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
        var menu = new List<MenuDbModel>();
        var expectedDish = new Menu(4, "name+1", DishType.Salat, 130, "bigfoot");

        _mockMenuRepository.Setup(s => s.GetDishByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync((int id) => menu.Find(d => d.Id == id));

        _mockMenuRepository.Setup(s => s.UpdateDishAsync(It.IsAny<MenuDbModel>()))
                           .Callback((MenuDbModel d) =>
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
        var dishId = 3;
        var excpectedCount = menu.Count - 1;
        _mockMenuRepository.Setup(s => s.GetDishByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(menu.Find(m => m.Id == dishId));

        _mockMenuRepository.Setup(s => s.DeleteDishAsync(It.IsAny<int>()))
                           .Callback((int id) => 
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
        var dishId = 13;

        _mockMenuRepository.Setup(s => s.GetDishByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(menu.Find(m => m.Id == dishId));

        _mockMenuRepository.Setup(s => s.DeleteDishAsync(It.IsAny<int>()))
                           .Callback((int id) =>
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
        var menu = new List<MenuDbModel>();
        var dishId = 1;

        _mockMenuRepository.Setup(s => s.GetDishByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(menu.Find(m => m.Id == dishId));

        _mockMenuRepository.Setup(s => s.DeleteDishAsync(It.IsAny<int>()))
                           .Callback((int id) =>
                           {
                               var dish = menu.Find(m => m.Id == id);
                               menu.Remove(dish!);
                           });

        // Act
        var action = async () => await _menuService.DeleteDishAsync(dishId); ;

        // Assert
        Assert.ThrowsAsync<MenuNotFoundException>(action);
    }

    private List<MenuDbModel> CreateMockMenu()
    {
        return new List<MenuDbModel>()
        {
            new MenuDbModel(1, "Dish1", DishType.FirstCourse, 350, "description 1"),
            new MenuDbModel(2, "Dish2", DishType.SecondCourse, 250, "description 2"),
            new MenuDbModel(3, "Dish3", DishType.FirstCourse, 120, "description 3")
        };
    }
}
