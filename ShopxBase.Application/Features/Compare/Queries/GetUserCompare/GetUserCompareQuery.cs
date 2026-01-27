using MediatR;
using ShopxBase.Application.DTOs.Compare;

namespace ShopxBase.Application.Features.Compare.Queries.GetUserCompare;

/// <summary>
/// Query to get user's compare list summary
/// </summary>
public class GetUserCompareQuery : IRequest<CompareSummaryDto>
{
    // UserId for ownership (set from ICurrentUserService)
    public string UserId { get; set; } = string.Empty;
}
