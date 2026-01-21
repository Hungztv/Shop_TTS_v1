using MediatR;
using ShopxBase.Application.Features.Auth.DTOs;

namespace ShopxBase.Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<AuthResponseDto>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
