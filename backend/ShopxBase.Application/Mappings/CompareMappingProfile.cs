using AutoMapper;
using ShopxBase.Domain.Entities;
using ShopxBase.Application.DTOs.Compare;

namespace ShopxBase.Application.Mappings;

public class CompareMappingProfile : Profile
{
    public CompareMappingProfile()
    {
        // CompareProduct Entity -> CompareItemDto (Read)
        CreateMap<CompareProduct, CompareItemDto>()
            .ForMember(dest => dest.ProductName,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null))
            .ForMember(dest => dest.ProductSlug,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.Slug : null))
            .ForMember(dest => dest.ProductDescription,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.Description : null))
            .ForMember(dest => dest.ProductImage,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.Image : null))
            .ForMember(dest => dest.ProductPrice,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.Price : 0))
            .ForMember(dest => dest.ProductCapitalPrice,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.CapitalPrice : 0))
            .ForMember(dest => dest.ProductQuantity,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.Quantity : 0))
            .ForMember(dest => dest.ProductSoldOut,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.SoldOut : 0))
            .ForMember(dest => dest.IsInStock,
                       opt => opt.MapFrom(src => src.Product != null && src.Product.Quantity > 0))
            .ForMember(dest => dest.AverageScore,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.AverageScore : 0))
            .ForMember(dest => dest.RatingCount,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.RatingCount : 0))
            .ForMember(dest => dest.BrandId,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.BrandId : 0))
            .ForMember(dest => dest.BrandName,
                       opt => opt.MapFrom(src => src.Product != null && src.Product.Brand != null ? src.Product.Brand.Name : null))
            .ForMember(dest => dest.BrandLogo,
                       opt => opt.MapFrom(src => src.Product != null && src.Product.Brand != null ? src.Product.Brand.Logo : null))
            .ForMember(dest => dest.CategoryId,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.CategoryId : 0))
            .ForMember(dest => dest.CategoryName,
                       opt => opt.MapFrom(src => src.Product != null && src.Product.Category != null ? src.Product.Category.Name : null));
    }
}
