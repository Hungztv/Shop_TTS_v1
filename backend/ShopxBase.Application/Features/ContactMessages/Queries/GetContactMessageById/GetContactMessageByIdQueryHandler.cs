using AutoMapper;
using MediatR;
using ShopxBase.Application.DTOs.ContactMessage;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.ContactMessages.Queries.GetContactMessageById;

public class GetContactMessageByIdQueryHandler : IRequestHandler<GetContactMessageByIdQuery, ContactMessageDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetContactMessageByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ContactMessageDto?> Handle(GetContactMessageByIdQuery request, CancellationToken cancellationToken)
    {
        var contactMessage = await _unitOfWork.ContactMessages.GetByIdAsync(request.Id);
        if (contactMessage == null)
            return null;

        return _mapper.Map<ContactMessageDto>(contactMessage);
    }
}
