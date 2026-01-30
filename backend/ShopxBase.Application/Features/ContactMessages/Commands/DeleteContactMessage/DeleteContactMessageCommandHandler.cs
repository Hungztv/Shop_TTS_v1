using MediatR;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.ContactMessages.Commands.DeleteContactMessage;

public class DeleteContactMessageCommandHandler : IRequestHandler<DeleteContactMessageCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteContactMessageCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteContactMessageCommand request, CancellationToken cancellationToken)
    {
        var contactMessage = await _unitOfWork.ContactMessages.GetByIdAsync(request.Id);
        if (contactMessage == null)
            return false;

        // Hard delete for contact messages
        await _unitOfWork.ContactMessages.DeletePermanentlyAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
