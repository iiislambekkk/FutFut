using FutFut.Billing.Service.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FutFut.Billing.Service.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity>  Users { get; set; }
}