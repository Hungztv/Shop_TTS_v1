using MediatR;
using ShopxBase.Application.DTOs.BusinessRegistration;

namespace ShopxBase.Application.Features.BusinessRegistrations.Commands.ApproveBusinessRegistration;

public class ApproveBusinessRegistrationCommand : IRequest<BusinessRegistrationDto>
{
    public int Id { get; set; }
}
