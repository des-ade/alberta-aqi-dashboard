using Microsoft.EntityFrameworkCore;
using AlbertaAqi.Api.Models;

namespace AlbertaAqi.Api.Data;

public class AqiDbContext : DbContext
{
    public AqiDbContext(DbContextOptions<AqiDbContext> options) : base(options) { }

    public DbSet<Station> Stations { get; set; }
    public DbSet<Sensor> Sensors { get; set; }
    public DbSet<Reading> Readings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Station indexes
        modelBuilder.Entity<Station>()
            .HasIndex(s => s.Timezone);

        modelBuilder.Entity<Station>()
            .HasIndex(s => s.IsActive);

        // Sensor indexes
        modelBuilder.Entity<Sensor>()
            .HasIndex(s => s.LocationId);

        modelBuilder.Entity<Sensor>()
            .HasIndex(s => s.Parameter);

        // Reading indexes — critical for time-series query performance
        modelBuilder.Entity<Reading>()
            .HasIndex(r => r.SensorId);

        modelBuilder.Entity<Reading>()
            .HasIndex(r => r.DatetimeUtc);

        modelBuilder.Entity<Reading>()
            .HasIndex(r => new { r.SensorId, r.DatetimeUtc });
    }
}