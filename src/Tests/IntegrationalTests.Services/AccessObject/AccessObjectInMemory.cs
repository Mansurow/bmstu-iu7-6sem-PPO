using Microsoft.Extensions.Options;
using Portal.Common.Models;
using Portal.Common.Models.Enums;
using Portal.Database.Context;
using Portal.Database.Repositories.Interfaces;
using Portal.Database.Repositories.NpgsqlRepositories;

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