using AutoMapper;
using MediatR;
using ShopxBase.Application.DTOs.BusinessRegistration;
using ShopxBase.Application.Interfaces;
using ShopxBase.Domain.Entities;
using ShopxBase.Domain.Enums;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.BusinessRegistrations.Commands.CreateBusinessRegistration;

public class CreateBusinessRegistrationCommandHandler : IRequestHandler<CreateBusinessRegistrationCommand, BusinessRegistrationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CreateBusinessRegistrationCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BusinessRegistrationDto> Handle(CreateBusinessRegistrationCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw UnauthorizedUserException.UserIdNotFound();

        var hasActive = await _unitOfWork.BusinessRegistrations.AnyAsync(br =>
            br.UserId == userId &&
            (br.Status == BusinessRegistrationStatus.Pending || br.Status == BusinessRegistrationStatus.Approved));

        if (hasActive)
            throw new DuplicateRegistrationException("Bạn đã có đơn đăng ký đang chờ hoặc đã được duyệt");

        var registration = _mapper.Map<BusinessRegistration>(request);
        registration.UserId = userId;
        registration.Status = BusinessRegistrationStatus.Pending;

        await _unitOfWork.BusinessRegistrations.AddAsync(registration);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<BusinessRegistrationDto>(registration);
    }
}
