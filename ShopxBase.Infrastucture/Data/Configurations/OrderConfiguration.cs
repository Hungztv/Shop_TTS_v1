using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopxBase.Domain.Entities;

namespace ShopxBase.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(o => o.Id);

            // Cấu hình Mã đơn hàng
            builder.Property(o => o.OrderCode)
                .IsRequired()
                .HasMaxLength(50);

            
            builder.HasIndex(o => o.OrderCode).IsUnique();

            // Cấu hình tiền tệ
            builder.Property(o => o.Total).HasColumnType("decimal(18,2)");
            builder.Property(o => o.Subtotal).HasColumnType("decimal(18,2)");
            builder.Property(o => o.ShippingCost).HasColumnType("decimal(18,2)");
            builder.Property(o => o.DiscountAmount).HasColumnType("decimal(18,2)");

            // Mối quan hệ: Order thuộc về 1 User
            // Xóa User -> CHẶN LẠI (để bảo toàn lịch sử đơn hàng)
            builder.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}