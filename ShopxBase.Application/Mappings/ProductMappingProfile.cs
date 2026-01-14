using AutoMapper;
using ShopxBase.Domain.Entities;
using ShopxBase.Application.DTOs.Product;

namespace ShopxBase.Application.Mappings;

public class ProductMappingProfile : Profile
{
	public ProductMappingProfile()
	{
		// Product Entity -> ProductDto (Read)
		CreateMap<Product, ProductDto>()
			.ForMember(dest => dest.BrandName,
					   opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : null))
			.ForMember(dest => dest.CategoryName,
					   opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
			.ForMember(dest => dest.AverageRating,
					   opt => opt.MapFrom(src => src.Ratings != null && src.Ratings.Any()
												 ? src.Ratings.Average(r => r.Star)
												 : 0));

		// CreateProductDto -> Product Entity (Create)
		CreateMap<CreateProductDto, Product>()
			.ForMember(dest => dest.Id, opt => opt.Ignore())
			.ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
			.ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
			.ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
			.ForMember(dest => dest.CapitalPrice, opt => opt.MapFrom(src => src.CapitalPrice))
		CreateMap<UpdateProductDto, Product>()
			.ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
			.ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
			.ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
			.ForMember(dest => dest.CapitalPrice, opt => opt.MapFrom(src => src.CapitalPrice))
			.ForMember(dest => dest.Brand, opt => opt.Ignore())
			.ForMember(dest => dest.Category, opt => opt.Ignore())
			.ForMember(dest => dest.Ratings, opt => opt.Ignore())
			.ForMember(dest => dest.OrderDetails, opt => opt.Ignore());
	}
}