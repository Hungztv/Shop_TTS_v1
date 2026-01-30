using MediatR;
using ShopxBase.Application.DTOs.ContactMessage;

namespace ShopxBase.Application.Features.ContactMessages.Queries.GetAllContactMessages;

public class GetAllContactMessagesQuery : IRequest<ContactMessageListDto>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public bool? IsRead { get; set; }
}
