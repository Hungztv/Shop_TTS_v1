using MediatR;
using ShopxBase.Application.DTOs.Auth;

namespace ShopxBase.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<LoginResponseDto>
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
