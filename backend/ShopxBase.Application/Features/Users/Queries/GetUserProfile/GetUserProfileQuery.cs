using MediatR;
using ShopxBase.Application.DTOs.User;

namespace ShopxBase.Application.Features.Users.Queries.GetUserProfile;

public class GetUserProfileQuery : IRequest<AppUserDto>
{
    public string UserId { get; set; }
}
