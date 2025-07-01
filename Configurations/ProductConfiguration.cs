using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetPharmacyApi.Models;

namespace VetPharmacyApi.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Quantity)
            .IsRequired();

        builder.Property(p => p.ArrivalDate)
            .HasColumnType("timestamp without time zone");

        builder.Property(p => p.ExpiryDate)
            .HasColumnType("timestamp without time zone");

        builder.Property(p => p.IsWriteOffAllowed)
            .IsRequired();

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}