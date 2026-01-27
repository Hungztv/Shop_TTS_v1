using AutoMapper;
using ShopxBase.Domain.Entities;
using ShopxBase.Application.DTOs.Coupon;
using ShopxBase.Application.Features.Coupons.Commands.CreateCoupon;
using ShopxBase.Application.Features.Coupons.Commands.UpdateCoupon;

namespace ShopxBase.Application.Mappings;

public class CouponMappingProfile : Profile
{
    public CouponMappingProfile()
    {
        // Coupon Entity -> CouponDto (Read)
        CreateMap<Coupon, CouponDto>()
            .ForMember(dest => dest.AvailableCount,
                       opt => opt.MapFrom(src => src.Quantity - src.UsedCount))
            .ForMember(dest => dest.IsValid,
                       opt => opt.MapFrom(src => src.IsValid()))
            .ForMember(dest => dest.IsExpired,
                       opt => opt.MapFrom(src => src.DateExpired < DateTime.Now));

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

        // Command Mappings (CQRS)
        CreateMap<CreateCouponCommand, Coupon>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UsedCount, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Orders, opt => opt.Ignore());

        CreateMap<UpdateCouponCommand, Coupon>()
            .ForMember(dest => dest.UsedCount, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Orders, opt => opt.Ignore());
    }
}