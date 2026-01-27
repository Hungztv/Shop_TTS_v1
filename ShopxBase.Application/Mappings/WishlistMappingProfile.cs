using AutoMapper;
using ShopxBase.Domain.Entities;
using ShopxBase.Application.DTOs.Wishlist;

namespace ShopxBase.Application.Mappings;

public class WishlistMappingProfile : Profile
{
    public WishlistMappingProfile()
    {
        // Wishlist Entity -> WishlistDto (Read)
        CreateMap<Wishlist, WishlistDto>()
            .ForMember(dest => dest.ProductName,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null))
            .ForMember(dest => dest.ProductSlug,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.Slug : null))
            .ForMember(dest => dest.ProductImage,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.Image : null))
            .ForMember(dest => dest.ProductPrice,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.Price : 0))
            .ForMember(dest => dest.ProductCapitalPrice,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.CapitalPrice : 0))
            .ForMember(dest => dest.ProductQuantity,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.Quantity : 0))
            .ForMember(dest => dest.IsInStock,
                       opt => opt.MapFrom(src => src.Product != null && src.Product.Quantity > 0))
            .ForMember(dest => dest.BrandName,
                       opt => opt.MapFrom(src => src.Product != null && src.Product.Brand != null ? src.Product.Brand.Name : null))
            .ForMember(dest => dest.CategoryName,
                       opt => opt.MapFrom(src => src.Product != null && src.Product.Category != null ? src.Product.Category.Name : null));
    }
}
