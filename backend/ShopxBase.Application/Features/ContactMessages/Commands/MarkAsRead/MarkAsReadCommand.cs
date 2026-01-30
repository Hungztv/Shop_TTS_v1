using MediatR;

namespace ShopxBase.Application.Features.ContactMessages.Commands.MarkAsRead;

public class MarkAsReadCommand : IRequest<bool>
{
    public int Id { get; set; }

    public MarkAsReadCommand(int id)
    {
        Id = id;
    }
}
