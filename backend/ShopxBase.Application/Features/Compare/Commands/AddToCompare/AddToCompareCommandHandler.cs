using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Entities;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.DTOs.Compare;
using ShopxBase.Application.Interfaces;

namespace ShopxBase.Application.Features.Compare.Commands.AddToCompare;

public class AddToCompareCommandHandler : IRequestHandler<AddToCompareCommand, CompareItemDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private const int MaxCompareItems = 5;

    public AddToCompareCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<CompareItemDto> Handle(AddToCompareCommand request, CancellationToken cancellationToken)
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
        var existingItems = await _unitOfWork.CompareProducts.FindAsync(
            c => c.UserId == userId && c.ProductId == request.ProductId);

        if (existingItems.Any())
            throw CompareDuplicateException.AlreadyExists(request.ProductId);

        // 4. Check max limit (5 items)
        var userCompareItems = await _unitOfWork.CompareProducts.FindAsync(c => c.UserId == userId);
        if (userCompareItems.Count() >= MaxCompareItems)
            throw CompareListFullException.MaxItemsReached();

        // 5. Create compare item
        var compareItem = new CompareProduct
        {
            UserId = userId,
            ProductId = request.ProductId
        };

        await _unitOfWork.CompareProducts.AddAsync(compareItem);
        await _unitOfWork.SaveChangesAsync();

        // 6. Build response DTO with full product details
        return new CompareItemDto
        {
            Id = compareItem.Id,
            ProductId = product.Id,
            ProductName = product.Name,
            ProductSlug = product.Slug,
            ProductDescription = product.Description,
            ProductImage = product.Image,
            ProductPrice = product.Price,
            ProductCapitalPrice = product.CapitalPrice,
            ProductQuantity = product.Quantity,
            ProductSoldOut = product.SoldOut,
            IsInStock = product.Quantity > 0,
            AverageScore = product.AverageScore,
            RatingCount = product.RatingCount,
            BrandId = product.BrandId,
            BrandName = product.Brand?.Name ?? "",
            BrandLogo = product.Brand?.Logo ?? "",
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? "",
            CreatedAt = compareItem.CreatedAt
        };
    }
}
