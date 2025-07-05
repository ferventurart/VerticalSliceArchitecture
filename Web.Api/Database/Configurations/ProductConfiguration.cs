using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Web.Api.Entities;

namespace Web.Api.Database.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(p => p.ProductCategoryId)
               .IsRequired()
               .HasMaxLength(500);

        builder.HasIndex(p => p.ProductCategoryId)
               .HasDatabaseName("IX_Products_Category_Id");

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.HasIndex(p => p.Name)
               .IsUnique()
               .HasDatabaseName("IX_Products_Name");

        builder.Property(p => p.Description)
               .HasMaxLength(500);

        builder.Property(p => p.Price)
               .IsRequired()
               .HasColumnType("numeric(18,2)");

        builder.Property(p => p.Cost)
               .IsRequired()
               .HasColumnType("numeric(18,2)");

        builder.Property(p => p.ImageUri)
                .HasColumnType("text");

        builder
            .Property(c => c.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasOne(p => p.ProductCategory)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.ProductCategoryId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(p => p.Profit);
    }
}
