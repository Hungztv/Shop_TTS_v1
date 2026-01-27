using MediatR;
using ShopxBase.Domain.Enums;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.Interfaces;

namespace ShopxBase.Application.Features.Orders.Commands.CancelOrder;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public CancelOrderCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);

        if (order == null)
            throw OrderNotFoundException.ById(request.OrderId);

        // AUTHORIZATION: Check ownership for non-admin/seller users
        if (!_currentUserService.IsAdminOrSeller)
        {
            if (string.IsNullOrEmpty(_currentUserService.UserId))
            {
                throw UnauthorizedUserException.UserIdNotFound();
            }

            if (order.UserId != _currentUserService.UserId)
            {
                throw ForbiddenAccessException.NotOwner();
            }
        }

        // Chỉ cho phép hủy đơn hàng ở trạng thái Pending hoặc Confirmed
        if (order.Status != (int)OrderStatus.Pending && order.Status != (int)OrderStatus.Confirmed)
        {
            throw new InvalidOrderException($"Không thể hủy đơn hàng ở trạng thái hiện tại");
        }

        order.Status = (int)OrderStatus.Cancelled;
        order.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
