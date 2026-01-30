using MediatR;
using ShopxBase.Application.DTOs.ContactMessage;

namespace ShopxBase.Application.Features.ContactMessages.Commands.CreateContactMessage;

public class CreateContactMessageCommand : IRequest<ContactMessageDto>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
