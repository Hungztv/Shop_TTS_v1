using AutoMapper;
using MediatR;
using ShopxBase.Application.DTOs.BusinessRegistration;
using ShopxBase.Application.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.BusinessRegistrations.Queries.GetMyBusinessRegistration;

public class GetMyBusinessRegistrationQueryHandler : IRequestHandler<GetMyBusinessRegistrationQuery, BusinessRegistrationDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetMyBusinessRegistrationQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BusinessRegistrationDto?> Handle(GetMyBusinessRegistrationQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw UnauthorizedUserException.UserIdNotFound();

        var registrations = await _unitOfWork.BusinessRegistrations.FindAsync(br => br.UserId == userId);
        var latest = registrations
            .OrderByDescending(br => br.CreatedAt)
            .FirstOrDefault();

        return latest == null ? null : _mapper.Map<BusinessRegistrationDto>(latest);
    }
}
