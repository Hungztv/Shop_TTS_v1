using AutoMapper;
using ShopxBase.Application.DTOs.BusinessRegistration;
using ShopxBase.Application.Features.BusinessRegistrations.Commands.CreateBusinessRegistration;
using ShopxBase.Domain.Entities;

namespace ShopxBase.Application.Mappings;

public class BusinessRegistrationMappingProfile : Profile
{
    public BusinessRegistrationMappingProfile()
    {
        CreateMap<BusinessRegistration, BusinessRegistrationDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<CreateBusinessRegistrationCommand, BusinessRegistration>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.RejectReason, opt => opt.Ignore())
            .ForMember(dest => dest.ReviewedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ReviewedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Shop, opt => opt.Ignore());
    }
}
