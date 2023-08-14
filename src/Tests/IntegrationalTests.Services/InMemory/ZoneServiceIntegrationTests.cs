using IntegrationalTests.Services.AccessObject;
using Microsoft.Extensions.Logging.Abstractions;
using Portal.Services.ZoneService;

namespace IntegrationalTests.Services.InMemory;

public class ZoneServiceIntegrationTests
{
    private readonly IZoneService _zoneService;
    private readonly AccessObjectInMemory _accessObject;

    public ZoneServiceIntegrationTests()
    {
        _accessObject = new AccessObjectInMemory();
        _zoneService = new ZoneService(_accessObject.ZoneRepository, 
            _accessObject.InventoryRepository, 
            _accessObject.PackageRepository,
            NullLogger<ZoneService>.Instance);
    }
}