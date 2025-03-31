using LocationForecasting.Models;
using Microsoft.EntityFrameworkCore;

namespace LocationForecasting.Data
{
    public class LocationForecastingContext : DbContext
    {
        public LocationForecastingContext(DbContextOptions<LocationForecastingContext> options) : base(options) { }

        public DbSet<RequestLog> RequestLogs { get; set; } 
        public DbSet<User> Users { get; set; }
        public DbSet<ExceptionLog> ExceptionLogs { get; set; }
    }
}
