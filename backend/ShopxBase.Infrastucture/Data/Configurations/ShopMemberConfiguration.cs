using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopxBase.Domain.Entities;

namespace ShopxBase.Infrastructure.Data.Configurations;

public class ShopMemberConfiguration : IEntityTypeConfiguration<ShopMember>
{
    public void Configure(EntityTypeBuilder<ShopMember> builder)
    {
        builder.ToTable("ShopMembers");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.UserId)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(e => e.Shop)
            .WithMany(s => s.Members)
            .HasForeignKey(e => e.ShopId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new { e.ShopId, e.UserId })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");
    }
}
