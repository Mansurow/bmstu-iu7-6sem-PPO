using Anticafe.BL.Enums;
using Anticafe.BL.Exceptions;
using Anticafe.BL.IRepositories;
using Anticafe.BL.Models;
using Anticafe.BL.Sevices.RoomService;
using Moq;
using Xunit;

namespace UnitTests.Service;

public class RoomServiceUnitTests
{
    private readonly IRoomService _roomService;
    private readonly Mock<IRoomRepository> _mockRoomRepository = new();


    public RoomServiceUnitTests() 
    {
        _roomService = new RoomService(_mockRoomRepository.Object);
    }

    [Fact]
    public async void GetAllRoomsTest() 
    {
        // Arange
        var expectedRooms = CreateMockRooms();

        _mockRoomRepository.Setup(s => s.GetAllRoomAsync())
                           .ReturnsAsync(expectedRooms);

        // Act
        var actualRooms = await _roomService.GetAllRoomsAsync();

        // Assert
        Assert.Equal(expectedRooms, actualRooms);
    }

    [Fact]
    public async void GetAllRoomsEmptyTest()
    {
        // Arange
        var expectedRooms = new List<Room>();

        _mockRoomRepository.Setup(s => s.GetAllRoomAsync())
                           .ReturnsAsync(expectedRooms);

        // Act
        var actualRooms = await _roomService.GetAllRoomsAsync();

        // Assert
        Assert.Equal(expectedRooms, actualRooms);
    }

    [Fact]
    public async void GetRoomByIdTest()
    {
        // Arange
        var roomId = 1;
        var rooms = CreateMockRooms();
        var expectedRoom = rooms[0];

        _mockRoomRepository.Setup(s => s.GetRoomByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(expectedRoom);

        // Act
        var actualRoom = await _roomService.GetRoomByIdAsync(roomId);

        // Assert
        Assert.Equal(expectedRoom, actualRoom);
    }

    [Fact]
    public void GetRoomByIdNotFoundTest()
    {
        // Arange
        var roomId = 100;

        // Act
        Task action() => _roomService.GetRoomByIdAsync(roomId);

        // Assert
        Assert.ThrowsAsync<RoomNotFoundException>(action);
    }

    [Fact]
    public async void AddRoomTest()
    {
        // Arange
        var rooms = CreateMockRooms();
        var expectedCountRooms = rooms.Count() + 1;
        var room = new Room(rooms.Count() + 1, 
                           "Веселье", 
                           30, 
                           4500,
                           0, 
                           null, 
                           null);

        _mockRoomRepository.Setup(s => s.GetAllRoomAsync())
                           .ReturnsAsync(rooms);

        _mockRoomRepository.Setup(s => s.GetRoomByNameAsync(It.IsAny<String>()))
                           .ReturnsAsync(rooms.Find(e => e.Name == room.Name));

        _mockRoomRepository.Setup(s => s.InsertRoomAsync(It.IsAny<Room>()))
                           .Callback((Room r) =>
                           {
                               r.Id = rooms.Count + 1;
                               rooms.Add(r);
                           });
                           
        // Act
        await _roomService.AddRoomAsync(room.Name,
                                        room.Size,
                                        room.Price);
        var actualCountRooms = rooms.Count();
        var actualRoom = rooms.Last();

        // Assert
        Assert.Equal(expectedCountRooms, actualCountRooms);
        Assert.Equal(room.Id, actualRoom.Id);
        Assert.Equal(room.Name, actualRoom.Name);
        Assert.Equal(room.Size, actualRoom.Size);
        Assert.Equal(room.Price, actualRoom.Price);
        Assert.Equal(room.Rating, actualRoom.Rating);
        Assert.Equal(room.Inventories, actualRoom.Inventories);
        Assert.Equal(room.Menu, actualRoom.Menu);
    }

    [Fact]
    public void AddRoomSameNameTest()
    {
        // Arange
        var rooms = CreateMockRooms();
        var expectedCountRooms = rooms.Count();
        var room = new Room(rooms.Count() + 1,
                           "Room3",
                           30,
                           4500,
                           0,
                           null,
                           null);

        _mockRoomRepository.Setup(s => s.GetAllRoomAsync())
                           .ReturnsAsync(rooms);

        _mockRoomRepository.Setup(s => s.GetRoomByNameAsync(It.IsAny<String>()))
                           .ReturnsAsync(rooms.Find(e => e.Name == room.Name));

        _mockRoomRepository.Setup(s => s.InsertRoomAsync(It.IsAny<Room>()))
                           .Callback((Room r) =>
                           {
                               r.Id = rooms.Count + 1;
                               rooms.Add(r);
                           });

        // Act
        Task action() => _roomService.AddRoomAsync(room.Name,
                                        room.Size,
                                        room.Price);
        var actualCountRooms = rooms.Count();

        // Assert
        Assert.ThrowsAsync<RoomNameExistException>(action);
        Assert.Equal(expectedCountRooms, actualCountRooms);
    }

    [Fact]
    public async void AddFisrtRoomTest()
    {
        // Arange
        var rooms = new List<Room>();
        var expectedCountRooms = 1;
        var room = new Room(rooms.Count() + 1,
                           "Room",
                           30,
                           4500,
                           0,
                           null,
                           null);

        _mockRoomRepository.Setup(s => s.GetAllRoomAsync())
                           .ReturnsAsync(rooms);

        _mockRoomRepository.Setup(s => s.GetRoomByNameAsync(It.IsAny<string>()))
                           .ReturnsAsync(rooms.Find(e => e.Name == room.Name));

        _mockRoomRepository.Setup(s => s.InsertRoomAsync(It.IsAny<Room>()))
                           .Callback((Room r) =>
                           {
                               r.Id = rooms.Count + 1;
                               rooms.Add(r);
                           });

        // Act
        await _roomService.AddRoomAsync(room.Name,
                                        room.Size,
                                        room.Price);
        var actualCountRooms = rooms.Count();
        var actualRoom = rooms.Last();

        // Assert
        Assert.Equal(expectedCountRooms, actualCountRooms);
        Assert.Equal(room.Id, actualRoom.Id);
        Assert.Equal(room.Name, actualRoom.Name);
        Assert.Equal(room.Size, actualRoom.Size);
        Assert.Equal(room.Price, actualRoom.Price);
        Assert.Equal(room.Rating, actualRoom.Rating);
        Assert.Equal(room.Inventories, actualRoom.Inventories);
        Assert.Equal(room.Menu, actualRoom.Menu);
    }

    [Fact]
    public async void UpdateRoomTest()
    {
        // Arange
        var rooms = CreateMockRooms();
        var expectedCountRooms = rooms.Count();
        var updateRoom = new Room(1,
                           "Room1",
                           40,
                           3500,
                           5.0,
                           null,
                           null);

        _mockRoomRepository.Setup(s => s.GetRoomByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(rooms.Find(e => e.Id == updateRoom.Id));

        _mockRoomRepository.Setup(s => s.GetRoomByNameAsync(It.IsAny<string>()))
                           .ReturnsAsync(rooms.Find(e => e.Name == updateRoom.Name));

        _mockRoomRepository.Setup(s => s.UpdateRoomAsync(It.IsAny<Room>()))
                           .Callback((Room r) =>
                           {
                               rooms.FindAll(e => e.Id == r.Id).ForEach
                               ( e => 
                               { 
                                   e.Name = r.Name;
                                   e.Size = r.Size;
                                   e.Price = r.Price;
                                   e.Rating = r.Rating;
                                   e.Inventories = r.Inventories;
                                   e.Menu = r.Menu;
                               });
                           });

        // Act
        await _roomService.UpdateRoomAsync(updateRoom);
        var actualCountRooms = rooms.Count();
        var actualRoom = rooms.Find(e => e.Id == updateRoom.Id);

        // Assert
        Assert.Equal(expectedCountRooms, actualCountRooms);
        Assert.Equal(updateRoom.Id, actualRoom.Id);
        Assert.Equal(updateRoom.Name, actualRoom.Name);
        Assert.Equal(updateRoom.Size, actualRoom.Size);
        Assert.Equal(updateRoom.Price, actualRoom.Price);
        Assert.Equal(updateRoom.Rating, actualRoom.Rating);
        Assert.Equal(updateRoom.Inventories, actualRoom.Inventories);
        Assert.Equal(updateRoom.Menu, actualRoom.Menu);
    }

    [Fact]
    public void UpdateRoomNameExistTest()
    {
        // Arange
        var rooms = CreateMockRooms();
        var updateRoom = new Room(1,
                           "Room3",
                           40,
                           3500,
                           5.0,
                           null,
                           null);

        _mockRoomRepository.Setup(s => s.GetRoomByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(rooms.Find(e => e.Id == updateRoom.Id));

        _mockRoomRepository.Setup(s => s.GetRoomByNameAsync(It.IsAny<string>()))
                           .ReturnsAsync(rooms.Find(e => e.Name == updateRoom.Name));

        // Act
        var action = async () => await _roomService.UpdateRoomAsync(updateRoom);
        
        // Assert
        Assert.ThrowsAsync<RoomNameExistException>(action);
        
    }

    [Fact]
    public void UpdateNoRoomsTest()
    {
        // Arange
        var rooms = new List<Room>();
        var updateRoom = new Room(1,
                           "Room1",
                           40,
                           3500,
                           5.0,
                           null,
                           null);


        // Act
        var action = async () => await _roomService.UpdateRoomAsync(updateRoom);

        // Assert
        Assert.ThrowsAsync<RoomNotFoundException>(action);
    }

    [Fact]
    public async void AddInventoryForRoomTest()
    {
        // Arange
        var rooms = CreateMockRooms();
        var newInventory = new Inventory(4, "Wifi Адаптер");
        var inventories = CreateMockInventory();
        inventories.Add(newInventory);

        _mockRoomRepository.Setup(s => s.GetRoomByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(rooms.Find(e => e.Id == rooms[3].Id));

        _mockRoomRepository.Setup(s => s.UpdateRoomAsync(It.IsAny<Room>()))
                           .Callback((Room r) =>
                           {
                               rooms.FindAll(e => e.Id == r.Id).ForEach
                               (e =>
                               {
                                   e.Name = r.Name;
                                   e.Size = r.Size;
                                   e.Price = r.Price;
                                   e.Rating = r.Rating;
                                   e.Inventories = r.Inventories;
                                   e.Menu = r.Menu;
                               });
                           });

        // Act
        await _roomService.AddInventoryForRoomAsync(rooms[3].Id, newInventory);
        var actualInventories = rooms[3].Inventories;

        // Assert
        Assert.Equal(inventories.Count(), actualInventories.Count());
        Assert.Equal(inventories.Last(), actualInventories.Last());
    }

    [Fact]
    public async void AddInventoryForRoomWithoutInventoriesTest()
    {
        // Arange
        var rooms = CreateMockRooms();
        var newInventory = new Inventory(4, "Wifi Адаптер");
        var inventories = new List<Inventory> { newInventory };

        _mockRoomRepository.Setup(s => s.GetRoomByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(rooms.Find(e => e.Id == rooms[0].Id));

        _mockRoomRepository.Setup(s => s.UpdateRoomAsync(It.IsAny<Room>()))
                           .Callback((Room r) =>
                           {
                               rooms.FindAll(e => e.Id == r.Id).ForEach
                               (e =>
                               {
                                   e.Name = r.Name;
                                   e.Size = r.Size;
                                   e.Price = r.Price;
                                   e.Rating = r.Rating;
                                   e.Inventories = r.Inventories;
                                   e.Menu = r.Menu;
                               });
                           });

        // Act
        await _roomService.AddInventoryForRoomAsync(rooms[0].Id, newInventory);
        var actualInventories = rooms[0].Inventories;

        // Assert
        Assert.Equal(inventories.Count(), actualInventories.Count());
        Assert.Equal(inventories.Last(), actualInventories.Last());
    }

    [Fact]
    public void AddInventoryForNoExistRoomTest()
    {
        // Arange
        var roomId = 100;
        var rooms = CreateMockRooms();
        var newInventory = new Inventory(4, "Wifi Адаптер");
        var inventories = new List<Inventory> { newInventory };

        _mockRoomRepository.Setup(s => s.GetRoomByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(rooms.Find(e => e.Id == roomId));

        
        // Act
        var action = async () => await _roomService.AddInventoryForRoomAsync(rooms[0].Id, newInventory);

        // Assert
        Assert.ThrowsAsync<RoomNotFoundException>(action);
    }

    [Fact]
    public async void AddMenuForRoomTest()
    {
        // Arange
        var rooms = CreateMockRooms();
        var newMenu = new Menu(4, "Dish4", DishType.FirstCourse, 400, "descreption 4");
        var menu = CreateMockMenu();
        menu.Add(newMenu);

        _mockRoomRepository.Setup(s => s.GetRoomByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(rooms.Find(e => e.Id == rooms[3].Id));

        _mockRoomRepository.Setup(s => s.UpdateRoomAsync(It.IsAny<Room>()))
                           .Callback((Room r) =>
                           {
                               rooms.FindAll(e => e.Id == r.Id).ForEach
                               (e =>
                               {
                                   e.Name = r.Name;
                                   e.Size = r.Size;
                                   e.Price = r.Price;
                                   e.Rating = r.Rating;
                                   e.Inventories = r.Inventories;
                                   e.Menu = r.Menu;
                               });
                           });

        // Act
        await _roomService.AddMenuForRoomAsync(rooms[3].Id, newMenu);
        var actualMenu = rooms[3].Menu;

        // Assert
        Assert.Equal(menu.Count(), actualMenu.Count());
        Assert.Equal(menu.Last(), actualMenu.Last());
    }

    [Fact]
    public async void AddMenuForRoomWithoutMenuTest()
    {
        // Arange
        var rooms = CreateMockRooms();
        var newMenu = new Menu(4, "Dish4", DishType.FirstCourse, 400, "descreption 4");
        var menu = new List<Menu> { newMenu };

        _mockRoomRepository.Setup(s => s.GetRoomByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(rooms.Find(e => e.Id == rooms[0].Id));

        _mockRoomRepository.Setup(s => s.UpdateRoomAsync(It.IsAny<Room>()))
                           .Callback((Room r) =>
                           {
                               rooms.FindAll(e => e.Id == r.Id).ForEach
                               (e =>
                               {
                                   e.Name = r.Name;
                                   e.Size = r.Size;
                                   e.Price = r.Price;
                                   e.Rating = r.Rating;
                                   e.Inventories = r.Inventories;
                                   e.Menu = r.Menu;
                               });
                           });

        // Act
        await _roomService.AddMenuForRoomAsync(rooms[0].Id, newMenu);
        var actualMenu = rooms[0].Menu;

        // Assert
        Assert.Equal(menu.Count(), actualMenu.Count());
        Assert.Equal(menu.Last(), actualMenu.Last());
    }

    [Fact]
    public void AddMenuForNoExistRoomTest()
    {
        // Arange
        var roomId = 100;
        var rooms = CreateMockRooms();
        var newMenu = new Menu(4, "Dish4", DishType.FirstCourse, 400, "descreption 4");
        var menu = new List<Menu> { newMenu };

        _mockRoomRepository.Setup(s => s.GetRoomByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(rooms.Find(e => e.Id == roomId));

        // Act
        var action = async () => await _roomService.AddMenuForRoomAsync(rooms[0].Id, newMenu);

        // Assert
        Assert.ThrowsAsync<RoomNotFoundException>(action);
    }

    [Fact]
    public async void DeleteRoomTest() 
    {
        // Arrange
        var roomId = 1;
        var rooms = CreateMockRooms();
        var expectedCountRooms = rooms.Count() - 1;

        _mockRoomRepository.Setup(s => s.GetRoomByIdAsync(roomId))
                           .ReturnsAsync(rooms[0]);

        _mockRoomRepository.Setup(s => s.DeleteRoomAsync(It.IsAny<int>()))
                           .Callback((int id) => rooms.RemoveAll(e => e.Id == id));

        // Act

        await _roomService.DeleteRoomAsync(roomId);
        var actualCountRooms = rooms.Count();

        // Assert
        Assert.Equal(expectedCountRooms, actualCountRooms);
    }

    [Fact]
    public void DeleteNoExistRoomTest()
    {
        // Arrange
        var roomId = 100;
        var rooms = CreateMockRooms();
        var expectedCountRooms = rooms.Count() - 1;

        // Act

        var action = async () => await _roomService.DeleteRoomAsync(roomId);
       
        // Assert
        Assert.ThrowsAsync<RoomNotFoundException>(action);
    }

    private List<Room> CreateMockRooms() 
    {
        return new List<Room>
        {
            new Room(1, "Room1", 20, 2500, 4.5, null, null),
            new Room(2, "Room2", 30, 3500, 5.0, null, null),
            new Room(3, "Room3", 25, 3000, 3.0, null, null),
            new Room(4, "Room4", 25, 1300, 5.0, 
                     CreateMockInventory(),
                     CreateMockMenu()),
        };
    }

    private List<Inventory> CreateMockInventory() 
    {
        return new List<Inventory>()
        {
            new Inventory(1, "Подушка"),
            new Inventory(2, "Телевизор"),
            new Inventory(3, "PS5")
        };
    }

    private List<Menu> CreateMockMenu()
    {
        return new List<Menu>()
        {
            new Menu(1, "Dish1", DishType.FirstCourse, 350, "description 1"),
            new Menu(2, "Dish2", DishType.SecondCourse, 250, "description 2"),
            new Menu(3, "Dish3", DishType.FirstCourse, 120, "description 3")
        };
    }
}