using MediatR;

namespace ShopxBase.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<bool>
{
    public string UserId { get; set; }
}
