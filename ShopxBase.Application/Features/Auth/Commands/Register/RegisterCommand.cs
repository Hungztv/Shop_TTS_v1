using MediatR;
using ShopxBase.Application.Features.Auth.DTOs;

namespace ShopxBase.Application.Features.Auth.Commands.Register;

public class RegisterCommand : IRequest<AuthResponseDto>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
}
