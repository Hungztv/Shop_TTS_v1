using MediatR;
using ShopxBase.Application.DTOs.BusinessRegistration;

namespace ShopxBase.Application.Features.BusinessRegistrations.Queries.GetMyBusinessRegistration;

public class GetMyBusinessRegistrationQuery : IRequest<BusinessRegistrationDto?>
{
}
