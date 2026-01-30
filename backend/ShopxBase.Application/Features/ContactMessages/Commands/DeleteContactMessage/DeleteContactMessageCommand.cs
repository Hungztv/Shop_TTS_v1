using MediatR;

namespace ShopxBase.Application.Features.ContactMessages.Commands.DeleteContactMessage;

public class DeleteContactMessageCommand : IRequest<bool>
{
    public int Id { get; set; }

    public DeleteContactMessageCommand(int id)
    {
        Id = id;
    }
}
