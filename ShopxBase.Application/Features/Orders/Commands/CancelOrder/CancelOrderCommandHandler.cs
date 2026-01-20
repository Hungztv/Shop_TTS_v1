using MediatR;
using ShopxBase.Domain.Enums;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.Orders.Commands.CancelOrder;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public CancelOrderCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);

        if (order == null)
            throw new KeyNotFoundException($"Order với ID {request.OrderId} không tồn tại");

        // Chỉ cho phép hủy đơn hàng ở trạng thái Pending hoặc Confirmed
        if (order.Status != (int)OrderStatus.Pending && order.Status != (int)OrderStatus.Confirmed)
        {
            throw new InvalidOperationException($"Không thể hủy đơn hàng ở trạng thái {order.Status}");
        }

        order.Status = (int)OrderStatus.Cancelled;
        order.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
