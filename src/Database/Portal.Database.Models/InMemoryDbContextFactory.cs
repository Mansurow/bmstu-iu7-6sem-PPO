using Microsoft.EntityFrameworkCore;

namespace Anticafe.DataAccess;

public class InMemoryDbContextFactory: IDbContextFactory
{
    private readonly string _dbName;
    public InMemoryDbContextFactory()
    {
        _dbName = "AnticafeDbTest" + Guid.NewGuid().ToString();
    }

    public AppDbContext getDbContext()
    {

        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseInMemoryDatabase(_dbName);
        var _adminDbContext = new AppDbContext(builder.Options);

        return _adminDbContext;
    }
}
