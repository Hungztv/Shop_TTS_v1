using MediatR;
using ShopxBase.Application.DTOs.ContactMessage;

namespace ShopxBase.Application.Features.ContactMessages.Commands.ReplyToContactMessage;

public class ReplyToContactMessageCommand : IRequest<ContactMessageDto?>
{
    public int Id { get; set; }
    public string ReplyMessage { get; set; } = string.Empty;
}
