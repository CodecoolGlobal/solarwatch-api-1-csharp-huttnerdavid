using Microsoft.EntityFrameworkCore;
using SolarWatch.Model;

namespace SolarWatch.Data;

public class SolarWatchApiContext : DbContext
{
    public DbSet<City> Cities { get; set; }
    public DbSet<SunsetTimes> SunsetTimes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=WeatherApi;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=true;");
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        //Configure the City entity - making the 'Name' unique
        builder.Entity<City>()
            .HasIndex(u => u.Name)
            .IsUnique();
    
        builder.Entity<City>()
            .HasData(
                new City { Id = 1, Name = "London", Lat = 51.509865, Lon = -0.118092 },
                new City { Id = 2, Name = "Budapest", Lat = 47.497913, Lon = 19.040236 },
                new City { Id = 3, Name = "Paris", Lat = 48.864716, Lon = 2.349014 }
            );
    }
}