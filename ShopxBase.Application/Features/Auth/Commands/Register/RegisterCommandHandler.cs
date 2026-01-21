using MediatR;
using ShopxBase.Application.Features.Auth.DTOs;

namespace ShopxBase.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    public RegisterCommandHandler()
    {
        // TODO: Inject dependencies (UserManager, JwtTokenService, etc.)
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement registration logic
        throw new NotImplementedException();
    }
}
