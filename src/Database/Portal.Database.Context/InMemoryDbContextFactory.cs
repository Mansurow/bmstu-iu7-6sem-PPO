using Microsoft.EntityFrameworkCore;

namespace Portal.Database.Context;

public class InMemoryDbContextFactory: IDbContextFactory
{
    private readonly string _dbName;
    public InMemoryDbContextFactory()
    {
        _dbName = "PortalDbTest" + Guid.NewGuid();
    }

    public PortalDbContext GetDbContext()
    {

        var builder = new DbContextOptionsBuilder<PortalDbContext>();
        builder.UseInMemoryDatabase(_dbName);
        var context = new PortalDbContext(builder.Options);

        return context;
    }
}
