using MediatR;
using ShopxBase.Application.DTOs.Auth;

namespace ShopxBase.Application.Features.Auth.Commands.Register;

public class RegisterCommand : IRequest<LoginResponseDto>
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FullName { get; set; }
}
