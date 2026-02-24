using MediatR;
using ShopxBase.Application.DTOs.BusinessRegistration;

namespace ShopxBase.Application.Features.BusinessRegistrations.Commands.CreateBusinessRegistration;

public class CreateBusinessRegistrationCommand : IRequest<BusinessRegistrationDto>
{
    public string CompanyName { get; set; }
    public string TaxCode { get; set; }
    public string OwnerName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
}
