using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopxBase.Domain.Entities;

namespace ShopxBase.Infrastructure.Data.Configurations;

public class ShopConfiguration : IEntityTypeConfiguration<Shop>
{
    public void Configure(EntityTypeBuilder<Shop> builder)
    {
        builder.ToTable("Shops");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.OwnerUserId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(e => e.Slug)
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.LogoUrl)
            .HasMaxLength(500);

        builder.Property(e => e.CoverUrl)
            .HasMaxLength(500);

        builder.HasIndex(e => e.Slug)
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        builder.HasOne(e => e.BusinessRegistration)
            .WithOne(br => br.Shop)
            .HasForeignKey<Shop>(e => e.BusinessRegistrationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
