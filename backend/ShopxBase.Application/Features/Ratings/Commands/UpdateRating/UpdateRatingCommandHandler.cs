using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Entities;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.DTOs.Rating;
using ShopxBase.Application.Interfaces;

namespace ShopxBase.Application.Features.Ratings.Commands.UpdateRating;

public class UpdateRatingCommandHandler : IRequestHandler<UpdateRatingCommand, RatingDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public UpdateRatingCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<RatingDto> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
    {
        // 1. SECURITY: Get user ID from token
        if (string.IsNullOrEmpty(_currentUserService.UserId))
            throw UnauthorizedUserException.UserIdNotFound();

        var userId = _currentUserService.UserId;

        // 2. Get rating
        var rating = await _unitOfWork.Ratings.GetByIdAsync(request.RatingId);
        if (rating == null || rating.IsDeleted)
            throw RatingNotFoundException.NotFound(request.RatingId);

        // 3. SECURITY: Validate ownership (only owner or admin can update)
        if (rating.UserId != userId && !_currentUserService.IsAdmin)
            throw RatingUnauthorizedException.UnauthorizedAccess();

        // 4. Get product for recalculation
        var product = await _unitOfWork.Products.GetByIdAsync(rating.ProductId);
        if (product == null)
            throw ProductNotFoundException.WithMessage("Product not found");

        // 5. Update rating
        rating.Star = request.Star;
        rating.Comment = request.Comment;
        rating.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Ratings.UpdateAsync(rating);

        // 6. TRANSACTION: Recalculate product stats
        await RecalculateProductRatingStats(product);

        // 7. Save all changes
        await _unitOfWork.SaveChangesAsync();

        // 8. Get user for response
        var user = await _unitOfWork.Users.GetByIdAsync(rating.UserId);

        // 9. Build response DTO
        return new RatingDto
        {
            Id = rating.Id,
            ProductId = rating.ProductId,
            ProductName = product.Name,
            UserId = rating.UserId,
            UserName = user?.FullName ?? rating.Name,
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

    private async Task RecalculateProductRatingStats(Product product)
    {
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
