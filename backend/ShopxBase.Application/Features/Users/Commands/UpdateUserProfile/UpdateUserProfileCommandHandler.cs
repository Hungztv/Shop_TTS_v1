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
        var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
        if (user == null || user.IsDeleted)
            throw new UserNotFoundException($"Người dùng với Id '{request.Id}' không tồn tại");

        // Update profile fields
        user.FullName = request.FullName;
        user.Occupation = request.Occupation;
        user.Address = request.Address;
        user.DateOfBirth = request.DateOfBirth;

        if (!string.IsNullOrEmpty(request.Avatar))
            user.Avatar = request.Avatar;

        user.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<AppUserDto>(user);
    }
}
