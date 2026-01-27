using MediatR;

namespace ShopxBase.Application.Features.Compare.Commands.RemoveFromCompare;

/// <summary>
/// Command to remove a product from user's compare list
/// </summary>
public class RemoveFromCompareCommand : IRequest<bool>
{
    public int CompareId { get; set; }

    // UserId for ownership validation (set from ICurrentUserService)
    public string UserId { get; set; } = string.Empty;
}
