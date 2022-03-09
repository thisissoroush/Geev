using Microsoft.EntityFrameworkCore;
using Geev.Core.Entities;
using System.Reflection;

namespace Geev.Data;

public partial class GeevDbContext : DbContext
{

    public GeevDbContext()
    {
    }

    public GeevDbContext(DbContextOptions<GeevDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<WeatherStatus> WeatherStatus { get; set; }
    public virtual DbSet<WeatherType> WeatherType { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<WeatherStatus>()
            .HasOne<WeatherType>(s => s.WeatherType)
            .WithMany(g => g.WeatherStatuses)
            .HasForeignKey(s => s.TypeId);

        modelBuilder.Entity<WeatherType>()
            .HasMany<WeatherStatus>(g => g.WeatherStatuses)
            .WithOne(s => s.WeatherType)
            .HasForeignKey(s => s.TypeId);


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}