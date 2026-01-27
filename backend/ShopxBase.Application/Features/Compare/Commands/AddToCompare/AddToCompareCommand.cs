using MediatR;
using ShopxBase.Application.DTOs.Compare;

namespace ShopxBase.Application.Features.Compare.Commands.AddToCompare;

/// <summary>
/// Command to add a product to user's compare list
/// </summary>
public class AddToCompareCommand : IRequest<CompareItemDto>
{
    public int ProductId { get; set; }

    // UserId will be set from ICurrentUserService
    public string UserId { get; set; } = string.Empty;
}
