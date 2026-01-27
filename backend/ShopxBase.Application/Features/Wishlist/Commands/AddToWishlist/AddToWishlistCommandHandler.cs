using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Entities;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.DTOs.Wishlist;
using ShopxBase.Application.Interfaces;

namespace ShopxBase.Application.Features.Wishlist.Commands.AddToWishlist;

public class AddToWishlistCommandHandler : IRequestHandler<AddToWishlistCommand, WishlistDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public AddToWishlistCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<WishlistDto> Handle(AddToWishlistCommand request, CancellationToken cancellationToken)
    {
        // 1. SECURITY: Get user ID from token
        if (string.IsNullOrEmpty(_currentUserService.UserId))
            throw UnauthorizedUserException.UserIdNotFound();

        var userId = _currentUserService.UserId;

        // 2. Check if product exists
        var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
        if (product == null || product.IsDeleted)
            throw ProductNotFoundException.WithMessage("Product not found");

        // 3. Check for duplicate
        var existingItems = await _unitOfWork.Wishlists.FindAsync(
            w => w.UserId == userId && w.ProductId == request.ProductId);

        if (existingItems.Any())
            throw WishlistDuplicateException.AlreadyExists(request.ProductId);

        // 4. Create wishlist item
        var wishlistItem = new Domain.Entities.Wishlist
        {
            UserId = userId,
            ProductId = request.ProductId
        };

        await _unitOfWork.Wishlists.AddAsync(wishlistItem);
        await _unitOfWork.SaveChangesAsync();

        // 5. Build response DTO
        return new WishlistDto
        {
            Id = wishlistItem.Id,
            ProductId = product.Id,
            ProductName = product.Name,
            ProductSlug = product.Slug,
            ProductImage = product.Image,
            ProductPrice = product.Price,
            ProductCapitalPrice = product.CapitalPrice,
            ProductQuantity = product.Quantity,
            IsInStock = product.Quantity > 0,
            BrandName = product.Brand?.Name ?? "",
            CategoryName = product.Category?.Name ?? "",
            CreatedAt = wishlistItem.CreatedAt
        };
    }
}
