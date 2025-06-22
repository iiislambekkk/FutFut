using FutFut.Songs.Service.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Namotion.Reflection;

namespace FutFut.Songs.Service.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) 
{
    public DbSet<SongEntity> Songs { get; set; }
    public DbSet<ArtistEntity> Artists { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<SongEntity>()
            .Property(t => t.StreamUrls)
            .HasColumnType("jsonb");

        modelBuilder.Entity<SongEntity>()
            .HasMany(s => s.ArtistEntities)
            .WithMany(a => a.Songs);
    }
}