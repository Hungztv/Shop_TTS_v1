using AutoMapper;
using ShopxBase.Application.DTOs.Slider;
using ShopxBase.Domain.Entities;

namespace ShopxBase.Application.Mappings;

public class SliderMappingProfile : Profile
{
    public SliderMappingProfile()
    {
        CreateMap<Slider, SliderDto>()
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Status == 1));
        
        CreateMap<CreateSliderDto, Slider>();
        CreateMap<UpdateSliderDto, Slider>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
