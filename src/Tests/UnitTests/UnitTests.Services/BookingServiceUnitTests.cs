using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using Portal.Common.Models;
using Portal.Common.Models.Enums;
using Portal.Database.Repositories.Interfaces;
using Portal.Services.BookingService;
using Portal.Services.BookingService.Configuration;
using Portal.Services.BookingService.Exceptions;
using Portal.Services.PackageService.Exceptions;
using Portal.Services.ZoneService.Exceptions;

namespace UnitTests.Services;

public class BookingServiceUnitTests
{
    private readonly IBookingService _bookingService;
    private readonly Mock<IBookingRepository> _mockBookingRepository = new();
    private readonly Mock<IPackageRepository> _mockPackageRepository = new();
    private readonly Mock<IZoneRepository> _mockZoneRepository = new();

    public BookingServiceUnitTests()
    {
        var config = Options.Create(
            new BookingServiceConfiguration()
            {
                StartTimeWorking = "8:00:00",
                EndTimeWorking = "23:00:00"
            });
        
        _bookingService = new BookingService(_mockBookingRepository.Object,
            _mockPackageRepository.Object,
            _mockZoneRepository.Object,
            NullLogger<BookingService>.Instance,
            config);
    }

    /// <summary>
    /// Тест на получение всех броней при заполненной базы данных
    /// </summary>
    [Fact]
    public async Task GetAllBookingsTest()
    {
        // Arrange
        var packages = CreateMockPackages();
        var users = CreateMockUsers();
        var zones = CreateMockZones();
        var bookings = CreateMockBooking(users, zones, packages);
        
        _mockBookingRepository.Setup(s => s.GetAllBookingAsync())
            .ReturnsAsync(bookings);
    
        _mockBookingRepository.Setup(s => s.UpdateNoActualBookingAsync(It.IsAny<Guid>()))
            .Callback((Guid id) =>
            {
                var booking = bookings.First(b => b.Id == id);
                booking.ChangeStatus(BookingStatus.NoActual);
            });
    
        // Act
        var actualBookings = await _bookingService.GetAllBookingAsync();

        // Assert
        Assert.Equal(bookings, actualBookings);
    }

    /// <summary>
    /// Тест на получение всех броней при пустой базы данных
    /// </summary>
    [Fact]
    public async Task GetAllBookingsEmptyTest()
    {
        // Arrange
        var bookings = new List<Booking>();
    
        _mockBookingRepository.Setup(s => s.GetAllBookingAsync())
            .ReturnsAsync(bookings);
    
        _mockBookingRepository.Setup(s => s.UpdateNoActualBookingAsync(It.IsAny<Guid>()))
            .Callback((Guid id) =>
            {
                var booking = bookings.First(b => b.Id == id);
                booking.ChangeStatus(BookingStatus.NoActual);
            });
    
        // Act
        var actualBookings = await _bookingService.GetAllBookingAsync();
        
        // Assert
        Assert.Equal(bookings, actualBookings);
    }
    
    /// <summary>
    /// Тест на получение броней для пользователя
    /// </summary>
    [Fact]
    public async Task GetBookingByUserTest()
    {
        // Arrange
        var packages = CreateMockPackages();
        var users = CreateMockUsers();
        var zones = CreateMockZones();
        var bookings = CreateMockBooking(users, zones, packages);
        var expectedUser = users.First();
        var expectedBookings = bookings.Where(e => e.UserId == expectedUser.Id).ToList();
    
        _mockBookingRepository.Setup(s => s.GetBookingByUserAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid userId) => bookings.FindAll(e => e.UserId == userId));
    
        _mockBookingRepository.Setup(s => s.UpdateNoActualBookingAsync(It.IsAny<Guid>()))
            .Callback((Guid id) =>
            {
                var booking = bookings.First(b => b.Id == id);
                booking.ChangeStatus(BookingStatus.NoActual);
            });
    
        // Act
        var actualBookings = await _bookingService.GetBookingByUserAsync(expectedUser.Id);
        
        // Assert
        Assert.Equal(expectedBookings, actualBookings);
    }
    
    /// <summary>
    /// Тест на получение броней для пользователя
    /// </summary>
    [Fact]
    public async Task GetBookingByUserEmptyTest()
    {
        // Arrange
        var users = CreateMockUsers();
        var bookings = new List<Booking>();
        var expectedUser = users.First();
    
        _mockBookingRepository.Setup(s => s.GetBookingByUserAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid userId) => bookings.FindAll(e => e.UserId == userId));
    
        _mockBookingRepository.Setup(s => s.UpdateNoActualBookingAsync(It.IsAny<Guid>()))
            .Callback((Guid id) =>
            {
                var booking = bookings.First(b => b.Id == id);
                booking.ChangeStatus(BookingStatus.NoActual);
            });
        
        // Act
        var actualBookings = await _bookingService.GetBookingByUserAsync(expectedUser.Id);
        
        // Assert
        Assert.Equal(bookings, actualBookings);
    }

    /// <summary>
    /// Тест на нахождение брони по идентификатору
    /// </summary>
    [Fact]
    public async Task GetBookingByIdTest()
    {
        // Arrange
        var packages = CreateMockPackages();
        var users = CreateMockUsers();
        var zones = CreateMockZones();
        var bookings = CreateMockBooking(users, zones, packages);
        var expectedBooking = bookings.First();

        _mockBookingRepository.Setup(s => s.GetBookingByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => bookings.First(b => b.Id == id));
        
        // Act
        var actualBooking = await _bookingService.GetBookingByIdAsync(expectedBooking.Id);

        // Assert
        Assert.Equal(expectedBooking, actualBooking);
    }
    
    /// <summary>
    /// Тест на нахождение брони по идентификатору
    /// </summary>
    [Fact]
    public async Task GetBookingByIdNotFoundTest()
    {
        // Arrange
        var packages = CreateMockPackages();
        var users = CreateMockUsers();
        var zones = CreateMockZones();
        var bookings = CreateMockBooking(users, zones, packages);
        var bookingId = Guid.NewGuid();

        _mockBookingRepository.Setup(s => s.GetBookingByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => bookings.First(b => b.Id == id));
        
        // Act
        async Task<Booking> Action() => await _bookingService.GetBookingByIdAsync(bookingId);

        // Assert
        await Assert.ThrowsAsync<BookingNotFoundException>(Action);
    }
    
    /// <summary>
    /// Тест на создание брони зоны
    /// </summary>
    [Fact]
    public async Task CreateBookingTest()
    {
        // Arrange
        var packages = CreateMockPackages();
        var users = CreateMockUsers();
        var zones = CreateMockZones();
        var bookings = CreateMockBooking(users, zones, packages);
        var expectedCount = bookings.Count + 1;
        
        var expectedBooking = new Booking(Guid.NewGuid(), zones.Last().Id, users.First().Id, 
            packages.First().Id, zones.Last().Limit, BookingStatus.TemporaryReserved, 
            DateOnly.FromDateTime(DateTime.UtcNow), 
            new TimeOnly(12, 0),
            new TimeOnly(20, 0));

        _mockZoneRepository.Setup(s => s.GetZoneByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid zoneId) => zones.First(z => z.Id == zoneId));

        _mockPackageRepository.Setup(s => s.GetPackageByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid packageId) => packages.First(p => p.Id == packageId));

        _mockBookingRepository.Setup(s => s.GetAllBookingAsync())
            .ReturnsAsync(bookings);
        
        _mockBookingRepository.Setup(s => s.GetBookingByUserAndZoneAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync((Guid userId, Guid zoneId) =>
                bookings.FindAll(b => b.UserId == userId && b.ZoneId == zoneId));
        
        _mockBookingRepository.Setup(s => s.InsertBookingAsync(It.IsAny<Booking>()))
            .Callback((Booking b) => bookings.Add(b));
        
        // Act
        var expectedId = await _bookingService.AddBookingAsync(expectedBooking.UserId, expectedBooking.ZoneId, expectedBooking.PackageId,
            expectedBooking.Date, expectedBooking.StartTime, expectedBooking.EndTime);
        var actualCount = bookings.Count;
        var actualBooking = bookings.Last();
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.Equal(expectedId, actualBooking.Id);
        Assert.Equal(expectedBooking.UserId,  actualBooking.UserId);
        Assert.Equal(expectedBooking.ZoneId,  actualBooking.ZoneId);
        Assert.Equal(expectedBooking.PackageId,  actualBooking.PackageId);
        Assert.Equal(expectedBooking.Status,  actualBooking.Status);
        Assert.Equal(expectedBooking.Date,  actualBooking.Date);
        Assert.Equal(expectedBooking.StartTime,  actualBooking.StartTime);
        Assert.Equal(expectedBooking.EndTime,  actualBooking.EndTime);
    }
    
    /// <summary>
    /// Тест на создание брони зоны (в выбранной время уже полностью или частично забронировано )
    /// </summary>
    [Theory]
    [InlineData("9:00", "11:00")]
    [InlineData("8:00", "12:00")]
    [InlineData("8:00", "13:00")]
    [InlineData("10:00", "16:00")]
    public async Task CreateBookingReversedTimeTest(string startTime, string endTime)
    {
        // Arrange
        var packages = CreateMockPackages();
        var users = CreateMockUsers();
        var zones = CreateMockZones();
        var bookings = CreateMockBooking(users, zones, packages);
        var expectedCount = bookings.Count;
        
        var expectedBooking = new Booking(Guid.NewGuid(), zones.First().Id, users.First().Id, 
            packages.First().Id, zones.Last().Limit, BookingStatus.TemporaryReserved, 
            DateOnly.FromDateTime(DateTime.UtcNow  + new TimeSpan(20, 0, 0, 0)), 
            TimeOnly.Parse(startTime),
            TimeOnly.Parse(endTime));

        _mockZoneRepository.Setup(s => s.GetZoneByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid zoneId) => zones.First(z => z.Id == zoneId));

        _mockPackageRepository.Setup(s => s.GetPackageByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid packageId) => packages.First(p => p.Id == packageId));

        _mockBookingRepository.Setup(s => s.GetAllBookingAsync())
            .ReturnsAsync(bookings);
        
        _mockBookingRepository.Setup(s => s.GetBookingByUserAndZoneAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync((Guid userId, Guid zoneId) =>
                bookings.FindAll(b => b.UserId == userId && b.ZoneId == zoneId));
        
        // _mockBookingRepository.Setup(s => s.InsertBookingAsync(It.IsAny<Booking>()))
        //     .Callback((Booking b) => bookings.Add(b));
        
        // Act
        async Task<Guid> Action() => await _bookingService.AddBookingAsync(expectedBooking.UserId, expectedBooking.ZoneId, expectedBooking.PackageId, expectedBooking.Date, expectedBooking.StartTime, expectedBooking.EndTime);
        var actualCount = bookings.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        await Assert.ThrowsAsync<BookingReversedException>(Action);
    }
    
    /// <summary>
    /// Тест на создание брони зоны (в выбранной дате уже есть бронь пользователя )
    /// </summary>
    [Fact]
    public async Task CreateBookingReversedDateTest()
    {
        // Arrange
        var packages = CreateMockPackages();
        var users = CreateMockUsers();
        var zones = CreateMockZones();
        var bookings = CreateMockBooking(users, zones, packages);
        var expectedCount = bookings.Count;
        
        var expectedBooking = new Booking(Guid.NewGuid(), zones.First().Id, users.First().Id, 
            packages.First().Id, zones.First().Limit, BookingStatus.TemporaryReserved, 
            DateOnly.FromDateTime(DateTime.Today), 
            new TimeOnly(12, 0),
            new TimeOnly(20, 0));

        _mockZoneRepository.Setup(s => s.GetZoneByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid zoneId) => zones.First(z => z.Id == zoneId));

        _mockPackageRepository.Setup(s => s.GetPackageByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid packageId) => packages.First(p => p.Id == packageId));

        _mockBookingRepository.Setup(s => s.GetAllBookingAsync())
            .ReturnsAsync(bookings);
        
        _mockBookingRepository.Setup(s => s.GetBookingByUserAndZoneAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync((Guid userId, Guid zoneId) =>
                bookings.FindAll(b => b.UserId == userId && b.ZoneId == zoneId));
        
        // _mockBookingRepository.Setup(s => s.InsertBookingAsync(It.IsAny<Booking>()))
        //     .Callback((Booking b) => bookings.Add(b));
        
        // Act
        async Task<Guid> Action() => await _bookingService.AddBookingAsync(expectedBooking.UserId, 
            expectedBooking.ZoneId, expectedBooking.PackageId, expectedBooking.Date, 
            expectedBooking.StartTime, expectedBooking.EndTime);
        var actualCount = bookings.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        await Assert.ThrowsAsync<BookingExistsException>(Action);
    }
    
    /// <summary>
    /// Тест на создание брони зоны (зона не найдена)
    /// </summary>
    [Fact]
    public async Task CreateBookingZoneNotFoundTest()
    {
        // Arrange
        var packages = CreateMockPackages();
        var users = CreateMockUsers();
        var zones = CreateMockZones();
        var bookings = CreateMockBooking(users, zones, packages);

        var expectedBooking = new Booking(Guid.NewGuid(), Guid.NewGuid(), users.First().Id, 
            packages.First().Id, zones.First().Limit, BookingStatus.TemporaryReserved, 
            DateOnly.FromDateTime(DateTime.UtcNow), 
            new TimeOnly(12, 0),
            new TimeOnly(20, 0));
        
        _mockZoneRepository.Setup(s => s.GetZoneByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid zoneId) => zones.First(z => z.Id == zoneId));
        
        _mockPackageRepository.Setup(s => s.GetPackageByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid packageId) => packages.First(p => p.Id == packageId));

        _mockBookingRepository.Setup(s => s.GetAllBookingAsync())
            .ReturnsAsync(bookings);
        
        _mockBookingRepository.Setup(s => s.GetBookingByUserAndZoneAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync((Guid userId, Guid zoneId) =>
                bookings.FindAll(b => b.UserId == userId && b.ZoneId == zoneId));
        
        _mockBookingRepository.Setup(s => s.InsertBookingAsync(It.IsAny<Booking>()))
            .Callback((Booking b) => bookings.Add(b));
        
        // Act
        async Task<Guid> Action() => await _bookingService.AddBookingAsync(expectedBooking.UserId, expectedBooking.ZoneId, expectedBooking.PackageId, expectedBooking.Date, expectedBooking.StartTime, expectedBooking.EndTime);

        // Asserts
        await Assert.ThrowsAsync<ZoneNotFoundException>(Action);
    }
    
    /// <summary>
    /// Тест на создание брони зоны (пакет не найден)
    /// </summary>
    [Fact]
    public async Task CreateBookingPackageNotFoundTest()
    {
        // Arrange
        var packages = CreateMockPackages();
        var users = CreateMockUsers();
        var zones = CreateMockZones();
        var bookings = CreateMockBooking(users, zones, packages);

        var expectedBooking = new Booking(Guid.NewGuid(), zones.First().Id, users.First().Id, 
            Guid.NewGuid(), zones.First().Limit, BookingStatus.TemporaryReserved, 
            DateOnly.FromDateTime(DateTime.UtcNow), 
            new TimeOnly(12, 0),
            new TimeOnly(20, 0));

        _mockZoneRepository.Setup(s => s.GetZoneByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid zoneId) => zones.First(z => z.Id == zoneId));

        _mockPackageRepository.Setup(s => s.GetPackageByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid packageId) => packages.First(p => p.Id == packageId));
        
        _mockBookingRepository.Setup(s => s.GetAllBookingAsync())
            .ReturnsAsync(bookings);
        
        _mockBookingRepository.Setup(s => s.GetBookingByUserAndZoneAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync((Guid userId, Guid zoneId) =>
                bookings.FindAll(b => b.UserId == userId && b.ZoneId == zoneId));
        
        _mockBookingRepository.Setup(s => s.InsertBookingAsync(It.IsAny<Booking>()))
            .Callback((Booking b) => bookings.Add(b));
        
        // Act
        async Task<Guid> Action() => await _bookingService.AddBookingAsync(expectedBooking.UserId, expectedBooking.ZoneId, expectedBooking.PackageId, expectedBooking.Date, expectedBooking.StartTime, expectedBooking.EndTime);

        // Asserts
        await Assert.ThrowsAsync<PackageNotFoundException>(Action);
    }
    
    /// <summary>
    /// Тесты на определение незабронированного времени 
    /// </summary>
    [Theory]
    [InlineData("2002.06.23", "8:00", "12:00", false)]
    [InlineData("2002.06.23", "16:00", "20:00", false)]
    [InlineData("2002.05.07", "8:00", "12:00", false)]
    [InlineData("2002.05.07", "10:40", "11:00", false)]
    [InlineData("2002.05.07", "16:00", "20:00", false)]
    [InlineData("2023.08.21", "16:00", "20:00", false)]
    [InlineData("2023.08.21", "15:00", "19:00", false)]
    [InlineData("2023.09.21", "12:00", "16:00", true)]
    [InlineData("2023.09.21", "22:00", "23:00", true)]
    [InlineData("2023.09.21", "18:00", "23:00", true)]
    [InlineData("2023.09.21", "22:00", "23:30", false)]
    public async Task IsFreeTimeTest(string date, string startTime, string endTime, bool expectedResult)
    {
        // Arrange
        var bookings = new List<Booking>()
        {
            // 2002.05.07
            new Booking(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 
                10, BookingStatus.NoActual,
                new DateOnly(2002, 05, 7), 
                new TimeOnly(8 , 0),
                new TimeOnly(10, 40)),
            new Booking(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 
                10, BookingStatus.NoActual,
                new DateOnly(2002, 05, 7), 
                new TimeOnly(12 , 0),
                new TimeOnly(15, 30)),
            new Booking(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 
                10, BookingStatus.NoActual,
                new DateOnly(2002, 05, 7), 
                new TimeOnly(11 , 0),
                new TimeOnly(12, 00)),
            
            // Today
            new Booking(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 
                15, BookingStatus.Reserved,
                DateOnly.FromDateTime(DateTime.Today), 
                new TimeOnly(10 , 0),
                new TimeOnly(12, 00)),
            new Booking(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 
                20, BookingStatus.Reserved,
                DateOnly.FromDateTime(DateTime.Today), 
                new TimeOnly(16 , 0),
                new TimeOnly(22, 00))
        };
        
        _mockBookingRepository.Setup(s => s.GetAllBookingAsync())
            .ReturnsAsync(bookings);
        
        // Act
        var actualResult = await _bookingService.IsFreeTimeAsync(DateOnly.Parse(date), 
            TimeOnly.Parse(startTime),
            TimeOnly.Parse(endTime));
        
        // Assert
        Assert.Equal(expectedResult, actualResult);
    }
    
    /// <summary>
    /// Тесты на определение незабронированного времени 
    /// </summary>
    [Theory]
    [InlineData("2002.06.23", "16:00", "20:00", false)]
    [InlineData("2002.05.07", "8:00", "12:00", false)]
    [InlineData("2023.07.21", "16:00", "20:00", false)]
    [InlineData("2023.07.21", "15:00", "19:00", false)]
    [InlineData("2023.07.21", "18:00", "23:00", false)]
    public async Task IsFreeTimeEmptyTest(string date, string startTime, string endTime, bool expectedResult)
     {
         // Arrange
         var bookings = new List<Booking>();
         
         _mockBookingRepository.Setup(s => s.GetAllBookingAsync())
             .ReturnsAsync(bookings);
         
         // Act
         var actualResult = await _bookingService.IsFreeTimeAsync(DateOnly.Parse(date), 
             TimeOnly.Parse(startTime),
             TimeOnly.Parse(endTime));
         
         // Assert
         Assert.Equal(expectedResult, actualResult);
     }

    /// <summary>
    /// Тест на поиск незабронированного времени
    /// </summary>
    [Fact]
    public async Task GetFreeTimeTest1()
    {
        // Arrange
        var packages = CreateMockPackages();
        var zones = CreateMockZones();
        var zoneId = zones.First().Id;
        var day = DateOnly.FromDateTime(DateTime.UtcNow + new TimeSpan(1, 0, 0 ,0));
        var bookings = new List<Booking>()
        {
            // 2002.05.07
            new Booking(Guid.NewGuid(), zoneId, Guid.NewGuid(), Guid.NewGuid(), 
                10, BookingStatus.NoActual,
                new DateOnly(2002, 05, 7), 
                new TimeOnly(8 , 0),
                new TimeOnly(10, 40)),
            new Booking(Guid.NewGuid(), zoneId, Guid.NewGuid(), Guid.NewGuid(), 
                10, BookingStatus.NoActual,
                new DateOnly(2002, 05, 7), 
                new TimeOnly(14 , 0),
                new TimeOnly(18, 30)),
            new Booking(Guid.NewGuid(), zoneId, Guid.NewGuid(), Guid.NewGuid(), 
                10, BookingStatus.NoActual,
                new DateOnly(2002, 05, 7), 
                new TimeOnly(11 , 0),
                new TimeOnly(12, 00)),
            // Today
            new Booking(Guid.NewGuid(), zoneId, Guid.NewGuid(), Guid.NewGuid(), 
                15, BookingStatus.Reserved,
                day, 
                new TimeOnly(13 , 00),
                new TimeOnly(15, 40)),
            new Booking(Guid.NewGuid(), zoneId, Guid.NewGuid(), Guid.NewGuid(), 
                20, BookingStatus.Reserved,
                day, 
                new TimeOnly(16 , 0),
                new TimeOnly(20, 00)),
            new Booking(Guid.NewGuid(), zoneId, Guid.NewGuid(), Guid.NewGuid(), 
                20, BookingStatus.Reserved,
                day, 
                new TimeOnly(10, 0),
                new TimeOnly(11, 20))
        };
        
        _mockBookingRepository.Setup(s => s.GetBookingByZoneAsync(It.IsAny<Guid>()))
            .ReturnsAsync(bookings);
        
        var expectedFreeTime = new List<FreeTime>()
        {
            new FreeTime(new TimeOnly(8, 00), new TimeOnly(10, 00)),
            new FreeTime(new TimeOnly(11, 20), new TimeOnly(13, 00)),
            new FreeTime(new TimeOnly(20, 00), new TimeOnly(23, 00))
        };
    
        // Act
        var actualFreeTime = await _bookingService.GetFreeTimeAsync(zoneId, day);

        // Assert
        Assert.Equal(expectedFreeTime.Count, actualFreeTime.Count);
        Assert.Equal(expectedFreeTime.FirstOrDefault()?.StartTime, actualFreeTime.FirstOrDefault()?.StartTime);
        Assert.Equal(expectedFreeTime.FirstOrDefault()?.EndTime, actualFreeTime.FirstOrDefault()?.EndTime);
        Assert.Equal(expectedFreeTime.LastOrDefault()?.StartTime, actualFreeTime.LastOrDefault()?.StartTime);
        Assert.Equal(expectedFreeTime.LastOrDefault()?.EndTime, actualFreeTime.LastOrDefault()?.EndTime);
    }
    
     /// <summary>
    /// Тест на поиск незабронированного времени
    /// </summary>
    [Fact]
    public async Task GetFreeTimeTest2()
    {
        // Arrange
        var packages = CreateMockPackages();
        var zones = CreateMockZones();
        var zoneId = zones.First().Id;
        var day = DateOnly.FromDateTime(DateTime.UtcNow + new TimeSpan(1, 0, 0 ,0));
        var bookings = new List<Booking>()
        {
            // 2002.05.07
            new Booking(Guid.NewGuid(), zoneId, Guid.NewGuid(), Guid.NewGuid(), 
                10, BookingStatus.NoActual,
                new DateOnly(2002, 05, 7), 
                new TimeOnly(8 , 0),
                new TimeOnly(10, 40)),
            new Booking(Guid.NewGuid(), zoneId, Guid.NewGuid(), Guid.NewGuid(), 
                10, BookingStatus.NoActual,
                new DateOnly(2002, 05, 7), 
                new TimeOnly(14 , 0),
                new TimeOnly(18, 30)),
            new Booking(Guid.NewGuid(), zoneId, Guid.NewGuid(), Guid.NewGuid(), 
                10, BookingStatus.NoActual,
                new DateOnly(2002, 05, 7), 
                new TimeOnly(11 , 0),
                new TimeOnly(12, 00)),
            // Today
            new Booking(Guid.NewGuid(), zoneId, Guid.NewGuid(), Guid.NewGuid(), 
                15, BookingStatus.Reserved,
                day, 
                new TimeOnly(12 , 00),
                new TimeOnly(15, 40)),
            new Booking(Guid.NewGuid(), zoneId, Guid.NewGuid(), Guid.NewGuid(), 
                20, BookingStatus.Reserved,
                day, 
                new TimeOnly(16 , 0),
                new TimeOnly(22, 00)),
            new Booking(Guid.NewGuid(), zoneId, Guid.NewGuid(), Guid.NewGuid(), 
                20, BookingStatus.Reserved,
                day, 
                new TimeOnly(8, 0),
                new TimeOnly(10, 30))
        };
        
        _mockBookingRepository.Setup(s => s.GetBookingByZoneAsync(It.IsAny<Guid>()))
            .ReturnsAsync(bookings);
        
        var expectedFreeTime = new List<FreeTime>()
        {
            new FreeTime(new TimeOnly(10, 30), new TimeOnly(12, 00)),
            new FreeTime(new TimeOnly(22, 00), new TimeOnly(23, 00))
        };
    
        // Act
        var actualFreeTime = await _bookingService.GetFreeTimeAsync(zoneId, day);

        // Assert
        Assert.Equal(expectedFreeTime.Count, actualFreeTime.Count);
        Assert.Equal(expectedFreeTime.FirstOrDefault()?.StartTime, actualFreeTime.FirstOrDefault()?.StartTime);
        Assert.Equal(expectedFreeTime.FirstOrDefault()?.EndTime, actualFreeTime.FirstOrDefault()?.EndTime);
        Assert.Equal(expectedFreeTime.LastOrDefault()?.StartTime, actualFreeTime.LastOrDefault()?.StartTime);
        Assert.Equal(expectedFreeTime.LastOrDefault()?.EndTime, actualFreeTime.LastOrDefault()?.EndTime);
    }

     /// <summary>
     /// Тест на изменении статуса брони
     /// </summary>
     [Theory]
     [InlineData(BookingStatus.TemporaryReserved, BookingStatus.Reserved)]
     [InlineData(BookingStatus.TemporaryReserved, BookingStatus.NoActual)]
     [InlineData(BookingStatus.TemporaryReserved, BookingStatus.Cancelled)]
     [InlineData(BookingStatus.Reserved, BookingStatus.NoActual)]
     [InlineData(BookingStatus.Reserved, BookingStatus.Cancelled)]
     public async Task ChangeBookingStatusTest(BookingStatus oldStatus, BookingStatus newStatus)
     {
         // Arrange
         var packages = CreateMockPackages();
         var users = CreateMockUsers();
         var zones = CreateMockZones();
         var bookings = CreateMockBooking(users, zones, packages);

         _mockBookingRepository.Setup(s => s.GetBookingByIdAsync(It.IsAny<Guid>()))
             .ReturnsAsync((Guid id) => bookings.First(b => b.Id == id));

         _mockBookingRepository.Setup(s => s.UpdateBookingAsync(It.IsAny<Booking>()))
             .Callback((Booking b) =>
             {
                 var booking = bookings.First(e => e.Id == b.Id);
                 booking.UserId = b.UserId;
                 booking.ZoneId = b.ZoneId;
                 booking.PackageId = b.PackageId;
                 booking.AmountPeople = b.AmountPeople;
                 booking.Status = b.Status;
                 booking.Date = b.Date;
                 booking.StartTime = b.StartTime;
                 booking.EndTime = b.EndTime;
             });

         // Act
         var bookingId = bookings.FirstOrDefault(b => b.Status == oldStatus)!.Id;
         await _bookingService.ChangeBookingStatusAsync(bookingId, newStatus);

         var actualBooking = bookings.First(b => b.Id == bookingId);
         // Assert
         Assert.Equal(newStatus, actualBooking.Status);
     }
     
     /// <summary>
     /// Тест на изменении статуса брони
     /// </summary>
     [Theory]
     [InlineData(BookingStatus.Reserved, BookingStatus.TemporaryReserved)]
     [InlineData(BookingStatus.NoActual, BookingStatus.TemporaryReserved)]
     [InlineData(BookingStatus.NoActual, BookingStatus.Reserved)]
     [InlineData(BookingStatus.Cancelled, BookingStatus.Reserved)]
     public async Task ChangeBookingStatusNotSuitableTest(BookingStatus oldStatus, BookingStatus newStatus)
     {
         // Arrange
         var packages = CreateMockPackages();
         var users = CreateMockUsers();
         var zones = CreateMockZones();
         var bookings = CreateMockBooking(users, zones, packages);

         _mockBookingRepository.Setup(s => s.GetBookingByIdAsync(It.IsAny<Guid>()))
             .ReturnsAsync((Guid id) => bookings.First(b => b.Id == id));

         _mockBookingRepository.Setup(s => s.UpdateBookingAsync(It.IsAny<Booking>()))
             .Callback((Booking b) =>
             {
                 var booking = bookings.First(e => e.Id == b.Id);
                 booking.UserId = b.UserId;
                 booking.ZoneId = b.ZoneId;
                 booking.PackageId = b.PackageId;
                 booking.AmountPeople = b.AmountPeople;
                 booking.Status = b.Status;
                 booking.Date = b.Date;
                 booking.StartTime = b.StartTime;
                 booking.EndTime = b.EndTime;
             });

         // Act
         var bookingId = bookings.FirstOrDefault(b => b.Status == oldStatus)!.Id;
         async Task Action() => await _bookingService.ChangeBookingStatusAsync(bookingId, newStatus);
         
         // Assert
         await Assert.ThrowsAsync<BookingNotSuitableStatusException>(Action);
     }
     
     /// <summary>
     /// Тест на обновления брони
     /// </summary>
     [Fact]
     public async Task UpdateBookingTest()
     {
         // Arrange
         var packages = CreateMockPackages();
         var users = CreateMockUsers();
         var zones = CreateMockZones();
         var bookings = CreateMockBooking(users, zones, packages);
         var booking = bookings.First();
         var expectedBooking = new Booking(booking.Id, booking.ZoneId, booking.UserId, booking.PackageId,
             15, booking.Status, booking.Date, booking.StartTime, booking.EndTime);

         _mockBookingRepository.Setup(s => s.GetBookingByIdAsync(It.IsAny<Guid>()))
             .ReturnsAsync((Guid id) => bookings.First(b => b.Id == id));

         _mockBookingRepository.Setup(s => s.UpdateBookingAsync(It.IsAny<Booking>()))
             .Callback((Booking b) =>
             {
                 bookings.FindAll(e => e.Id == b.Id).ForEach(e =>
                 {
                     e.UserId = b.UserId;
                     e.ZoneId = b.ZoneId;
                     e.PackageId = b.PackageId;
                     e.AmountPeople = b.AmountPeople;
                     e.Status = b.Status;
                     e.Date = b.Date;
                     e.StartTime = b.StartTime;
                     e.EndTime = b.EndTime;
                 });
             });
        
         // Act
         await _bookingService.UpdateBookingAsync(expectedBooking);
         var actualBooking = bookings.First(b => b.Id == booking.Id);
         
         // Assert
         Assert.StrictEqual(expectedBooking.AmountPeople, actualBooking.AmountPeople);
     }
     
     /// <summary>
     /// Тест на обновления брони
     /// </summary>
     [Fact]
     public async Task UpdateBookingNotFoundTest()
     {
         // Arrange
         var packages = CreateMockPackages();
         var users = CreateMockUsers();
         var zones = CreateMockZones();
         var bookings = CreateMockBooking(users, zones, packages);
         var booking = bookings.First();
         var expectedBooking = new Booking(Guid.NewGuid(), booking.ZoneId, booking.UserId, booking.PackageId,
             20, booking.Status, booking.Date, booking.StartTime, booking.EndTime);

         _mockBookingRepository.Setup(s => s.GetBookingByIdAsync(It.IsAny<Guid>()))
             .ReturnsAsync((Guid id) => bookings.First(b => b.Id == id));

         _mockZoneRepository.Setup(s => s.GetZoneByIdAsync(It.IsAny<Guid>()))
             .ReturnsAsync((Guid id) => zones.First(z => z.Id == id));
         
         _mockBookingRepository.Setup(s => s.UpdateBookingAsync(It.IsAny<Booking>()))
             .Callback((Booking b) =>
             {
                 bookings.FindAll(e => e.Id == b.Id).ForEach(e =>
                 {
                     e.UserId = b.UserId;
                     e.ZoneId = b.ZoneId;
                     e.PackageId = b.PackageId;
                     e.AmountPeople = b.AmountPeople;
                     e.Status = b.Status;
                     e.Date = b.Date;
                     e.StartTime = b.StartTime;
                     e.EndTime = b.EndTime;
                 });
             });
        
         // Act
         async Task Action() => await _bookingService.UpdateBookingAsync(expectedBooking);
         
         // Assert
         await Assert.ThrowsAsync<BookingNotFoundException>(Action);
     }
     
     /// <summary>
     /// Тест на обновления брони
     /// </summary>
     [Fact]
     public async Task UpdateBookingExceedsLimitTest()
     {
         // Arrange
         var packages = CreateMockPackages();
         var users = CreateMockUsers();
         var zones = CreateMockZones();
         var bookings = CreateMockBooking(users, zones, packages);
         var booking = bookings.First();
         var expectedBooking = new Booking(booking.Id, booking.ZoneId, booking.UserId, booking.PackageId,
             100, booking.Status, booking.Date, booking.StartTime, booking.EndTime);

         _mockBookingRepository.Setup(s => s.GetBookingByIdAsync(It.IsAny<Guid>()))
             .ReturnsAsync((Guid id) => bookings.First(b => b.Id == id));

         _mockZoneRepository.Setup(s => s.GetZoneByIdAsync(It.IsAny<Guid>()))
             .ReturnsAsync((Guid id) => zones.First(z => z.Id == id));
         
         _mockBookingRepository.Setup(s => s.UpdateBookingAsync(It.IsAny<Booking>()))
             .Callback((Booking b) =>
             {
                 bookings.FindAll(e => e.Id == b.Id).ForEach(e =>
                 {
                     e.UserId = b.UserId;
                     e.ZoneId = b.ZoneId;
                     e.PackageId = b.PackageId;
                     e.AmountPeople = b.AmountPeople;
                     e.Status = b.Status;
                     e.Date = b.Date;
                     e.StartTime = b.StartTime;
                     e.EndTime = b.EndTime;
                 });
             });
        
         // Act
         async Task Action() => await _bookingService.UpdateBookingAsync(expectedBooking);
         
         // Assert
         await Assert.ThrowsAsync<BookingExceedsLimitException>(Action);
     }
     
     /// <summary>
     /// Тест на обновления брони
     /// </summary>
     [Fact]
     public async Task UpdateBookingUpdateDateTest()
     {
         // Arrange
         var packages = CreateMockPackages();
         var users = CreateMockUsers();
         var zones = CreateMockZones();
         var bookings = CreateMockBooking(users, zones, packages);
         var booking = bookings.First();
         var expectedBooking = new Booking(booking.Id, booking.ZoneId, booking.UserId, booking.PackageId,
             15, booking.Status, new DateOnly(2002, 02, 23), booking.StartTime, booking.EndTime);

         _mockBookingRepository.Setup(s => s.GetBookingByIdAsync(It.IsAny<Guid>()))
             .ReturnsAsync((Guid id) => bookings.First(b => b.Id == id));

         _mockBookingRepository.Setup(s => s.UpdateBookingAsync(It.IsAny<Booking>()))
             .Callback((Booking b) =>
             {
                 bookings.FindAll(e => e.Id == b.Id).ForEach(e =>
                 {
                     e.UserId = b.UserId;
                     e.ZoneId = b.ZoneId;
                     e.PackageId = b.PackageId;
                     e.AmountPeople = b.AmountPeople;
                     e.Status = b.Status;
                     e.Date = b.Date;
                     e.StartTime = b.StartTime;
                     e.EndTime = b.EndTime;
                 });
             });
        
         // Act
         async Task Action() => await _bookingService.UpdateBookingAsync(expectedBooking);
         
         // Assert
         await Assert.ThrowsAsync<BookingChangeDateTimeException>(Action);
     }
     
    /// <summary>
    /// Тест на удаление брони зоны
    /// </summary>
    [Fact]
    public async void RemoveBookingTest()
    {
        // Arrange
        var packages = CreateMockPackages();
        var users = CreateMockUsers();
        var zones = CreateMockZones();
        var bookings = CreateMockBooking(users, zones, packages);
        var expectedCount = bookings.Count - 1;
    
        var booking = bookings.First();
        
        _mockBookingRepository.Setup(s => s.GetBookingByIdAsync(It.IsAny<Guid>()))
                              .ReturnsAsync(booking);
        _mockBookingRepository.Setup(s => s.DeleteBookingAsync(It.IsAny<Guid>()))
                              .Callback((Guid id) =>
                              {
                                  var b = bookings.First(b => b.Id == id);
                                  bookings.Remove(b);
                              });
        // Act
        await _bookingService.RemoveBookingAsync(booking.Id);
        var actualCount = bookings.Count;
    
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.NotEqual(booking, bookings.First());
    }
    
    /// <summary>
    /// Тест на удаление брони зоны
    /// </summary>
    [Fact]
    public async void RemoveBookingNotFoundTest()
    {
        // Arrange
        var packages = CreateMockPackages();
        var users = CreateMockUsers();
        var zones = CreateMockZones();
        var bookings = CreateMockBooking(users, zones, packages);
        var expectedCount = bookings.Count;
        var bookingId = Guid.NewGuid();
        
        _mockBookingRepository.Setup(s => s.DeleteBookingAsync(It.IsAny<Guid>()))
            .Callback((Guid id) =>
            {
                var b = bookings.First(b => b.Id == id);
                bookings.Remove(b);
            });
        // Act
        async Task Action() => await _bookingService.RemoveBookingAsync(bookingId);
        var actualCount = bookings.Count;
    
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        await Assert.ThrowsAsync<BookingNotFoundException>(Action);
    }
    
    private List<Package> CreateMockPackages()
    {
        return new List<Package>()
        {
            new Package(Guid.NewGuid(), "Почасовая аренда", PackageType.Usual, 350, 2,
                "Почасовая стоимость аренды зала для компании людей", new List<Zone>(), new List<Dish>()),
            new Package(Guid.NewGuid(), "Пакет \"Для своих\"", PackageType.Simple, 999, 3,
                "Почасовая стоимость аренды зала для компании людей", new List<Zone>(), new List<Dish>())
        };
    }

    private List<Zone> CreateMockZones()
    {
        return new List<Zone>
        {
            new Zone(Guid.NewGuid(), "Zone1", "address1", 10, 10, 250, 0.0, new List<Inventory>(), new List<Package>()),
            new Zone(Guid.NewGuid(), "Zone2", "address2", 30, 10, 350, 0.0, new List<Inventory>(), new List<Package>()),
            new Zone(Guid.NewGuid(), "Zone3", "address3", 25, 10, 300, 0.0, new List<Inventory>(), new List<Package>()),
            new Zone(Guid.NewGuid(), "Zone3", "address3", 25, 10, 300, 0.0, new List<Inventory>(), new List<Package>())
        };
    }
    
    private List<User> CreateMockUsers()
    {
        return new List<User>()
        {
            new User(Guid.NewGuid(), "Иванов", "Иван", "Иванович",  new DateTime(2002, 03, 17), Gender.Male, "ivanov@mail.ru", "+7899999999", "password12123434"),
            new User(Guid.NewGuid(), "Петров", "Петр", "Петрович",  new DateTime(2003, 05, 18), Gender.Male, "petrov@mail.ru", "+7899909999", "password12122323"),
            new User(Guid.NewGuid(), "Cударь", "Елена", "Александровна",  new DateTime(1999, 09, 18), Gender.Female, "sudar@mail.ru", "+781211111", "password12121212")
        };
    }
    
    private List<Booking> CreateMockBooking(List<User> users, List<Zone> zones, List<Package> packages)
    {
        return new List<Booking>()
        {
            new Booking(Guid.NewGuid(), zones[0].Id, users[0].Id, packages[0].Id,
                10, BookingStatus.Reserved,
                DateOnly.FromDateTime(DateTime.Today),
                new TimeOnly(8, 00), 
                new TimeOnly(12, 00)),
            new Booking(Guid.NewGuid(), zones[2].Id, users[1].Id, packages[1].Id, 
                10, BookingStatus.Reserved, 
                DateOnly.FromDateTime(DateTime.UtcNow + new TimeSpan(1, 0, 0, 0)),
                new TimeOnly(13, 00, 00), 
                new TimeOnly(15, 00, 00)),
            new Booking(Guid.NewGuid(), zones[2].Id, users[1].Id, packages[1].Id, 
                10, BookingStatus.TemporaryReserved, 
                DateOnly.FromDateTime(DateTime.UtcNow + new TimeSpan(5, 0, 0, 0)),
                new TimeOnly(13, 00, 00), 
                new TimeOnly(15, 00, 00)),
            new Booking(Guid.NewGuid(), zones[0].Id, users[2].Id, packages[0].Id, 
                10, BookingStatus.NoActual,
                DateOnly.Parse("2023.05.20"),
                new TimeOnly(12, 00), 
                new TimeOnly(18, 00)),
            new Booking(Guid.NewGuid(), zones[0].Id, users[0].Id, packages[0].Id, 
                10, BookingStatus.Reserved, 
                DateOnly.FromDateTime(DateTime.UtcNow  + new TimeSpan(2, 0, 0, 0)),
                new TimeOnly(12, 00), 
                new TimeOnly(18, 00)),
            new Booking(Guid.NewGuid(), zones[0].Id, users[0].Id, packages[0].Id, 
                10, BookingStatus.Cancelled, 
                DateOnly.FromDateTime(DateTime.UtcNow  + new TimeSpan(6, 0, 0, 0)),
                new TimeOnly(12, 00), 
                new TimeOnly(18, 00)),
            
            // Create Reversed Test
            new Booking(Guid.NewGuid(), zones[0].Id, users[2].Id, packages[0].Id, 
                10, BookingStatus.Cancelled, 
                DateOnly.FromDateTime(DateTime.UtcNow  + new TimeSpan(20, 0, 0, 0)),
                new TimeOnly(08, 00), 
                new TimeOnly(11, 00)),
            new Booking(Guid.NewGuid(), zones[0].Id, users[1].Id, packages[0].Id, 
                10, BookingStatus.Cancelled, 
                DateOnly.FromDateTime(DateTime.UtcNow  + new TimeSpan(20, 0, 0, 0)),
                new TimeOnly(12, 00), 
                new TimeOnly(16, 00)),
            new Booking(Guid.NewGuid(), zones[0].Id, Guid.Empty, packages[0].Id, 
                10, BookingStatus.Cancelled, 
                DateOnly.FromDateTime(DateTime.UtcNow  + new TimeSpan(20, 0, 0, 0)),
                new TimeOnly(18, 00), 
                new TimeOnly(23, 00)),
        };
    }
}
