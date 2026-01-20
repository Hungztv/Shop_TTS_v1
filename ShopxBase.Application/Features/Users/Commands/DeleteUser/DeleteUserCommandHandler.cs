using MediatR;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;

namespace ShopxBase.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
        if (user == null || user.IsDeleted)
            throw new UserNotFoundException($"Người dùng với Id '{request.UserId}' không tồn tại");

        // Check if user has orders
        var hasOrders = await _unitOfWork.Orders.AnyAsync(o => o.UserId == request.UserId);
        if (hasOrders)
        {
            // Soft delete - just mark as deleted
            user.IsDeleted = true;
            user.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Users.UpdateAsync(user);
        }
        else
        {
            // Hard delete if no orders
            await _unitOfWork.Users.DeleteAsync(request.UserId);
        }

        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
