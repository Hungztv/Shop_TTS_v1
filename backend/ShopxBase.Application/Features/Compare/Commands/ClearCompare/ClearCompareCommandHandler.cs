using MediatR;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.Interfaces;

namespace ShopxBase.Application.Features.Compare.Commands.ClearCompare;

public class ClearCompareCommandHandler : IRequestHandler<ClearCompareCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public ClearCompareCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(ClearCompareCommand request, CancellationToken cancellationToken)
    {
        // 1. SECURITY: Get user ID from token
        if (string.IsNullOrEmpty(_currentUserService.UserId))
            throw UnauthorizedUserException.UserIdNotFound();

        var userId = _currentUserService.UserId;

        // 2. Get all compare items for user
        var compareItems = await _unitOfWork.CompareProducts.FindAsync(c => c.UserId == userId);

        // 3. Delete all items
        foreach (var item in compareItems)
        {
            await _unitOfWork.CompareProducts.DeletePermanentlyAsync(item.Id);
        }

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
