using Portal.Database.Context;

namespace IntegrationalTests.Services.AccessObject;

public class AccessObjectInMemory: PortalAccessObject, IDisposable
{

    private readonly PortalDbContext _context;

    public AccessObjectInMemory(): base()
    {
        var factory = new InMemoryDbContextFactory();
        _context = factory.GetDbContext();

        InitRepos(_context);
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }
}