using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Portal.Database.Context
{
    public class NpgsqlDbContextFactory: IDbContextFactory
    {
        private readonly IConfiguration _configuration;

        public NpgsqlDbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public PortalDbContext GetDbContext()
        {
            string curPerms = _configuration["DbConnection"]!;

            var builder = new DbContextOptionsBuilder<PortalDbContext>();
            builder.UseNpgsql(_configuration.GetConnectionString(curPerms));
            var context = new PortalDbContext(builder.Options);

            return context;
        }
    }
}
