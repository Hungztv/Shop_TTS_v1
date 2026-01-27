using MediatR;

namespace ShopxBase.Application.Features.Compare.Commands.ClearCompare;

/// <summary>
/// Command to clear all items from user's compare list
/// </summary>
public class ClearCompareCommand : IRequest<bool>
{
    // UserId for ownership (set from ICurrentUserService)
    public string UserId { get; set; } = string.Empty;
}
