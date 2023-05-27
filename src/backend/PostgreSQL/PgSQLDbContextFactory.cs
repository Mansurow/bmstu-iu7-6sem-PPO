using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace Anticafe.PostgreSQL
{
    public class PgSQLDbContextFactory: IDbContextFactory<PgSQLDbContext>
    {
        private readonly IConfiguration _configuration;

        public PgSQLDbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public PgSQLDbContext getDbContext()
        {
            string curPerms = "default"; //_configuration["DbConnection"]!;

            var builder = new DbContextOptionsBuilder<PgSQLDbContext>();
            builder.UseNpgsql(_configuration.GetSection("PostgreSQL").GetConnectionString(curPerms));
            var _adminDbContext = new PgSQLDbContext(builder.Options);

            return _adminDbContext;
        }
    }
}
