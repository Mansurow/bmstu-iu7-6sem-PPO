using Moq;
using Portal.Common.Models;
using Portal.Common.Models.Enums;
using Xunit;
using Portal.Database.Repositories.Interfaces;
using Portal.Services.BookingService;

namespace UnitTests.Services;

public class BookingServiceUnitTests
{
    private readonly IBookingService _bookingService;
    private readonly Mock<IBookingRepository> _mockBookingRepository = new();
    private readonly Mock<IUserRepository> _mockUserRepository = new();

    public BookingServiceUnitTests()
    {
        _bookingService = new BookingService(_mockBookingRepository.Object,
                                             _mockUserRepository.Object);
    }

    [Fact]
    public async Task GetFreeTimeTest()
    {
        // Arrange
        var bookings = new List<Booking>()
        {
            new Booking(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 
                10,BookingStatus.Reserved,
                new DateOnly(2023, 05, 7), 
                new TimeOnly(8 , 0),
                new TimeOnly(10, 40)),
            new Booking(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 
                10,BookingStatus.Reserved,
                new DateOnly(2023, 05, 7), 
                new TimeOnly(12 , 0),
                new TimeOnly(15, 30)),
            new Booking(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 
                10,BookingStatus.Reserved,
                new DateOnly(2023, 05, 7), 
                new TimeOnly(11 , 0),
                new TimeOnly(12, 00)),
        };
        
        _mockBookingRepository.Setup(s => s.GetAllBookingAsync())
            .ReturnsAsync(bookings);
        // Act
        var free = await _bookingService.IsFreeTimeAsync(new DateOnly(2023, 05, 7), 
            new TimeOnly(16, 0),
            new TimeOnly(20, 0));
        
        // Assert
    }

    // [Fact]
    // public async void GetAllBookingsOkTest()
    // {
    //     // Arrange
    //     var bookings = CreateMockBooking();
    //
    //
    //     _mockBookingRepository.Setup(s => s.GetAllBookingAsync())
    //                           .ReturnsAsync(bookingsDb);
    //
    //     _mockBookingRepository.Setup(s => s.UpdateNoActualBookingAsync(It.IsAny<int>()))
    //                           .Callback((int id) =>
    //                           {
    //                               bookingsDb.FindAll(b => b.Id == id).ForEach(b => b.Status = BookingStatus.NoActual);
    //                           });
    //
    //     // Act
    //     var actualBookings = await _bookingService.GetAllBookingAsync();
    //     var actualBookingsDb = actualBookings.Select(b => BookingConverter.ConvertAppModelToDbModel(b)).ToList();
    //
    //     // Assert
    //     AssertBookings(bookingsDb, actualBookingsDb);
    // }
    //
    // [Fact]
    // public async void GetAllBookingsEmptyTest()
    // {
    //     // Arrange
    //     var bookingDb = new List<BookingDbModel>();
    //
    //     _mockBookingRepository.Setup(s => s.GetAllBookingAsync())
    //                           .ReturnsAsync(bookingDb);
    //
    //     _mockBookingRepository.Setup(s => s.UpdateNoActualBookingAsync(It.IsAny<int>()))
    //                           .Callback((int id) =>
    //                           {
    //                               bookingDb.FindAll(b => b.Id == id).ForEach(b => b.Status = BookingStatus.NoActual);
    //                           });
    //
    //     // Act
    //     var actaulBookings = await _bookingService.GetAllBookingAsync();
    //     var actaulBookingsDb = actaulBookings.Select(b => BookingConverter.ConvertAppModelToDbModel(b)).ToList();
    //
    //     // Assert
    //     Assert.Equal(bookingDb!.Count, actaulBookingsDb!.Count);
    //     Assert.Equal(bookingDb, actaulBookingsDb);
    // }
    //
    // [Fact]
    // public async void GetBookingByUserOkTest()
    // {
    //     // Arrange
    //     var usersDb = CreateMockUsers();
    //     var bookingDb = CreateMockBooking();
    //     var expectedUser = usersDb.First();
    //     var expectedBookings = bookingDb.Where(e => e.UserId == expectedUser.Id).ToList();
    //
    //     _mockBookingRepository.Setup(s => s.GetBookingByUserAsync(It.IsAny<int>()))
    //                           .ReturnsAsync((int userId) => bookingDb.FindAll(e => e.UserId == userId));
    //
    //     _mockBookingRepository.Setup(s => s.UpdateNoActualBookingAsync(It.IsAny<int>()))
    //                          .Callback((int id) =>
    //                          {
    //                              bookingDb.FindAll(b => b.Id == id).ForEach(b => b.Status = BookingStatus.NoActual);
    //                          });
    //
    //     // Act
    //     var actaulBookings = await _bookingService.GetBookingByUserAsync(expectedUser.Id);
    //     var actaulBookingsDb = actaulBookings.Select(b => BookingConverter.ConvertAppModelToDbModel(b)).ToList();
    //
    //     // Assert
    //     AssertBookings(expectedBookings, actaulBookingsDb);
    // }
    //
    // [Fact]
    // public async void GetBookingByUserEmptyTest()
    // {
    //     // Arrange
    //     var usersDb = CreateMockUsers();
    //     var bookingDb = new List<BookingDbModel>();
    //     var expectedUser = usersDb.First();
    //     var expectedBookings = bookingDb.Where(e => e.UserId == expectedUser.Id).ToList();
    //
    //     _mockBookingRepository.Setup(s => s.GetBookingByUserAsync(It.IsAny<int>()))
    //                           .ReturnsAsync((int userId) => bookingDb.FindAll(e => e.UserId == userId));
    //
    //     _mockBookingRepository.Setup(s => s.UpdateNoActualBookingAsync(It.IsAny<int>()))
    //                          .Callback((int id) =>
    //                          {
    //                              bookingDb.FindAll(b => b.Id == id).ForEach(b => b.Status = BookingStatus.NoActual);
    //                          });
    //
    //     // Act
    //     var actaulBookings = await _bookingService.GetBookingByUserAsync(expectedUser.Id);
    //     var actaulBookingsDb = actaulBookings.Select(b => BookingConverter.ConvertAppModelToDbModel(b)).ToList();
    //
    //     // Assert
    //     AssertBookings(expectedBookings, actaulBookingsDb);
    // }
    //
    // [Fact]
    // public async void GetBookingByRoomOkTest()
    // {
    //     // Arrange
    //     var roomsDb = CreateMockRooms();
    //     var bookingDb = CreateMockBooking();
    //     var expectedRoom = roomsDb.First();
    //     var expectedBookings = bookingDb.Where(e => e.RoomId == expectedRoom.Id).ToList();
    //
    //     _mockBookingRepository.Setup(s => s.GetBookingByRoomAsync(It.IsAny<int>()))
    //                           .ReturnsAsync((int roomId) => bookingDb.FindAll(e => e.RoomId == roomId));
    //
    //     _mockBookingRepository.Setup(s => s.UpdateNoActualBookingAsync(It.IsAny<int>()))
    //                          .Callback((int id) =>
    //                          {
    //                              bookingDb.FindAll(b => b.Id == id).ForEach(b => b.Status = BookingStatus.NoActual);
    //                          });
    //
    //     // Act
    //     var actaulBookings = await _bookingService.GetBookingByRoomAsync(expectedRoom.Id);
    //     var actaulBookingsDb = actaulBookings.Select(b => BookingConverter.ConvertAppModelToDbModel(b)).ToList();
    //
    //     // Assert
    //     AssertBookings(expectedBookings, actaulBookingsDb);
    // }
    //
    // [Fact]
    // public async void GetBookingByRoomEmptyTest()
    // {
    //     // Arrange
    //     var roomsDb = CreateMockRooms();
    //     var bookingDb = new List<BookingDbModel>();
    //     var expectedRoom = roomsDb.First();
    //     var expectedBookings = bookingDb.Where(e => e.RoomId == expectedRoom.Id).ToList();
    //
    //     _mockBookingRepository.Setup(s => s.GetBookingByRoomAsync(It.IsAny<int>()))
    //                           .ReturnsAsync((int roomId) => bookingDb.FindAll(e => e.RoomId == roomId));
    //
    //     _mockBookingRepository.Setup(s => s.UpdateNoActualBookingAsync(It.IsAny<int>()))
    //                          .Callback((int id) =>
    //                          {
    //                              bookingDb.FindAll(b => b.Id == id).ForEach(b => b.Status = BookingStatus.NoActual);
    //                          });
    //
    //     // Act
    //     var actaulBookings = await _bookingService.GetBookingByRoomAsync(expectedRoom.Id);
    //     var actaulBookingsDb = actaulBookings.Select(b => BookingConverter.ConvertAppModelToDbModel(b)).ToList();
    //
    //     // Assert
    //     AssertBookings(expectedBookings, actaulBookingsDb);
    // }
    //
    // [Fact]
    // public async void CreateBookingOkTest()
    // {
    //     // Arrange
    //     var userDb = CreateMockUsers();
    //     var roomDb = CreateMockUsers();
    //     var bookingDb = CreateMockBooking();
    //     var expectedCount = bookingDb.Count + 1;
    //
    //     var startTomorrow = DateTime.Today + new TimeSpan(1, 12, 00, 00);
    //     var endTomorrow = DateTime.Today + new TimeSpan(1, 18, 00, 00);
    //     var excpectedBooking = new BookingDbModel(5, userDb.Last().Id, roomDb.Last().Id, 10, startTomorrow, endTomorrow, BookingStatus.Reserved);
    //
    //     _mockBookingRepository.Setup(s => s.GetUserBookingByRoomForTimeRangeAsync(
    //                                                                   It.IsAny<int>(),
    //                                                                   It.IsAny<int>(),
    //                                                                   It.IsAny<DateTime>(),
    //                                                                   It.IsAny<DateTime>()))
    //                        .ReturnsAsync((int roomId, int userId, DateTime start, DateTime end) =>
    //                                       bookingDb.Where(b => b.RoomId == roomId
    //                                                    && b.UserId == userId
    //                                                    && b.StartTime >= start
    //                                                    && b.EndTime <= end).FirstOrDefault());
    //
    //     _mockBookingRepository.Setup(s => s.GetAllBookingByRoomForTimeRange(It.IsAny<int>(),
    //                                                                   It.IsAny<DateTime>(),
    //                                                                   It.IsAny<DateTime>()))
    //                           .ReturnsAsync((int roomId, DateTime start, DateTime end) =>
    //                                          bookingDb.FindAll(b => b.RoomId == roomId
    //                                                    && b.StartTime >= start
    //                                                    && b.EndTime <= end));
    //     _mockBookingRepository.Setup(s => s.InsertBookingAsync(It.IsAny<BookingDbModel>()))
    //                           .Callback((BookingDbModel b) =>
    //                           {
    //                               b.Id = bookingDb.Count + 1;
    //                               bookingDb.Add(b);
    //                           });
    //     // Act
    //     await _bookingService.CreateBookingAsync(excpectedBooking.UserId, excpectedBooking.RoomId, excpectedBooking.AmountPeople, excpectedBooking.StartTime.ToString(), excpectedBooking.EndTime.ToString());
    //     var actualCount = bookingDb.Count;
    //
    //     // Asserts
    //     Assert.Equal(expectedCount, actualCount);
    //     AssertBooking(bookingDb.Last(), excpectedBooking);
    // }
    //
    // [Fact]
    // public async void DeleteBookingOkTest()
    // {
    //     // Arrange
    //     var userDb = CreateMockUsers();
    //     var roomDb = CreateMockUsers();
    //     var bookingDb = CreateMockBooking();
    //     var expectedCount = bookingDb.Count - 1;
    //
    //     var booking = bookingDb.First();
    //
    //     _mockBookingRepository.Setup(s => s.GetUserBookingByRoomForTimeRangeAsync(
    //                                                                   It.IsAny<int>(),
    //                                                                   It.IsAny<int>(),
    //                                                                   It.IsAny<DateTime>(),
    //                                                                   It.IsAny<DateTime>()))
    //                        .ReturnsAsync((int roomId, int userId, DateTime start, DateTime end) =>
    //                                       bookingDb.Where(b => b.RoomId == roomId
    //                                                    && b.UserId == userId
    //                                                    && b.StartTime >= start
    //                                                    && b.EndTime <= end).FirstOrDefault());
    //
    //     _mockBookingRepository.Setup(s => s.GetBookingByIdAsync(It.IsAny<int>()))
    //                           .ReturnsAsync(booking);
    //     _mockBookingRepository.Setup(s => s.DeleteBookingAsync(It.IsAny<int>()))
    //                           .Callback((int id) =>
    //                           {
    //                               var b = bookingDb.Find(b => b.RoomId == id);
    //                               bookingDb.Remove(b!);
    //                           });
    //     // Act
    //     await _bookingService.DeleteBookingAsync(booking.Id);
    //     var actualCount = bookingDb.Count;
    //
    //     // Asserts
    //     Assert.Equal(expectedCount, actualCount);
    //     Assert.NotEqual(booking, bookingDb.First());
    // }

    // private List<Zone> CreateMockRooms()
    // {
    //     return new List<Zone>
    //     {
    //         new Zone(Guid.NewGuid(), "Room1", 20, 2500, 4.5),
    //         new Zone(2, "Room2", 30, 3500, 5.0),
    //         new Zone(3, "Room3", 25, 3000, 3.0),
    //         new Zone(4, "Room4", 25, 1300, 5.0),
    //     };
    // }
    //
    // private List<User> CreateMockUsers()
    // {
    //     return new List<User>()
    //     {
    //         new User(1, "Иванов", "Иван", "Иванович",  new DateTime(2002, 03, 17), Gender.Male, "ivanov@mail.ru", "+7899999999", "password12123434"),
    //         new User(2, "Петров", "Петр", "Петрович",  new DateTime(2003, 05, 18), Gender.Male, "petrov@mail.ru", "+7899909999", "password12122323"),
    //         new User(3, "Cударь", "Елена", "Александровна",  new DateTime(1999, 09, 18), Gender.Female, "sudar@mail.ru", "+781211111", "password12121212")
    //     };
    // }
    //
    // private List<Booking> CreateMockBooking()
    // {
    //     return new List<Booking>()
    //     {
    //         new Booking(1, 1, 1, 10, new DateTime(2023, 05, 7, 12, 00, 00), new DateTime(2023, 05, 7, 20, 00, 00), BookingStatus.Reserved),
    //         new Booking(2, 3, 2, 10, new DateTime(2023, 05, 10, 13, 00, 00), new DateTime(2023, 05, 10, 15, 00, 00), BookingStatus.Reserved),
    //         new Booking(3, 1, 3, 10, new DateTime(2023, 05, 01, 12, 00, 00), new DateTime(2023, 05, 01, 18, 00, 00), BookingStatus.NoActual),
    //         new Booking(4, 1, 1, 10, new DateTime(2023, 04, 01, 12, 00, 00), new DateTime(2023, 04, 01, 18, 00, 00), BookingStatus.Reserved),
    //     };
    // }
}
