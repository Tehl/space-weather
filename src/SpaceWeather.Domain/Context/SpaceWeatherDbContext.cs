using Microsoft.EntityFrameworkCore;
using SpaceWeather.Domain.Models;

namespace SpaceWeather.Domain.Context;

public class SpaceWeatherDbContext : DbContext
{
    public SpaceWeatherDbContext(
        DbContextOptions<SpaceWeatherDbContext> options
    )
        : base(options)
    {
    }

    public DbSet<MagneticIndexReading> MagneticIndexReadings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        _ = modelBuilder.Entity<MagneticIndexReading>()
            .ToTable("MagneticIndexReadings")
            .HasKey(x => new { x.StartTimeUtc, x.Type, x.Station });

        _ = modelBuilder.Entity<MagneticIndexReading>()
            .Property(x => x.Station)
            .HasConversion<byte>();

        _ = modelBuilder.Entity<MagneticIndexReading>()
            .Property(x => x.Type)
            .HasConversion<string>()
            .HasColumnType("char(1)");

        _ = modelBuilder.Entity<MagneticIndexReading>()
            .Property(x => x.Value)
            .HasPrecision(3, 2);
    }
}
