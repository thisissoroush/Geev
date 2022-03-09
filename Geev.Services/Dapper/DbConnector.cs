using Microsoft.Extensions.Configuration;

namespace Geev.Services.Dapper
{
    public class DbConnector : IDbConnector
    {
        private IConfiguration Configuration { get; set; }

        public DbConnector(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public string ConnectionString => Configuration.GetConnectionString("DefaultConnection");

    }
}
