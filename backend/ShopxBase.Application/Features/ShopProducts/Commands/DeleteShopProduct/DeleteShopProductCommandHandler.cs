using MediatR;
using ShopxBase.Application.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.ShopProducts.Commands.DeleteShopProduct;

public class DeleteShopProductCommandHandler : IRequestHandler<DeleteShopProductCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public DeleteShopProductCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(DeleteShopProductCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw UnauthorizedUserException.UserIdNotFound();

        var shop = await _unitOfWork.Shops.FirstOrDefaultAsync(s => s.OwnerUserId == userId);
        if (shop == null)
            throw new ShopNotFoundException("Bạn chưa có shop được duyệt");

        var product = await _unitOfWork.Products.GetByIdAsync(request.Id);
        if (product == null)
            throw new InvalidProductException($"Sản phẩm với Id {request.Id} không tồn tại");

        if (product.ShopId != shop.Id)
            throw ForbiddenAccessException.NotOwner();

        var result = await _unitOfWork.Products.DeleteAsync(request.Id);
        if (result)
            await _unitOfWork.SaveChangesAsync();

        return result;
    }
}
