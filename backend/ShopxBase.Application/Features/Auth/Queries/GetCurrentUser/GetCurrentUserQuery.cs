using MediatR;
using ShopxBase.Application.DTOs.Auth;

namespace ShopxBase.Application.Features.Auth.Queries.GetCurrentUser;

public class GetCurrentUserQuery : IRequest<UserInfoDto>
{
    public string UserId { get; set; }
}
