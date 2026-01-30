using AutoMapper;
using ShopxBase.Application.DTOs.ContactMessage;

namespace ShopxBase.Application.Mappings;

public class ContactMessageMappingProfile : Profile
{
    public ContactMessageMappingProfile()
    {
        CreateMap<Domain.Entities.ContactMessage, ContactMessageDto>();
        CreateMap<CreateContactMessageDto, Domain.Entities.ContactMessage>();
    }
}
