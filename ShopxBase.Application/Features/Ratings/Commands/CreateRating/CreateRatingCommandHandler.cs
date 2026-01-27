using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Entities;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.DTOs.Rating;
using ShopxBase.Application.Interfaces;

namespace ShopxBase.Application.Features.Ratings.Commands.CreateRating;

public class CreateRatingCommandHandler : IRequestHandler<CreateRatingCommand, RatingDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CreateRatingCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<RatingDto> Handle(CreateRatingCommand request, CancellationToken cancellationToken)
    {
        // 1. SECURITY: Get user ID from token
        if (string.IsNullOrEmpty(_currentUserService.UserId))
            throw UnauthorizedUserException.UserIdNotFound();

        var userId = _currentUserService.UserId;

        // 2. Check if product exists
        var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
        if (product == null || product.IsDeleted)
            throw ProductNotFoundException.WithMessage("Product not found");

        // 3. Check for duplicate rating (user already rated this product)
        var existingRatings = await _unitOfWork.Ratings.FindAsync(
            r => r.UserId == userId && r.ProductId == request.ProductId && !r.IsDeleted);

        if (existingRatings.Any())
            throw RatingDuplicateException.AlreadyRated(request.ProductId);

        // 4. Get user for username
        var user = await _unitOfWork.Users.GetByIdAsync(userId);

        // 5. Create rating entity
        var rating = new Rating
        {
            ProductId = request.ProductId,
            UserId = userId,
            Star = request.Star,
            Comment = request.Comment,
            Name = request.Name,
            Email = request.Email,
            IsVerifiedPurchase = false, // Can be set based on order history
            IsApproved = true // Auto-approve or set to false for moderation
        };

        // 6. TRANSACTION: Add rating and update product stats atomically
        await _unitOfWork.Ratings.AddAsync(rating);

        // 7. Recalculate product rating statistics
        await RecalculateProductRatingStats(product);

        // 8. Save all changes
        await _unitOfWork.SaveChangesAsync();

        // 9. Build response DTO
        return new RatingDto
        {
            Id = rating.Id,
            ProductId = rating.ProductId,
            ProductName = product.Name,
            UserId = rating.UserId,
            UserName = user?.FullName ?? request.Name,
            Star = rating.Star,
            StarDisplay = rating.GetStarDisplay(),
            Comment = rating.Comment,
            Name = rating.Name,
            Email = rating.Email,
            IsVerifiedPurchase = rating.IsVerifiedPurchase,
            IsApproved = rating.IsApproved,
            CreatedAt = rating.CreatedAt
        };
    }

    /// <summary>
    /// Recalculate product's AverageScore and RatingCount
    /// </summary>
    private async Task RecalculateProductRatingStats(Product product)
    {
        // Get all approved ratings for this product
        var ratings = await _unitOfWork.Ratings.FindAsync(
            r => r.ProductId == product.Id && !r.IsDeleted && r.IsApproved);

        var ratingsList = ratings.ToList();

        if (ratingsList.Count == 0)
        {
            product.AverageScore = 0;
            product.RatingCount = 0;
        }
        else
        {
            product.RatingCount = ratingsList.Count;
            product.AverageScore = Math.Round((decimal)ratingsList.Average(r => r.Star), 2);
        }

        await _unitOfWork.Products.UpdateAsync(product);
    }
}
