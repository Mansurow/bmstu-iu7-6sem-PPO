namespace Anticafe.DataAccess;

public interface IDbContextFactory
{ 
    AppDbContext getDbContext();
}
