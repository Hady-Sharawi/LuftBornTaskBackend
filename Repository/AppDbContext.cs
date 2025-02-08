using Entities.Domains;
using Microsoft.EntityFrameworkCore;
using Repository.Configurations;

namespace Repository;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        new UserConfiguration(modelBuilder.Entity<User>());
        new ProductConfiguration(modelBuilder.Entity<Product>());
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
}
