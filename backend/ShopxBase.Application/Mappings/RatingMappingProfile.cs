using AutoMapper;
using ShopxBase.Domain.Entities;
using ShopxBase.Application.DTOs.Rating;

namespace ShopxBase.Application.Mappings;

public class RatingMappingProfile : Profile
{
    public RatingMappingProfile()
    {
        // Rating Entity -> RatingDto (Read)
        CreateMap<Rating, RatingDto>()
            .ForMember(dest => dest.ProductName,
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null))
            .ForMember(dest => dest.StarDisplay,
                       opt => opt.MapFrom(src => src.GetStarDisplay()))
            .ForMember(dest => dest.UserName,
                       opt => opt.MapFrom(src => src.User != null ? src.User.FullName : src.Name));

        // CreateRatingDto -> Rating Entity (Create)
        CreateMap<CreateRatingDto, Rating>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.IsVerifiedPurchase, opt => opt.Ignore())
            .ForMember(dest => dest.IsApproved, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());
    }
}