using AutoMapper;
using ShopxBase.Domain.Entities;
using ShopxBase.Application.DTOs.User;

namespace ShopxBase.Application.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        // AppUser Entity -> AppUserDto (Read)
        CreateMap<AppUser, AppUserDto>();

        // UpdateUserProfileDto -> AppUser Entity (Update)
        CreateMap<UpdateUserProfileDto, AppUser>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserName, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
            .ForMember(dest => dest.Orders, opt => opt.Ignore())
            .ForMember(dest => dest.Wishlists, opt => opt.Ignore())
            .ForMember(dest => dest.CompareProducts, opt => opt.Ignore());
    }
}