using AutoMapper;
using MediatR;
using ShopxBase.Application.DTOs.ContactMessage;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.ContactMessages.Queries.GetAllContactMessages;

public class GetAllContactMessagesQueryHandler : IRequestHandler<GetAllContactMessagesQuery, ContactMessageListDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllContactMessagesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ContactMessageListDto> Handle(GetAllContactMessagesQuery request, CancellationToken cancellationToken)
    {
        var allMessages = await _unitOfWork.ContactMessages.GetAllAsync();
        
        // Filter by IsRead if specified
        var query = allMessages.AsQueryable();
        if (request.IsRead.HasValue)
        {
            query = query.Where(m => m.IsRead == request.IsRead.Value);
        }

        // Order by CreatedAt DESC
        var orderedMessages = query.OrderByDescending(m => m.CreatedAt).ToList();

        // Get counts
        var totalCount = orderedMessages.Count;
        var unreadCount = allMessages.Count(m => !m.IsRead);

        // Pagination
        var pagedMessages = orderedMessages
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new ContactMessageListDto
        {
            Items = _mapper.Map<List<ContactMessageDto>>(pagedMessages),
            TotalCount = totalCount,
            UnreadCount = unreadCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
