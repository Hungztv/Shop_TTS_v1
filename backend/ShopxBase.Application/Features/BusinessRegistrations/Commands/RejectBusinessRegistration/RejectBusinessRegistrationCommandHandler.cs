using AutoMapper;
using MediatR;
using ShopxBase.Application.DTOs.BusinessRegistration;
using ShopxBase.Application.Interfaces;
using ShopxBase.Domain.Enums;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.BusinessRegistrations.Commands.RejectBusinessRegistration;

public class RejectBusinessRegistrationCommandHandler : IRequestHandler<RejectBusinessRegistrationCommand, BusinessRegistrationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public RejectBusinessRegistrationCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BusinessRegistrationDto> Handle(RejectBusinessRegistrationCommand request, CancellationToken cancellationToken)
    {
        var registration = await _unitOfWork.BusinessRegistrations.GetByIdAsync(request.Id);
        if (registration == null)
            throw new BusinessRegistrationNotFoundException($"Đơn đăng ký với Id {request.Id} không tồn tại");

        if (registration.Status != BusinessRegistrationStatus.Pending)
            throw new DomainException("Đơn đăng ký đã được xử lý", "BUSINESS_REGISTRATION_STATUS_INVALID");

        var reviewerId = _currentUserService.UserId
            ?? throw UnauthorizedUserException.UserIdNotFound();

        registration.Status = BusinessRegistrationStatus.Rejected;
        registration.RejectReason = request.RejectReason;
        registration.ReviewedAt = DateTime.UtcNow;
        registration.ReviewedBy = reviewerId;

        await _unitOfWork.BusinessRegistrations.UpdateAsync(registration);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<BusinessRegistrationDto>(registration);
    }
}
