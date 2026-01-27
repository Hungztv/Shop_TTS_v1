using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopxBase.Domain.Entities;

namespace ShopxBase.Infrastructure.Data.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        // Table name
        builder.ToTable("Carts");

        // Primary key
        builder.HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(e => e.ProductId)
            .IsRequired();

        builder.Property(e => e.Quantity)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(e => e.UpdatedAt)
            .IsRequired(false);

        builder.Property(e => e.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Composite unique index on (UserId, ProductId)
        builder.HasIndex(e => new { e.UserId, e.ProductId })
            .IsUnique()
            .HasDatabaseName("IX_Carts_UserId_ProductId");

        // Foreign key to AppUser with cascade delete
        builder.HasOne(e => e.User)
            .WithMany(u => u.Carts)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Foreign key to Product with restrict delete
        builder.HasOne(e => e.Product)
            .WithMany(p => p.Carts)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
