using FutFut.Profile.Service.Entities;
using Microsoft.EntityFrameworkCore;

namespace FutFut.Profile.Service.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) 
{
    public DbSet<ProfileEntity> Profiles { get; set; }
    public DbSet<PlayedHistoryEntity> PlayedHistory { get; set; }
    public DbSet<AboutPhotoEntity> AboutPhotos { get; set; }
    public DbSet<SystemWorks>  SystemWorks { get; set; }
    public DbSet<FriendShipEntity>  FriendShipEntities { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FriendShipEntity>()
            .HasIndex(f => new { f.RequestedUserId, f.RespondedUserId })
            .IsUnique();

        modelBuilder.Entity<AboutPhotoEntity>()
            .HasOne<ProfileEntity>()
            .WithMany()
            .HasForeignKey(ph => ph.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<FriendShipEntity>()
            .HasOne<ProfileEntity>()
            .WithMany()
            .HasForeignKey(ph => ph.RequestedUserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<FriendShipEntity>()
            .HasOne<ProfileEntity>()
            .WithMany()
            .HasForeignKey(ph => ph.RespondedUserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<PlayedHistoryEntity>()
            .HasOne<ProfileEntity>()
            .WithMany()
            .HasForeignKey(ph => ph.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}