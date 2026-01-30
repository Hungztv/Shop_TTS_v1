using AutoMapper;
using MediatR;
using ShopxBase.Application.DTOs.ContactMessage;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.ContactMessages.Commands.CreateContactMessage;

public class CreateContactMessageCommandHandler : IRequestHandler<CreateContactMessageCommand, ContactMessageDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateContactMessageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ContactMessageDto> Handle(CreateContactMessageCommand request, CancellationToken cancellationToken)
    {
        var contactMessage = new Domain.Entities.ContactMessage
        {
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Subject = request.Subject,
            Message = request.Message,
            IsRead = false
        };

        await _unitOfWork.ContactMessages.AddAsync(contactMessage);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ContactMessageDto>(contactMessage);
    }
}
