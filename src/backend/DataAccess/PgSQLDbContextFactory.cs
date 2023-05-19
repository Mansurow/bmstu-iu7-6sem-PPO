using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Anticafe.DataAccess
{
    public class PgSQLDbContextFactory: IDbContextFactory
    {
        private readonly IConfiguration _configuration;

        public PgSQLDbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AppDbContext getDbContext()
        {
            string curPerms = "default"; //_configuration["DbConnection"]!;

            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseNpgsql(_configuration.GetSection("PostgreSQL").GetConnectionString(curPerms));
            var _adminDbContext = new AppDbContext(builder.Options);

            return _adminDbContext;
        }
    }
}
