using AutoMapper;
using ShopxBase.Domain.Entities;
using ShopxBase.Application.DTOs.Cart;

namespace ShopxBase.Application.Mappings;

/// <summary>
/// AutoMapper profile for Cart entity mappings
/// </summary>
public class CartMappingProfile : Profile
{
    public CartMappingProfile()
    {
        // Cart Entity -> CartDto (Read)
        CreateMap<Cart, CartDto>()
            .ForMember(dest => dest.ProductName,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null))
            .ForMember(dest => dest.ProductImage,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.Image : null))
            .ForMember(dest => dest.ProductSlug,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.Slug : null))
            .ForMember(dest => dest.Price,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.Price : 0))
            .ForMember(dest => dest.CapitalPrice,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.CapitalPrice : 0))
            .ForMember(dest => dest.MaxQuantity,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.Quantity : 0))
            .ForMember(dest => dest.Subtotal,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.Price * src.Quantity : 0));
    }
}
