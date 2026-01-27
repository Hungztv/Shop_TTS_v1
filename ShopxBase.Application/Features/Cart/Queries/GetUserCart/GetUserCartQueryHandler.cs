using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.DTOs.Cart;
using ShopxBase.Application.Interfaces;

namespace ShopxBase.Application.Features.Cart.Queries.GetUserCart;

/// <summary>
/// Handler for GetUserCartQuery
/// </summary>
public class GetUserCartQueryHandler : IRequestHandler<GetUserCartQuery, CartSummaryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetUserCartQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<CartSummaryDto> Handle(GetUserCartQuery request, CancellationToken cancellationToken)
    {
        // SECURITY: Validate that user is authenticated
        if (string.IsNullOrEmpty(_currentUserService.UserId))
            throw UnauthorizedUserException.UserIdNotFound();

        // Get the authenticated user's ID from token (ignore request.UserId for security)
        var userId = _currentUserService.UserId;

        // 1. Get all cart items for authenticated user
        var cartItems = await _unitOfWork.Carts.FindAsync(c => c.UserId == userId);

        // 2. Build cart DTOs with product details (filter out deleted products)
        var cartDtos = new List<CartDto>();

        foreach (var item in cartItems)
        {
            // Get product for each cart item
            var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);

            // Skip if product is deleted or doesn't exist
            if (product == null)
                continue;

            cartDtos.Add(new CartDto
            {
                Id = item.Id,
                ProductId = product.Id,
                ProductName = product.Name,
                ProductImage = product.Image,
                ProductSlug = product.Slug,
                Price = product.Price,
                CapitalPrice = product.CapitalPrice,
                Quantity = item.Quantity,
                MaxQuantity = product.Quantity,
                Subtotal = product.Price * item.Quantity,
                CreatedAt = item.CreatedAt
            });
        }

        // 3. Calculate totals
        var totalItems = cartDtos.Sum(c => c.Quantity);
        var totalPrice = cartDtos.Sum(c => c.Subtotal);
        var uniqueProducts = cartDtos.Count;

        // 4. Return CartSummaryDto
        return new CartSummaryDto
        {
            Items = cartDtos,
            TotalItems = totalItems,
            TotalPrice = totalPrice,
            UniqueProducts = uniqueProducts
        };
    }
}
