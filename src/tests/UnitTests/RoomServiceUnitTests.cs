using Anticafe.BL.Exceptions;
using Anticafe.BL.Models;
using Anticafe.BL.Sevices.RoomService;
using Anticafe.DataAccess.Converter;
using Anticafe.DataAccess.DBModels;
using Anticafe.DataAccess.IRepositories;
using Moq;
using Xunit;

namespace UnitTests.Services;

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
        var getRooms = await _roomService.GetAllRoomsAsync();
        var actualRooms = getRooms.Select(r => RoomConverter.ConvertAppModelToDbModel(r)).ToList();

        // Assert
        Assert.Equal(expectedRooms.Count, actualRooms.Count);
    }

    [Fact]
    public async void GetAllRoomsEmptyTest()
    {
        // Arange
        var expectedRooms = new List<RoomDbModel>();

        _mockRoomRepository.Setup(s => s.GetAllRoomAsync())
                           .ReturnsAsync(expectedRooms);

        // Act
        var getRooms = await _roomService.GetAllRoomsAsync();
        var actualRooms = getRooms.Select(r => RoomConverter.ConvertAppModelToDbModel(r)).ToList();

        // Assert
        Assert.Equal(expectedRooms, actualRooms);
    }

    [Fact]
    public async void GetRoomByIdTest()
    {
        // Arange
        var roomId = 1;
        var rooms = CreateMockRooms();
        var expectedRoom = RoomConverter.ConvertDbModelToAppModel(rooms[0]);

        _mockRoomRepository.Setup(s => s.GetRoomByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(rooms.Find(e => e.Id == roomId));

        // Act
        var actualRoom = await _roomService.GetRoomByIdAsync(roomId);

        // Assert
        Assert.Equal(expectedRoom.Id, actualRoom.Id);
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
                           null);

        _mockRoomRepository.Setup(s => s.GetAllRoomAsync())
                           .ReturnsAsync(rooms);

        _mockRoomRepository.Setup(s => s.GetRoomByNameAsync(It.IsAny<String>()))
                           .ReturnsAsync(rooms.Find(e => e.Name == room.Name));

        _mockRoomRepository.Setup(s => s.InsertRoomAsync(It.IsAny<RoomDbModel>()))
                           .Callback((RoomDbModel r) =>
                           {
                               r.Id = rooms.Count + 1;
                               rooms.Add(r);
                           });
                           
        // Act
        await _roomService.AddRoomAsync(room.Name,
                                        room.Size,
                                        room.Price);
        var actualCountRooms = rooms.Count();
        var actualRoom = RoomConverter.ConvertDbModelToAppModel(rooms.Last());

        // Assert
        Assert.Equal(expectedCountRooms, actualCountRooms);
        Assert.Equal(room.Id, actualRoom.Id);
        Assert.Equal(room.Name, actualRoom.Name);
        Assert.Equal(room.Size, actualRoom.Size);
        Assert.Equal(room.Price, actualRoom.Price);
        Assert.Equal(room.Rating, actualRoom.Rating);
        Assert.Equal(room.Inventories, actualRoom.Inventories);
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
                           null);

        _mockRoomRepository.Setup(s => s.GetAllRoomAsync())
                           .ReturnsAsync(rooms);

        _mockRoomRepository.Setup(s => s.GetRoomByNameAsync(It.IsAny<String>()))
                           .ReturnsAsync(rooms.Find(e => e.Name == room.Name));

        _mockRoomRepository.Setup(s => s.InsertRoomAsync(It.IsAny<RoomDbModel>()))
                           .Callback((RoomDbModel r) =>
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
        var rooms = new List<RoomDbModel>();
        var expectedCountRooms = 1;
        var room = new Room(rooms.Count() + 1,
                           "Room",
                           30,
                           4500,
                           0,
                           null);

        _mockRoomRepository.Setup(s => s.GetAllRoomAsync())
                           .ReturnsAsync(rooms);

        _mockRoomRepository.Setup(s => s.GetRoomByNameAsync(It.IsAny<string>()))
                           .ReturnsAsync(rooms.Find(e => e.Name == room.Name));

        _mockRoomRepository.Setup(s => s.InsertRoomAsync(It.IsAny<RoomDbModel>()))
                           .Callback((RoomDbModel r) =>
                           {
                               r.Id = rooms.Count + 1;
                               rooms.Add(r);
                           });

        // Act
        await _roomService.AddRoomAsync(room.Name,
                                        room.Size,
                                        room.Price);
        var actualCountRooms = rooms.Count();
        var actualRoom = RoomConverter.ConvertDbModelToAppModel(rooms.Last());

        // Assert
        Assert.Equal(expectedCountRooms, actualCountRooms);
        Assert.Equal(room.Id, actualRoom.Id);
        Assert.Equal(room.Name, actualRoom.Name);
        Assert.Equal(room.Size, actualRoom.Size);
        Assert.Equal(room.Price, actualRoom.Price);
        Assert.Equal(room.Rating, actualRoom.Rating);
        Assert.Equal(room.Inventories, actualRoom.Inventories);
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
                           null);

        _mockRoomRepository.Setup(s => s.GetRoomByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(rooms.Find(e => e.Id == updateRoom.Id));

        _mockRoomRepository.Setup(s => s.GetRoomByNameAsync(It.IsAny<string>()))
                           .ReturnsAsync(rooms.Find(e => e.Name == updateRoom.Name));

        _mockRoomRepository.Setup(s => s.UpdateRoomAsync(It.IsAny<RoomDbModel>()))
                           .Callback((RoomDbModel r) =>
                           {
                               rooms.FindAll(e => e.Id == r.Id).ForEach
                               ( e => 
                               { 
                                   e.Name = r.Name;
                                   e.Size = r.Size;
                                   e.Price = r.Price;
                                   e.Rating = r.Rating;
                                   e.Inventories = r.Inventories;
                               });
                           });

        // Act
        await _roomService.UpdateRoomAsync(updateRoom);
        var actualCountRooms = rooms.Count();
        var actualRoom = RoomConverter.ConvertDbModelToAppModel(rooms.Find(e => e.Id == updateRoom.Id));

        // Assert
        Assert.Equal(expectedCountRooms, actualCountRooms);
        Assert.Equal(updateRoom.Id, actualRoom.Id);
        Assert.Equal(updateRoom.Name, actualRoom.Name);
        Assert.Equal(updateRoom.Size, actualRoom.Size);
        Assert.Equal(updateRoom.Price, actualRoom.Price);
        Assert.Equal(updateRoom.Rating, actualRoom.Rating);
        Assert.Equal(updateRoom.Inventories, actualRoom.Inventories);
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

        _mockRoomRepository.Setup(s => s.GetRoomByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(rooms.Find(e => e.Id == rooms[3].Id));

        _mockRoomRepository.Setup(s => s.UpdateRoomAsync(It.IsAny<RoomDbModel>()))
                           .Callback((RoomDbModel r) =>
                           {
                               rooms.FindAll(e => e.Id == r.Id).ForEach
                               (e =>
                               {
                                   e.Name = r.Name;
                                   e.Size = r.Size;
                                   e.Price = r.Price;
                                   e.Rating = r.Rating;
                                   e.Inventories = r.Inventories;
                               });
                           });

        // Act
        await _roomService.AddInventoryForRoomAsync(rooms[3].Id, newInventory);
        var actualInventories = rooms[3].Inventories;
        var actual = actualInventories?.Last();

        // Assert
        Assert.Equal(inventories.Count() + 1, actualInventories?.Count());
        Assert.Equal(newInventory.Id, actual?.Id);
        Assert.Equal(newInventory.Name, actual?.Name);
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

        _mockRoomRepository.Setup(s => s.UpdateRoomAsync(It.IsAny<RoomDbModel>()))
                           .Callback((RoomDbModel r) =>
                           {
                               rooms.FindAll(e => e.Id == r.Id).ForEach
                               (e =>
                               {
                                   e.Name = r.Name;
                                   e.Size = r.Size;
                                   e.Price = r.Price;
                                   e.Rating = r.Rating;
                                   e.Inventories = r.Inventories;
                               });
                           });

        // Act
        await _roomService.AddInventoryForRoomAsync(rooms[0].Id, newInventory);
        var actualInventories = rooms[0].Inventories?.Select(i => InventoryConverter.ConvertDbModelToAppModel(i)).ToList();
        var actual = actualInventories?.Last();

        // Assert
        Assert.Equal(inventories.Count(), actualInventories?.Count());
        Assert.Equal(newInventory.Id, actual?.Id);
        Assert.Equal(newInventory.Name, actual?.Name);
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

    private List<RoomDbModel> CreateMockRooms() 
    {
        return new List<RoomDbModel>
        {
            new RoomDbModel(1, "Room1", 20, 2500, 4.5, null),
            new RoomDbModel(2, "Room2", 30, 3500, 5.0, null),
            new RoomDbModel(3, "Room3", 25, 3000, 3.0, null),
            new RoomDbModel(4, "Room4", 25, 1300, 5.0, 
                     CreateMockInventory()),
        };
    }

    private List<InventoryDbModel> CreateMockInventory() 
    {
        return new List<InventoryDbModel>()
        {
            new InventoryDbModel(1, "Подушка"),
            new InventoryDbModel(2, "Телевизор"),
            new InventoryDbModel(3, "PS5")
        };
    }
}