using FutFut.Notify.Service.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FutFut.Notify.Service.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<NotificationEntity> Notifications { get; set; }
    public DbSet<DeviceEntity> Devices { get; set; }
}