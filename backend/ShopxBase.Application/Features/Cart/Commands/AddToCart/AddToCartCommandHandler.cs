using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.DTOs.Cart;
using ShopxBase.Application.Interfaces;

namespace ShopxBase.Application.Features.Cart.Commands.AddToCart;

public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, CartDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public AddToCartCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<CartDto> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        // SECURITY: Validate that the UserId matches the current user
        if (string.IsNullOrEmpty(_currentUserService.UserId))
        {
            throw UnauthorizedUserException.UserIdNotFound();
        }

        // Override UserId with token value for security
        var userId = _currentUserService.UserId;

        // 1. Validate product exists and not deleted
        var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
        if (product == null)
            throw ProductNotFoundException.WithMessage("Sản phẩm không tồn tại");

        // 2. Validate quantity > 0 and <= product.Quantity
        if (request.Quantity <= 0)
            throw new InvalidProductException("Số lượng phải lớn hơn 0");

        // 3. Check if product already in user's cart
        var existingCartItem = await _unitOfWork.Carts
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == request.ProductId);

        Domain.Entities.Cart cartItem;

        if (existingCartItem != null)
        {
            // 4. If exists: update quantity (existing + new), validate total <= stock
            var newQuantity = existingCartItem.Quantity + request.Quantity;

            if (newQuantity > product.Quantity)
                throw InsufficientStockException.WithMessage($"Không đủ hàng trong kho (còn {product.Quantity} sản phẩm)");

            existingCartItem.UpdateQuantity(newQuantity);
            await _unitOfWork.Carts.UpdateAsync(existingCartItem);
            cartItem = existingCartItem;
        }
        else
        {
            // 5. If new: validate quantity <= stock and create new cart item
            if (request.Quantity > product.Quantity)
                throw InsufficientStockException.WithMessage($"Không đủ hàng trong kho (còn {product.Quantity} sản phẩm)");

            cartItem = new Domain.Entities.Cart
            {
                UserId = userId,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Carts.AddAsync(cartItem);
        }

        // 6. Save changes
        await _unitOfWork.SaveChangesAsync();

        // 7. Return CartDto with product details
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
