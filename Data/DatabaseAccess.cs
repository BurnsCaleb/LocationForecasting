
namespace LocationForecasting.Data
{
    public class DatabaseAccess
    {
        protected readonly string _connectionString;

        public DatabaseAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
    }
}
