using MediatR;
using ShopxBase.Application.Features.Auth.DTOs;

namespace ShopxBase.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    public LoginCommandHandler()
    {
        // TODO: Inject dependencies (UserManager, SignInManager, JwtTokenService, etc.)
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement login logic
        throw new NotImplementedException();
    }
}
