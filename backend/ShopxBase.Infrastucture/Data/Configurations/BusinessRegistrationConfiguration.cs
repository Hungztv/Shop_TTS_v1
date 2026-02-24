using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopxBase.Domain.Entities;

namespace ShopxBase.Infrastructure.Data.Configurations;

public class BusinessRegistrationConfiguration : IEntityTypeConfiguration<BusinessRegistration>
{
    public void Configure(EntityTypeBuilder<BusinessRegistration> builder)
    {
        builder.ToTable("BusinessRegistrations");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.UserId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.CompanyName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.TaxCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.OwnerName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.Phone)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(e => e.Address)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(e => e.RejectReason)
            .HasMaxLength(500);

        builder.Property(e => e.ReviewedBy)
            .HasMaxLength(100);

        builder.HasIndex(e => e.UserId)
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false AND \"Status\" IN (0,1)");
    }
}
