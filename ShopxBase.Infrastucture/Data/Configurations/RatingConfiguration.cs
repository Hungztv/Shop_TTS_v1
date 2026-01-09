using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopxBase.Domain.Entities;

namespace ShopxBase.Infrastructure.Data.Configurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.ToTable("Ratings");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Star).IsRequired();

            
            builder.HasOne(e => e.Product)
                .WithMany(p => p.Ratings)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}