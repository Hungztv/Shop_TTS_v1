using AutoMapper;
using ShopxBase.Application.DTOs.Shop;
using ShopxBase.Application.Features.Shops.Commands.UpdateShop;
using ShopxBase.Domain.Entities;

namespace ShopxBase.Application.Mappings;

public class ShopMappingProfile : Profile
{
    public ShopMappingProfile()
    {
        CreateMap<Shop, ShopDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<UpdateShopCommand, Shop>()
            .ForMember(dest => dest.OwnerUserId, opt => opt.Ignore())
            .ForMember(dest => dest.BusinessRegistrationId, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.BusinessRegistration, opt => opt.Ignore())
            .ForMember(dest => dest.Products, opt => opt.Ignore())
            .ForMember(dest => dest.Members, opt => opt.Ignore());
    }
}
