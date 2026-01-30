using MediatR;
using ShopxBase.Application.DTOs.ContactMessage;

namespace ShopxBase.Application.Features.ContactMessages.Queries.GetContactMessageById;

public class GetContactMessageByIdQuery : IRequest<ContactMessageDto?>
{
    public int Id { get; set; }

    public GetContactMessageByIdQuery(int id)
    {
        Id = id;
    }
}
