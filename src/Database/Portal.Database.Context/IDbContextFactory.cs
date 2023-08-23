namespace Portal.Database.Context;

public interface IDbContextFactory
{ 
    PortalDbContext GetDbContext();
}
