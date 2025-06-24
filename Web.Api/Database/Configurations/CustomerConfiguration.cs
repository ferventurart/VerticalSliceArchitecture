using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Api.Entities;

namespace Web.Api.Database.Configurations;

internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).HasMaxLength(500);

        builder.Property(c => c.FirstName).HasMaxLength(100);

        builder.Property(c => c.LastName).HasMaxLength(100);

        builder.Property(c => c.Email).HasMaxLength(300);

        builder.HasIndex(c => c.Email)
                .IsUnique()
                .HasDatabaseName("IX_Customers_Email");

        builder.Property(c => c.IdentificationNumber).HasMaxLength(10);

        builder.Property(c => c.PhoneNumber).HasMaxLength(9);

        builder.HasIndex(c => c.PhoneNumber)
            .IsUnique()
            .HasDatabaseName("IX_Customers_PhoneNumber");

        builder
            .Property(c => c.Status)
            .HasConversion<string>()
            .HasMaxLength(12);
    }
}
