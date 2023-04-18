using Anticafe.BL.Enums;
using Anticafe.BL.Exceptions;
using Anticafe.BL.IRepositories;
using Anticafe.BL.Models;
using Anticafe.BL.Sevices.FeedbackService;
using Moq;
using Xunit;

namespace UnitTests.Service;

public class FeedbackServiceUnitTests
{
    private readonly IFeedbackService _feedbackService;
    private readonly Mock<IFeedbackRepository> _mockFeedbackRepository = new();
    private readonly Mock<IRoomRepository> _mockRoomRepository = new();
    private readonly Mock<IUserRepository> _mockUserRepository = new();

    public FeedbackServiceUnitTests() 
    {
        _feedbackService = new FeedbackService(_mockFeedbackRepository.Object,
                                               _mockRoomRepository.Object,
                                               _mockUserRepository.Object);
    }

    [Fact]
    public async void GetAllFeedbackTest() 
    {
        // Arrange
        var feedbacks = CreateMockFeedback();

        _mockFeedbackRepository.Setup(s => s.GetAllFeedbackAsync())
                               .ReturnsAsync(feedbacks);

        // Act

        var actualFeedbacks = await _feedbackService.GetAllFeedbackAsync();

        // Assert
        Assert.Equal(feedbacks, actualFeedbacks);
    }

    [Fact]
    public async void GetAllFeedbackEmptyTest()
    {
        // Arrange
        var feedbacks = new List<Feedback>();

        _mockFeedbackRepository.Setup(s => s.GetAllFeedbackAsync())
                               .ReturnsAsync(feedbacks);

        // Act

        var actualFeedbacks = await _feedbackService.GetAllFeedbackAsync();

        // Assert
        Assert.Equal(feedbacks, actualFeedbacks);
    }

    [Fact]
    public async void GetAllFeedbackByRoomTest()
    {
        // Arrange
        var rooms = CreateMockRooms();
        var feedbacks = CreateMockFeedback();
        var roomId = 1;

        var excepctedFeedbacks = feedbacks.FindAll(e => e.RoomId == roomId);

        _mockRoomRepository.Setup(s => s.GetRoomByIdAsync(roomId))
                           .ReturnsAsync(rooms.Find(e => e.Id == roomId));

        _mockFeedbackRepository.Setup(s => s.GetAllFeedbackByRoomAsync(roomId))
                               .ReturnsAsync(feedbacks.FindAll(e => e.RoomId == roomId));
        // Act

        var actualFeedbacks = await _feedbackService.GetAllFeedbackByRoomAsync(roomId);

        // Assert
        Assert.Equal(excepctedFeedbacks, actualFeedbacks);
    }

    [Fact]
    public async void GetAllFeedbackByRoomEmptyTest()
    {
        // Arrange
        var rooms = CreateMockRooms();
        var feedbacks = new List<Feedback>();
        var roomId = 1;

        var excepctedFeedbacks = feedbacks.FindAll(e => e.RoomId == roomId);

        _mockRoomRepository.Setup(s => s.GetRoomByIdAsync(roomId))
                           .ReturnsAsync(rooms.Find(e => e.Id == roomId));

        _mockFeedbackRepository.Setup(s => s.GetAllFeedbackByRoomAsync(roomId))
                               .ReturnsAsync(feedbacks.FindAll(e => e.RoomId == roomId));
        // Act

        var actualFeedbacks = await _feedbackService.GetAllFeedbackByRoomAsync(roomId);

        // Assert
        Assert.Equal(excepctedFeedbacks, actualFeedbacks);
    }

    [Fact]
    public void GetAllFeedbackByRoomNotFoundTest()
    {
        // Arrange
        var rooms = CreateMockRooms();
        var roomId = 10;

        _mockRoomRepository.Setup(s => s.GetRoomByIdAsync(roomId))
                           .ReturnsAsync(rooms.Find(e => e.Id == roomId));

        // Act
        var action = async () => await _feedbackService.GetAllFeedbackByRoomAsync(roomId);

        // Assert
        Assert.ThrowsAsync<RoomNotFoundException>(action);
    }

    [Fact]
    public async void AddFeedbackTest()
    {
        // Arrange
        var rooms = CreateMockRooms();
        var feedbacks = CreateMockFeedback();

        var roomId = 1;
        var userId = 3;
        var user = CreateMockUser(userId);

        var feedback = new Feedback(3, userId, roomId, DateTime.UtcNow, 5, "Описание 1");

        _mockUserRepository.Setup(s => s.GetUserByIdAsync(userId))
                           .ReturnsAsync(user);    

        _mockRoomRepository.Setup(s => s.GetRoomByIdAsync(roomId))
                           .ReturnsAsync(rooms.Find(e => e.Id == roomId));

        _mockFeedbackRepository.Setup(s => s.InsertFeedbackAsync(It.IsAny<Feedback>()))
                               .Callback((Feedback f) => feedbacks.Add(f));

        // Act
        await _feedbackService.AddFeedbackAsync(feedback);

        var actualFeedback = feedbacks.Last();

        // Assert
        Assert.Equal(feedback, actualFeedback);
    }

    [Fact]
    public async void AddFeedbackEmptyTest()
    {
        // Arrange
        var rooms = CreateMockRooms();
        var feedbacks = new List<Feedback>();

        var roomId = 1;
        var userId = 3;
        var user = CreateMockUser(userId);

        var feedback = new Feedback(3, userId, roomId, DateTime.UtcNow, 5, "Описание 1");

        _mockUserRepository.Setup(s => s.GetUserByIdAsync(userId))
                           .ReturnsAsync(user);

        _mockRoomRepository.Setup(s => s.GetRoomByIdAsync(roomId))
                           .ReturnsAsync(rooms.Find(e => e.Id == roomId));

        _mockFeedbackRepository.Setup(s => s.InsertFeedbackAsync(It.IsAny<Feedback>()))
                               .Callback((Feedback f) => feedbacks.Add(f));

        // Act
        await _feedbackService.AddFeedbackAsync(feedback);

        var actualFeedback = feedbacks.Last();

        // Assert
        Assert.Equal(feedback, actualFeedback);
    }

    [Fact]
    public void AddFeedbackNoExistUserTest()
    {
        // Arrange
        var rooms = CreateMockRooms();
        var feedbacks = new List<Feedback>();

        var roomId = 1;
        var userId = 3;

        var feedback = new Feedback(3, userId, roomId, DateTime.UtcNow, 5, "Описание 1");

        _mockRoomRepository.Setup(s => s.GetRoomByIdAsync(roomId))
                           .ReturnsAsync(rooms.Find(e => e.Id == roomId));

        _mockFeedbackRepository.Setup(s => s.InsertFeedbackAsync(It.IsAny<Feedback>()))
                               .Callback((Feedback f) => feedbacks.Add(f));

        // Act
        var action = async() => await _feedbackService.AddFeedbackAsync(feedback);

        // Assert
        Assert.ThrowsAsync<UserNotFoundException>(action);
    }

    [Fact]
    public void AddFeedbackNotFoundRoomTest()
    {
        // Arrange
        var rooms = CreateMockRooms();
        var feedbacks = new List<Feedback>();

        var roomId = 100;
        var userId = 3;
        var user = CreateMockUser(userId);

        var feedback = new Feedback(3, userId, roomId, DateTime.UtcNow, 5, "Описание 1");

        _mockRoomRepository.Setup(s => s.GetRoomByIdAsync(roomId))
                           .ReturnsAsync(rooms.Find(e => e.Id == roomId));

        _mockFeedbackRepository.Setup(s => s.InsertFeedbackAsync(It.IsAny<Feedback>()))
                               .Callback((Feedback f) => feedbacks.Add(f));

        // Act
        var action = async () => await _feedbackService.AddFeedbackAsync(feedback);

        // Assert
        Assert.ThrowsAsync<RoomNotFoundException>(action);
    }

    [Fact]
    public async void UpdateFeedbackTest()
    {
        // Arrange
        var rooms = CreateMockRooms();
        var feedbacks = CreateMockFeedback();

        var feedbackId = 1;
        var roomId = 1;
        var userId = 3;
        var user = CreateMockUser(userId);

        var feedback = new Feedback(feedbackId, userId, roomId, DateTime.UtcNow, 3, "Описание Новое");

        _mockUserRepository.Setup(s => s.GetUserByIdAsync(userId))
                           .ReturnsAsync(user);

        _mockRoomRepository.Setup(s => s.GetRoomByIdAsync(roomId))
                           .ReturnsAsync(rooms.Find(e => e.Id == roomId));

        _mockFeedbackRepository.Setup(s => s.GetFeedbackAsync(feedbackId))
                               .ReturnsAsync(feedbacks.Find(e => e.Id == feedbackId));

        _mockFeedbackRepository.Setup(s => s.UpdateFeedbackAsync(It.IsAny<Feedback>()))
                           .Callback((Feedback f) =>
                           {
                               feedbacks.FindAll(e => e.Id == f.Id).ForEach
                               (e =>
                               {
                                   e.Date = f.Date;
                                   e.Mark = f.Mark;
                                   e.Message = f.Message;
                               });
                           });

        // Act
        await _feedbackService.UpdateFeedbackAsync(feedback);

        var actualFeedback = feedbacks.Find(e => e.Id == feedbackId);

        // Assert
        Assert.Equal(feedback.Id, actualFeedback.Id);
        Assert.Equal(feedback.Mark, actualFeedback.Mark);
        Assert.Equal(feedback.Message, actualFeedback.Message);
        Assert.Equal(feedback.Date, actualFeedback.Date);
        Assert.Equal(feedback.RoomId, actualFeedback.RoomId);
        Assert.Equal(feedback.UserId, actualFeedback.UserId);
    }

    [Fact]
    public void UpdateFeedbackEmptyTest()
    {
        // Arrange
        var rooms = CreateMockRooms();
        var feedbacks = new List<Feedback>();

        var feedbackId = 1;
        var roomId = 1;
        var userId = 3;
        var user = CreateMockUser(userId);

        var feedback = new Feedback(feedbackId, userId, roomId, DateTime.UtcNow, 3, "Описание Новое");

        _mockFeedbackRepository.Setup(s => s.GetFeedbackAsync(feedbackId))
                               .ReturnsAsync(feedbacks.Find(e => e.Id == feedbackId));

        // Act
        var action = async () => await _feedbackService.UpdateFeedbackAsync(feedback);

        // Assert
        Assert.ThrowsAsync<FeedbackNotFoundException>(action);
    }

    [Fact]
    public async void UpdateRoomRaitingTest()
    {
        // Arrange
        var rooms = CreateMockRooms();
        var feedbacks = new List<Feedback>()
        {
            new Feedback(1, 3, 1, new DateTime(2023, 03, 28), 4, "Описание1"),
            new Feedback(2, 1, 1, new DateTime(2023, 02, 10), 3, "Описание2"),
            new Feedback(3, 2, 1, new DateTime(2023, 02, 10), 5, "Описание3"),
            new Feedback(4, 2, 2, new DateTime(2023, 02, 10), 5, "Описание4")
        };

        var roomId = 1;

        var exceptedRaiting = 4.0; 

        _mockRoomRepository.Setup(s => s.GetRoomByIdAsync(roomId))
                               .ReturnsAsync(rooms.Find(e => e.Id == roomId));

        _mockFeedbackRepository.Setup(s => s.GetAllFeedbackByRoomAsync(roomId))
                               .ReturnsAsync(feedbacks.FindAll(e => e.Id == roomId));

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
        await _feedbackService.UpdateRoomRatingAsync(roomId);

        var actualRaiting = rooms[0].Rating;

        // Assert
        Assert.Equal(exceptedRaiting, actualRaiting);
    }

    [Fact]
    public async void DeleteFeedbackTest()
    {
        // Arrange
        var rooms = CreateMockRooms();
        var feedbacks = CreateMockFeedback();

        var feedbackId = 1;
        var userId = 1;
        var user = CreateMockUser(userId);
        var count = feedbacks.Count();

        _mockFeedbackRepository.Setup(s => s.GetFeedbackAsync(feedbackId))
                               .ReturnsAsync(feedbacks.Find(e => e.Id == feedbackId));

        _mockFeedbackRepository.Setup(s => s.DeleteFeedbackAsync(It.IsAny<int>()))
                               .Callback((int id) => feedbacks.RemoveAll(e => e.Id == id)); ;

        // Act
        await _feedbackService.DeleteFeedbackAsync(feedbackId);

        var actualCount = feedbacks.Count();

        // Assert
        Assert.Equal(count - 1, actualCount);
    }

    [Fact]
    public void DeleteFeedbackEmptyTest()
    {
        // Arrange
        var rooms = CreateMockRooms();
        var feedbacks = new List<Feedback>();

        var feedbackId = 1;
        var roomId = 1;
        var userId = 3;
        var user = CreateMockUser(userId);

        var feedback = new Feedback(feedbackId, userId, roomId, DateTime.UtcNow, 3, "Описание Новое");

        _mockFeedbackRepository.Setup(s => s.GetFeedbackAsync(feedbackId))
                               .ReturnsAsync(feedbacks.Find(e => e.Id == feedbackId));

        // Act
        var action = async () => await _feedbackService.DeleteFeedbackAsync(feedbackId);

        // Assert
        Assert.ThrowsAsync<FeedbackNotFoundException>(action);
    }

    private User CreateMockUser(int userId) 
    {
        return new User(userId, "Иванов", "Иван", "Иванович", new DateTime(2002, 06, 28), Gender.Male, "login", "99999");
    }

    private List<Feedback> CreateMockFeedback()
    {
        return new List<Feedback>()
        {
            new Feedback(1, 3, 1, new DateTime(2023, 03, 28), 4, "Описание1"),
            new Feedback(2, 1, 2, new DateTime(2023, 02, 10), 3, "Описание2"),
            new Feedback(2, 2, 1, new DateTime(2023, 02, 10), 3, "Описание3")
        };
    }

    private List<Room> CreateMockRooms()
    {
        return new List<Room>
        {
            new Room(1, "Room1", 20, 2500, 4, null, null),
            new Room(2, "Room2", 30, 3500, 0.0, null, null),
            new Room(3, "Room3", 25, 3000, 0.0, null, null),
            new Room(4, "Room4", 25, 1300, 0.0,
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
