using AutoMapper;
using MediatR;
using ShopxBase.Application.DTOs.BusinessRegistration;
using ShopxBase.Domain.Enums;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.BusinessRegistrations.Queries.AdminListBusinessRegistrations;

public class AdminListBusinessRegistrationsQueryHandler : IRequestHandler<AdminListBusinessRegistrationsQuery, List<BusinessRegistrationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AdminListBusinessRegistrationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<BusinessRegistrationDto>> Handle(AdminListBusinessRegistrationsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.BusinessRegistration> registrations;

        if (request.Status.HasValue)
        {
            var status = request.Status.Value;
            registrations = await _unitOfWork.BusinessRegistrations.FindAsync(br => br.Status == status);
        }
        else
        {
            registrations = await _unitOfWork.BusinessRegistrations.GetAllAsync();
        }

        return _mapper.Map<List<BusinessRegistrationDto>>(registrations);
    }
}
