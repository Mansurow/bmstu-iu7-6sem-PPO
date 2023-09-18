using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Portal.Database.Context;

namespace IntegrationalTests.Controllers.AccessObject;

public class NpgsqlAccessObject: IDisposable
{
    public PortalDbContext _context {  get; private set; }

    protected readonly string DatabaseName = "PortalTestDb" + Guid.NewGuid();
    protected readonly string ConnectionString;

    public NpgsqlAccessObject()
    {
        ConnectionString = "Host=localhost;Port=5432;Database=" + DatabaseName + ";User Id=postgres;Password=postgres;";

        var contextOptions = new DbContextOptionsBuilder<PortalDbContext>()
            .UseNpgsql(ConnectionString);

        _context = new PortalDbContext(contextOptions.Options);

        _context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        _context.Database.EnsureDeleted();
    }
}
