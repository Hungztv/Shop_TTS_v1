using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping.Domain.Entities;

namespace Shopping.Infrastructure.Data.Configurations
{
    public class CompareProductConfiguration : IEntityTypeConfiguration<CompareProduct>
    {
        public void Configure(EntityTypeBuilder<CompareProduct> builder)
        {
            builder.ToTable("CompareProducts");
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Product)
                .WithMany(p => p.CompareProducts)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.User)
                .WithMany(u => u.CompareProducts)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

           
            builder.HasIndex(e => new { e.UserId, e.ProductId }).IsUnique();
        }
    }
}