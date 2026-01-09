using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopxBase.Domain.Entities;

namespace ShopxBase.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // 1. Cấu hình Bảng (Table) & Khóa chính (Key)

            builder.ToTable("Products");
            builder.HasKey(e => e.Id);

            // 2. Cấu hình các thuộc tính (Properties)
            builder.Property(e => e.Name)
                .IsRequired()           // Không được null
                .HasMaxLength(200);

            builder.Property(e => e.Slug)
                .HasMaxLength(250);

            builder.Property(e => e.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)"); 

            builder.Property(e => e.CapitalPrice)
                .HasColumnType("decimal(18,2)");

            // 3. Cấu hình Mối quan hệ (Relationships)

            // Relation: Product -> Brand (1-N)

            builder.HasOne(e => e.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(e => e.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relation: Product -> Category (1-N)

            builder.HasOne(e => e.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relation: Product -> Ratings (1-N)

            builder.HasMany(e => e.Ratings)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}