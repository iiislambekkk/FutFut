using FutFut.Profile.Service.Entities;
using Microsoft.EntityFrameworkCore;

namespace FutFut.Profile.Service.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) 
{
    public DbSet<ProfileEntity> Profiles { get; set; }
    public DbSet<PlayedHistory> PlayedHistory { get; set; }
    public DbSet<SystemWorks>  SystemWorks { get; set; }
}