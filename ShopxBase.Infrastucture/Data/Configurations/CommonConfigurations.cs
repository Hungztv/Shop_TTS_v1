using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping.Domain.Entities;

namespace Shopping.Infrastructure.Data.Configurations
{

	//  SLIDER

	public class SliderConfiguration : IEntityTypeConfiguration<Slider>
	{
		public void Configure(EntityTypeBuilder<Slider> builder)
		{
			builder.ToTable("Sliders");
			builder.HasKey(e => e.Id);

			builder.Property(e => e.Name)
				.IsRequired()
				.HasMaxLength(200);

			
		}
	}

	
	// CONTACT
	
	public class ContactConfiguration : IEntityTypeConfiguration<Contact>
	{
		public void Configure(EntityTypeBuilder<Contact> builder)
		{
			builder.ToTable("Contacts");
			builder.HasKey(e => e.Id);

			builder.Property(e => e.Email)
				.IsRequired() 
				.HasMaxLength(100);

			builder.Property(e => e.Phone)
				.HasMaxLength(20);
		}
	}

//Shipp
	public class ShippingConfiguration : IEntityTypeConfiguration<Shipping>
	{
		public void Configure(EntityTypeBuilder<Shipping> builder)
		{
			builder.ToTable("Shippings");
			builder.HasKey(e => e.Id);

			builder.Property(e => e.Price)
				.HasColumnType("decimal(18,2)");

			builder.Property(e => e.Ward).HasMaxLength(50);
			builder.Property(e => e.District).HasMaxLength(50);
			builder.Property(e => e.City).HasMaxLength(50);
		}
	}
}