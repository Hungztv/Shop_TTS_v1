using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopxBase.Domain.Entities;

namespace ShopxBase.Infrastructure.Data.Configurations
{
    public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            builder.ToTable("Wishlists");
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Product)
                .WithMany(p => p.Wishlists)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.User)
                .WithMany(u => u.Wishlists)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.UserId, e.ProductId }).IsUnique();
        }
    }
}