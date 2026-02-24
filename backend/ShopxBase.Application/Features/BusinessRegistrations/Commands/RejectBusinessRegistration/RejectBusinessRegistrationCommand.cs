using MediatR;
using ShopxBase.Application.DTOs.BusinessRegistration;

namespace ShopxBase.Application.Features.BusinessRegistrations.Commands.RejectBusinessRegistration;

public class RejectBusinessRegistrationCommand : IRequest<BusinessRegistrationDto>
{
    public int Id { get; set; }
    public string RejectReason { get; set; }
}
