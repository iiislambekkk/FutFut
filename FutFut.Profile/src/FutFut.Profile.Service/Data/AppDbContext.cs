using FutFut.Profile.Service.Entities;
using Microsoft.EntityFrameworkCore;

namespace FutFut.Profile.Service.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) 
{
    public DbSet<ProfileEntity> Profiles { get; set; }
    public DbSet<PlayedHistoryEntity> PlayedHistory { get; set; }
    public DbSet<AboutPhotoEntity> AboutPhotos { get; set; }
    public DbSet<SystemWorks>  SystemWorks { get; set; }
    public DbSet<FriendShipRequestEntity>  FriendShipRequests { get; set; }
    public DbSet<FriendShipEntity>  FriendShips { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FriendShipRequestEntity>()
            .HasOne(f => f.FromProfile)
            .WithMany(p => p.SentFriendShipRequests)
            .HasForeignKey(f => f.FromUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<FriendShipRequestEntity>()
            .HasOne(f => f.ToProfile)
            .WithMany(p => p.ReceivedFriendShipRequests)
            .HasForeignKey(f => f.ToUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AboutPhotoEntity>()
            .HasOne<ProfileEntity>()
            .WithMany()
            .HasForeignKey(ph => ph.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<PlayedHistoryEntity>()
            .HasOne<ProfileEntity>()
            .WithMany()
            .HasForeignKey(ph => ph.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<FriendShipEntity>(builder =>
        {
            builder.HasKey(f => f.Id);

            builder
                .HasOne(f => f.FriendA)
                .WithMany()
                .HasForeignKey(f => f.FriendAId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(f => f.FriendB)
                .WithMany() 
                .HasForeignKey(f => f.FriendBId)
                .OnDelete(DeleteBehavior.Restrict); 
        });

        base.OnModelCreating(modelBuilder);
    }
}