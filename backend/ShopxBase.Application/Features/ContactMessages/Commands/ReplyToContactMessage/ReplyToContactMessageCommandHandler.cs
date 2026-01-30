using AutoMapper;
using MediatR;
using ShopxBase.Application.DTOs.ContactMessage;
using ShopxBase.Application.Interfaces;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.ContactMessages.Commands.ReplyToContactMessage;

public class ReplyToContactMessageCommandHandler : IRequestHandler<ReplyToContactMessageCommand, ContactMessageDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;

    public ReplyToContactMessageCommandHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _emailService = emailService;
    }

    public async Task<ContactMessageDto?> Handle(ReplyToContactMessageCommand request, CancellationToken cancellationToken)
    {
        var contactMessage = await _unitOfWork.ContactMessages.GetByIdAsync(request.Id);
        if (contactMessage == null)
            return null;

        // Send reply email
        await _emailService.SendContactReplyAsync(
            contactMessage.Email,
            contactMessage.Name,
            request.ReplyMessage
        );

        // Update contact message
        contactMessage.IsRead = true;
        contactMessage.RepliedAt = DateTime.UtcNow;
        contactMessage.ReplyMessage = request.ReplyMessage;

        await _unitOfWork.ContactMessages.UpdateAsync(contactMessage);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ContactMessageDto>(contactMessage);
    }
}
