using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.DTOs.Cart;
using ShopxBase.Application.Interfaces;

namespace ShopxBase.Application.Features.Cart.Commands.UpdateCartQuantity;

public class UpdateCartQuantityCommandHandler : IRequestHandler<UpdateCartQuantityCommand, CartDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public UpdateCartQuantityCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<CartDto> Handle(UpdateCartQuantityCommand request, CancellationToken cancellationToken)
    {
        // SECURITY: Validate that user is authenticated
        if (string.IsNullOrEmpty(_currentUserService.UserId))
            throw UnauthorizedUserException.UserIdNotFound();

        // Get the authenticated user's ID from token
        var userId = _currentUserService.UserId;

        // 1. Get cart item
        var cartItem = await _unitOfWork.Carts.GetByIdAsync(request.CartId);

        // 2. Validate cart item exists
        if (cartItem == null)
            throw CartNotFoundException.WithMessage("Giỏ hàng không tồn tại");

        // 3. SECURITY: Validate cart item belongs to current user (from token, not request)
        if (cartItem.UserId != userId)
            throw CartUnauthorizedException.UnauthorizedAccess();

        // 4. Get product to validate stock
        var product = await _unitOfWork.Products.GetByIdAsync(cartItem.ProductId);
        if (product == null)
            throw ProductNotFoundException.WithMessage("Sản phẩm không tồn tại");

        // 5. Validate quantity <= product.Quantity
        if (request.Quantity > product.Quantity)
            throw InsufficientStockException.WithMessage($"Không đủ hàng trong kho (còn {product.Quantity} sản phẩm)");

        // 6. Update cart.Quantity and cart.UpdatedAt
        cartItem.UpdateQuantity(request.Quantity);

        // 7. Save changes
        await _unitOfWork.Carts.UpdateAsync(cartItem);
        await _unitOfWork.SaveChangesAsync();

        // 8. Return CartDto
        return new CartDto
        {
            Id = cartItem.Id,
            ProductId = product.Id,
            ProductName = product.Name,
            ProductImage = product.Image,
            ProductSlug = product.Slug,
            Price = product.Price,
            CapitalPrice = product.CapitalPrice,
            Quantity = cartItem.Quantity,
            MaxQuantity = product.Quantity,
            Subtotal = product.Price * cartItem.Quantity,
            CreatedAt = cartItem.CreatedAt
        };
    }
}
