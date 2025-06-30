using Microsoft.EntityFrameworkCore;
using VetPharmacyApi.Models;

namespace VetPharmacyApi.Data;

public class VetPharmacyDbContext : DbContext
{
    public VetPharmacyDbContext(DbContextOptions<VetPharmacyDbContext> options) : base(options) { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<AppUser> Users { get; set; }
}