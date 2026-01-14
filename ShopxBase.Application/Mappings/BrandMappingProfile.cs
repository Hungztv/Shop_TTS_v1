using AutoMapper;
using ShopxBase.Domain.Entities;
using ShopxBase.Application.DTOs.Brand;

namespace ShopxBase.Application.Mappings;

public class BrandMappingProfile : Profile
{
    public BrandMappingProfile()
    {
        // Brand Entity -> BrandDto (Read)
        CreateMap<Brand, BrandDto>()
            .ForMember(dest => dest.TotalProducts,
                       opt => opt.MapFrom(src => src.Products != null ? src.Products.Count : 0))
            .ForMember(dest => dest.IsActive,
                       opt => opt.MapFrom(src => !src.IsDeleted));

        // CreateBrandDto -> Brand Entity (Create)
        CreateMap<CreateBrandDto, Brand>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Products, opt => opt.Ignore());

        // UpdateBrandDto -> Brand Entity (Update)
        CreateMap<UpdateBrandDto, Brand>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Products, opt => opt.Ignore());
    }
}