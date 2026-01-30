using MediatR;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.ContactMessages.Commands.MarkAsRead;

public class MarkAsReadCommandHandler : IRequestHandler<MarkAsReadCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public MarkAsReadCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
    {
        var contactMessage = await _unitOfWork.ContactMessages.GetByIdAsync(request.Id);
        if (contactMessage == null)
            return false;

        contactMessage.IsRead = true;
        await _unitOfWork.ContactMessages.UpdateAsync(contactMessage);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
