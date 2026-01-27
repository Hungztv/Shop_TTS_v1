using MediatR;
using ShopxBase.Application.DTOs.Auth;

namespace ShopxBase.Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<LoginResponseDto>
{
    public string EmailOrUserName { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; } = false;
}
