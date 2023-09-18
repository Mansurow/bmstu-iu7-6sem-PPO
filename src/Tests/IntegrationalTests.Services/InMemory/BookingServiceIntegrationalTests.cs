using IntegrationalTests.Services.AccessObject;
using Microsoft.Extensions.Logging.Abstractions;
using Portal.Common.Models;
using Portal.Services.BookingService;
using Portal.Services.BookingService.Exceptions;
using Xunit;

namespace IntegrationalTests.Services.InMemory;

public class BookingServiceIntegrationalTests
{
    private readonly IBookingService _bookingService;
    private readonly AccessObjectInMemory _accessObject;
    
    public BookingServiceIntegrationalTests()
    {
        _accessObject = new AccessObjectInMemory();
        _bookingService = new BookingService(_accessObject.BookingRepository,
            _accessObject.PackageRepository,
            _accessObject.ZoneRepository,
            NullLogger<BookingService>.Instance,
            _accessObject.BookingServiceConfiguration);
    }

    [Fact]
    public async Task GetAllBookingsOkTest()
    {
        // Arrange
        var packages = _accessObject.CreateMockPackages();
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones(packages);
        var bookings = _accessObject.CreateMockBookings(users, zones, packages);

        var expectedCount = bookings.Count;

        await _accessObject.InsertManyUsersAsync(users);
        await _accessObject.InsertManyPackagesAsync(packages);
        await _accessObject.InsertManyZonesAsync(zones);
        await _accessObject.InsertManyBookingsAsync(bookings);
        
        // Act
        var actualBookings = await _bookingService.GetAllBookingAsync();
        var actualCount = actualBookings.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        // Assert.Equal(bookings, actualBookings);
    }
    
    [Fact]
    public async Task GetAllBookingsEmptyTest()
    {
        // Arrange
        var packages = _accessObject.CreateMockPackages();
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones(packages);
        var bookings = _accessObject.CreateEmptyMockBookings();

        var expectedCount = bookings.Count;

        await _accessObject.InsertManyUsersAsync(users);
        await _accessObject.InsertManyPackagesAsync(packages);
        await _accessObject.InsertManyZonesAsync(zones);
        await _accessObject.InsertManyBookingsAsync(bookings);
        
        // Act
        var actualBookings = await _bookingService.GetAllBookingAsync();
        var actualCount = actualBookings.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.Equal(bookings, actualBookings);
    }
    
    [Fact]
    public async Task GetBookingsByUserOkTest()
    {
        // Arrange
        var packages = _accessObject.CreateMockPackages();
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones(packages);
        var bookings = _accessObject.CreateMockBookings(users, zones, packages);

        var expectedUser = users.First();
        var expectedBookings = bookings.Where(e => e.UserId == expectedUser.Id).ToList();
        var expectedCount = expectedBookings.Count;

        await _accessObject.InsertManyUsersAsync(users);
        await _accessObject.InsertManyPackagesAsync(packages);
        await _accessObject.InsertManyZonesAsync(zones);
        await _accessObject.InsertManyBookingsAsync(bookings);
        
        // Act
        var actualBookings = await _bookingService.GetBookingByUserAsync(expectedUser.Id);
        var actualCount = actualBookings.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.Equal(expectedBookings, actualBookings);
    }
    
    [Fact]
    public async Task GetBookingsByUserEmptyTest()
    {
        // Arrange
        var packages = _accessObject.CreateMockPackages();
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones(packages);
        var bookings = _accessObject.CreateEmptyMockBookings();

        var expectedUser = users.First();
        var expectedBookings = bookings.Where(e => e.UserId == expectedUser.Id).ToList();
        var expectedCount = expectedBookings.Count;

        await _accessObject.InsertManyUsersAsync(users);
        await _accessObject.InsertManyPackagesAsync(packages);
        await _accessObject.InsertManyZonesAsync(zones);
        await _accessObject.InsertManyBookingsAsync(bookings);
        
        // Act
        var actualBookings = await _bookingService.GetBookingByUserAsync(expectedUser.Id);
        var actualCount = actualBookings.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.Equal(expectedBookings, actualBookings);
    }
    
    [Fact]
    public async Task GetBookingsByZoneOkTest()
    {
        // Arrange
        var packages = _accessObject.CreateMockPackages();
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones(packages);
        var bookings = _accessObject.CreateMockBookings(users, zones, packages);

        var expectedZone = zones.First();
        var expectedBookings = bookings.Where(e => e.ZoneId == expectedZone.Id).ToList();
        var expectedCount = expectedBookings.Count;

        await _accessObject.InsertManyUsersAsync(users);
        await _accessObject.InsertManyPackagesAsync(packages);
        await _accessObject.InsertManyZonesAsync(zones);
        await _accessObject.InsertManyBookingsAsync(bookings);
        
        // Act
        var actualBookings = await _bookingService.GetBookingByZoneAsync(expectedZone.Id);
        var actualCount = actualBookings.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        // Assert.Equal(expectedBookings, actualBookings);
    }
    
    [Fact]
    public async Task GetBookingsByZoneEmptyTest()
    {
        // Arrange
        var packages = _accessObject.CreateMockPackages();
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones(packages);
        var bookings = _accessObject.CreateEmptyMockBookings();

        var expectedZone = zones.First();
        var expectedBookings = bookings.Where(e => e.ZoneId == expectedZone.Id).ToList();
        var expectedCount = expectedBookings.Count;

        await _accessObject.InsertManyUsersAsync(users);
        await _accessObject.InsertManyPackagesAsync(packages);
        await _accessObject.InsertManyZonesAsync(zones);
        await _accessObject.InsertManyBookingsAsync(bookings);
        
        // Act
        var actualBookings = await _bookingService.GetBookingByZoneAsync(expectedZone.Id);
        var actualCount = actualBookings.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        Assert.Equal(expectedBookings, actualBookings);
    }
    
    [Fact]
    public async Task GetBookingOkTest()
    {
        // Arrange
        var packages = _accessObject.CreateMockPackages();
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones(packages);
        var bookings = _accessObject.CreateMockBookings(users, zones, packages);
        
        var expectedBooking = bookings.First();

        await _accessObject.InsertManyUsersAsync(users);
        await _accessObject.InsertManyPackagesAsync(packages);
        await _accessObject.InsertManyZonesAsync(zones);
        await _accessObject.InsertManyBookingsAsync(bookings);
        
        // Act
        var actualBooking = await _bookingService.GetBookingByIdAsync(expectedBooking.Id);

        // Asserts
        Assert.Equal(expectedBooking, actualBooking);
    }
    
    [Fact]
    public async Task GetBookingNotFoundTest()
    {
        // Arrange
        var packages = _accessObject.CreateMockPackages();
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones(packages);
        var bookings = _accessObject.CreateMockBookings(users, zones, packages);
        
        var expectedBookingId = Guid.NewGuid();

        await _accessObject.InsertManyUsersAsync(users);
        await _accessObject.InsertManyPackagesAsync(packages);
        await _accessObject.InsertManyZonesAsync(zones);
        await _accessObject.InsertManyBookingsAsync(bookings);
        
        // Act
        Task<Booking> Action() => _bookingService.GetBookingByIdAsync(expectedBookingId);

        // Asserts
        await Assert.ThrowsAsync<BookingNotFoundException>(Action);
    }

    [Fact]
    public async Task GetFreeTimeOkTest()
    {
        // Arrange
        var packages = _accessObject.CreateMockPackages();
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones(packages);
        var bookings = _accessObject.CreateMockBookings(users, zones, packages);
        
        await _accessObject.InsertManyUsersAsync(users);
        await _accessObject.InsertManyPackagesAsync(packages);
        await _accessObject.InsertManyZonesAsync(zones);
        await _accessObject.InsertManyBookingsAsync(bookings);

        var zone = zones.Last();
        var expectedListFreeTime = new List<FreeTime>()
        {
            new FreeTime("8:00:00", "23:00:00")
        };
        
        // Act
        var actualListFreeTime = await _bookingService.GetFreeTimeAsync(zone.Id, DateOnly.FromDateTime(DateTime.UtcNow + new TimeSpan(1, 0, 0, 0)));

        // Asserts
        Assert.Equal(expectedListFreeTime, actualListFreeTime);
    }
    
    [Fact]
    public async Task AddBookingOkTest()
    {
        // Arrange
        var packages = _accessObject.CreateMockPackages();
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones(packages);
        var bookings = _accessObject.CreateMockBookings(users, zones, packages);
        
        await _accessObject.InsertManyUsersAsync(users);
        await _accessObject.InsertManyPackagesAsync(packages);
        await _accessObject.InsertManyZonesAsync(zones);
        await _accessObject.InsertManyBookingsAsync(bookings);

        var zone = zones.Last();
        var user = users.Last();
        var package = packages.First();
        var expectedCount = bookings.Count + 1;

        // Act
        var bookingId = await _bookingService.AddBookingAsync(user.Id, zone.Id, package.Id, 
            DateOnly.FromDateTime(DateTime.Today), TimeOnly.Parse("12:00"), TimeOnly.Parse("13:00"));

        var actualBookings = await _bookingService.GetAllBookingAsync();
        var actualCount = actualBookings.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
    }
    
    [Fact]
    public async Task AddBookingExistsForUserTest()
    {
        // Arrange
        var packages = _accessObject.CreateMockPackages();
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones(packages);
        var bookings = _accessObject.CreateMockBookings(users, zones, packages);
        
        await _accessObject.InsertManyUsersAsync(users);
        await _accessObject.InsertManyPackagesAsync(packages);
        await _accessObject.InsertManyZonesAsync(zones);
        await _accessObject.InsertManyBookingsAsync(bookings);

        var zone = zones.First();
        var user = users.First();
        var package = packages.First();
        var expectedCount = bookings.Count;

        // Act
        Task<Guid> Action() => _bookingService.AddBookingAsync(user.Id, zone.Id, package.Id, 
            DateOnly.FromDateTime(DateTime.Today), TimeOnly.Parse("12:00"), TimeOnly.Parse("14:00"));

        var actualBookings = await _bookingService.GetAllBookingAsync();
        var actualCount = actualBookings.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        await Assert.ThrowsAsync<BookingExistsException>(Action);
    }
    
    [Fact]
    public async Task AddBookingReversedTimeTest()
    {
        // Arrange
        var packages = _accessObject.CreateMockPackages();
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones(packages);
        var bookings = _accessObject.CreateMockBookings(users, zones, packages);
        
        await _accessObject.InsertManyUsersAsync(users);
        await _accessObject.InsertManyPackagesAsync(packages);
        await _accessObject.InsertManyZonesAsync(zones);
        await _accessObject.InsertManyBookingsAsync(bookings);

        var zone = zones.First();
        var user = users.Last();
        var package = packages.First();
        var expectedCount = bookings.Count;

        // Act
        Task<Guid> Action() => _bookingService.AddBookingAsync(user.Id, zone.Id, package.Id, 
            DateOnly.FromDateTime(DateTime.Today), 
            TimeOnly.Parse("8:00"), TimeOnly.Parse("14:00"));

        var actualBookings = await _bookingService.GetAllBookingAsync();
        var actualCount = actualBookings.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        await Assert.ThrowsAsync<BookingReversedException>(Action);
    }
    
    [Fact]
    public async Task RemoveBookingOkTest()
    {
        // Arrange
        var packages = _accessObject.CreateMockPackages();
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones(packages);
        var bookings = _accessObject.CreateMockBookings(users, zones, packages);
        
        await _accessObject.InsertManyUsersAsync(users);
        await _accessObject.InsertManyPackagesAsync(packages);
        await _accessObject.InsertManyZonesAsync(zones);
        await _accessObject.InsertManyBookingsAsync(bookings);

        var booking = bookings.First();
        var expectedCount = bookings.Count - 1;
        
        // Act
        await _bookingService.RemoveBookingAsync(booking.Id);

        var actualBookings = await _bookingService.GetAllBookingAsync();
        var actualCount = actualBookings.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
    }
    
    [Fact]
    public async Task RemoveBookingNotFoundTest()
    {
        // Arrange
        var packages = _accessObject.CreateMockPackages();
        var users = _accessObject.CreateMockUsers();
        var zones = _accessObject.CreateMockZones(packages);
        var bookings = _accessObject.CreateMockBookings(users, zones, packages);
        
        await _accessObject.InsertManyUsersAsync(users);
        await _accessObject.InsertManyPackagesAsync(packages);
        await _accessObject.InsertManyZonesAsync(zones);
        await _accessObject.InsertManyBookingsAsync(bookings);
        
        var expectedCount = bookings.Count;
        
        // Act
        Task Action() => _bookingService.RemoveBookingAsync(Guid.NewGuid());

        var actualBookings = await _bookingService.GetAllBookingAsync();
        var actualCount = actualBookings.Count;
        
        // Asserts
        Assert.Equal(expectedCount, actualCount);
        await Assert.ThrowsAsync<BookingNotFoundException>(Action);
    }
}