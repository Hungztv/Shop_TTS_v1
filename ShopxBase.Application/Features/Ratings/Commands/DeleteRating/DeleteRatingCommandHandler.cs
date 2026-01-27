using MediatR;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Entities;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.Interfaces;

namespace ShopxBase.Application.Features.Ratings.Commands.DeleteRating;

public class DeleteRatingCommandHandler : IRequestHandler<DeleteRatingCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public DeleteRatingCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(DeleteRatingCommand request, CancellationToken cancellationToken)
    {
        // 1. SECURITY: Get user ID from token
        if (string.IsNullOrEmpty(_currentUserService.UserId))
            throw UnauthorizedUserException.UserIdNotFound();

        var userId = _currentUserService.UserId;

        // 2. Get rating
        var rating = await _unitOfWork.Ratings.GetByIdAsync(request.RatingId);
        if (rating == null || rating.IsDeleted)
            throw RatingNotFoundException.NotFound(request.RatingId);

        // 3. SECURITY: Validate ownership (only owner or admin can delete)
        if (rating.UserId != userId && !_currentUserService.IsAdmin)
            throw RatingUnauthorizedException.UnauthorizedAccess();

        // 4. Get product for recalculation
        var product = await _unitOfWork.Products.GetByIdAsync(rating.ProductId);

        // 5. Soft delete rating
        rating.IsDeleted = true;
        rating.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.Ratings.UpdateAsync(rating);

        // 6. TRANSACTION: Recalculate product stats
        if (product != null)
        {
            await RecalculateProductRatingStats(product);
        }

        // 7. Save all changes
        await _unitOfWork.SaveChangesAsync();

        return true;
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
