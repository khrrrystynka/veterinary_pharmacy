using Microsoft.EntityFrameworkCore;
using VetPharmacyApi.Configurations;
using VetPharmacyApi.Models;

namespace VetPharmacyApi.Data;

public class VetPharmacyDbContext : DbContext
{
    public VetPharmacyDbContext(DbContextOptions<VetPharmacyDbContext> options) : base(options) { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<AppUser> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}