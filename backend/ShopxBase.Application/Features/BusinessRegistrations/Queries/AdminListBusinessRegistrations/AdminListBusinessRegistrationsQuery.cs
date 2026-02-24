using MediatR;
using ShopxBase.Application.DTOs.BusinessRegistration;
using ShopxBase.Domain.Enums;

namespace ShopxBase.Application.Features.BusinessRegistrations.Queries.AdminListBusinessRegistrations;

public class AdminListBusinessRegistrationsQuery : IRequest<List<BusinessRegistrationDto>>
{
    public BusinessRegistrationStatus? Status { get; set; }
}
