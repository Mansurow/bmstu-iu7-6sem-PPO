using Moq;
using Portal.Database.Repositories.Interfaces;
using Portal.Services.ZoneService;

namespace UnitTests.Services;

public class ZoneServiceUnitTests
{
    private readonly IZoneService _zoneService;
    private readonly Mock<IZoneRepository> _mockZoneRepository = new();
    private readonly Mock<IInventoryRepository> _mockInventoryRepository = new();
    private readonly Mock<IPackageRepository> _mockPackageRepository = new();

    public ZoneServiceUnitTests()
    {
        _zoneService = new ZoneService(_mockZoneRepository.Object,
            _mockInventoryRepository.Object,
            _mockPackageRepository.Object);
    }
}