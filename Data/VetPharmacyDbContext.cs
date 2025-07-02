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
            base.OnModelCreating(modelBuilder);
    
            modelBuilder.Entity<Category>().HasData(
                 new Category { Id = 1, Name = "Antibiotics" },
                            new Category { Id = 2, Name = "Vaccines" },
                            new Category { Id = 3, Name = "Parasite Control" },
                            new Category { Id = 4, Name = "Pain Relief" },
                            new Category { Id = 5, Name = "Vitamins and Supplements" },
                            new Category { Id = 6, Name = "Skin and Coat Care" },
                            new Category { Id = 7, Name = "Dental Care" },
                            new Category { Id = 8, Name = "Ear Care" },
                            new Category { Id = 9, Name = "Eye Care" },
                            new Category { Id = 10, Name = "Digestive Health" }
            );
        }
}