using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Application.DTOs.User;
using ShopxBase.Domain.Exceptions;

namespace ShopxBase.Application.Features.Users.Commands.UpdateUserProfile;

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, AppUserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateUserProfileCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AppUserDto> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        // Get user without tracking (to avoid conflict with middleware-tracked entity)
        var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
        if (user == null || user.IsDeleted)
            throw new UserNotFoundException($"Người dùng với Id '{request.Id}' không tồn tại");

        // Update only provided fields (null means keep current value)
        if (!string.IsNullOrEmpty(request.FullName))
            user.FullName = request.FullName;

        if (!string.IsNullOrEmpty(request.Occupation))
            user.Occupation = request.Occupation;

        if (!string.IsNullOrEmpty(request.Address))
            user.Address = request.Address;

        if (request.DateOfBirth.HasValue)
            user.DateOfBirth = DateTime.SpecifyKind(request.DateOfBirth.Value, DateTimeKind.Utc);

        if (!string.IsNullOrEmpty(request.Avatar))
            user.Avatar = request.Avatar;

        user.UpdatedAt = DateTime.UtcNow;

        // Attach and mark as modified (handles both tracked and untracked scenarios)
        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<AppUserDto>(user);
    }
}
