using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Api.Entities;

namespace Web.Api.Database.Configurations;

internal sealed class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasMany(c => c.Products)
               .WithOne(p => p.ProductCategory)
               .HasForeignKey(p => p.ProductCategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

