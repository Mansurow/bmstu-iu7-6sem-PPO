namespace Anticafe.PostgreSQL;

public interface IDbContextFactory<TContext>
{
    TContext getDbContext();
}
