using AutoMapper;
using ShopxBase.Domain.Entities;
using ShopxBase.Application.DTOs.Coupon;

namespace ShopxBase.Application.Mappings;

public class CouponMappingProfile : Profile
{
    public CouponMappingProfile()
    {
        // Coupon Entity -> CouponDto (Read)
        CreateMap<Coupon, CouponDto>()
            .ForMember(dest => dest.AvailableCount,
                       opt => opt.MapFrom(src => src.MaxUsage - src.UsedCount))
            .ForMember(dest => dest.IsValid,
                       opt => opt.MapFrom(src => src.IsValid()))
            .ForMember(dest => dest.IsExpired,
                       opt => opt.MapFrom(src => src.EndDate.HasValue && src.EndDate.Value < DateTime.Now));

        // CreateCouponDto -> Coupon Entity (Create)
        CreateMap<CreateCouponDto, Coupon>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UsedCount, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Orders, opt => opt.Ignore());

        // UpdateCouponDto -> Coupon Entity (Update)
        CreateMap<UpdateCouponDto, Coupon>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Orders, opt => opt.Ignore());
    }
}