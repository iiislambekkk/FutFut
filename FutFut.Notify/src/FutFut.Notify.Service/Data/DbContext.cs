using FutFut.Notify.Service.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FutFut.Notify.Service.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<NotificationEntity> Notifications { get; set; }
    public DbSet<DeviceEntity> Devices { get; set; }
    public DbSet<UserEntity> Profiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DeviceEntity>()
            .HasOne(d => d.User)
            .WithMany(p => p.Devices)
            .HasForeignKey(d => d.UserId);
        
        modelBuilder.Entity<NotificationEntity>()
            .HasOne(d => d.User)
            .WithMany(p => p.Notifications)
            .HasForeignKey(d => d.UserId);
        
        base.OnModelCreating(modelBuilder);
    }
}