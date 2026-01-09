using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping.Domain.Entities;

namespace Shopping.Infrastructure.Data.Configurations
{
    public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.ToTable("Coupons");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(e => e.Code).IsUnique();

            builder.Property(e => e.DiscountValue)
                .HasColumnType("decimal(18,2)");

            builder.Property(e => e.MinimumOrderValue)
                .HasColumnType("decimal(18,2)");
        }
    }
}