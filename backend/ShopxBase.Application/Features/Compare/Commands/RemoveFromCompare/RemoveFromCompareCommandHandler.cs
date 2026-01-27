using MediatR;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.Interfaces;

namespace ShopxBase.Application.Features.Compare.Commands.RemoveFromCompare;

public class RemoveFromCompareCommandHandler : IRequestHandler<RemoveFromCompareCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public RemoveFromCompareCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(RemoveFromCompareCommand request, CancellationToken cancellationToken)
    {
        // 1. SECURITY: Get user ID from token
        if (string.IsNullOrEmpty(_currentUserService.UserId))
            throw UnauthorizedUserException.UserIdNotFound();

        var userId = _currentUserService.UserId;

        // 2. Get compare item
        var compareItem = await _unitOfWork.CompareProducts.GetByIdAsync(request.CompareId);
        if (compareItem == null)
            throw CompareNotFoundException.NotFound(request.CompareId);

        // 3. SECURITY: Validate ownership
        if (compareItem.UserId != userId)
            throw CompareUnauthorizedException.UnauthorizedAccess();

        // 4. Delete item
        await _unitOfWork.CompareProducts.DeletePermanentlyAsync(request.CompareId);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
