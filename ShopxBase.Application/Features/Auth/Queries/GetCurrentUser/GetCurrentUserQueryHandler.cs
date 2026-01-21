using MediatR;
using Microsoft.AspNetCore.Identity;
using ShopxBase.Application.DTOs.Auth;
using ShopxBase.Domain.Entities;
using ShopxBase.Domain.Exceptions;

namespace ShopxBase.Application.Features.Auth.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserInfoDto>
{
    private readonly UserManager<AppUser> _userManager;

    public GetCurrentUserQueryHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserInfoDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user == null || user.IsDeleted)
            throw new UserNotFoundException("Người dùng không tồn tại");

        var roles = await _userManager.GetRolesAsync(user);

        return new UserInfoDto
        {
            Id = user.Id,
            UserName = user.UserName!,
            Email = user.Email!,
            FullName = user.FullName,
            Avatar = user.Avatar,
            PhoneNumber = user.PhoneNumber,
            Roles = roles
        };
    }
}
