using Microsoft.EntityFrameworkCore;

namespace Anticafe.PostgreSQL;

public class InMemoryDbContextFactory: IDbContextFactory<PgSQLDbContext>
{
    private readonly string _dbName;
    public InMemoryDbContextFactory()
    {
        _dbName = "AnticafeDbTest" + Guid.NewGuid().ToString();
    }

    public PgSQLDbContext getDbContext()
    {

        var builder = new DbContextOptionsBuilder<PgSQLDbContext>();
        builder.UseInMemoryDatabase(_dbName);
        var _adminDbContext = new PgSQLDbContext(builder.Options);

        return _adminDbContext;
    }
}
