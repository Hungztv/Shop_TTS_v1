using MediatR;
using ShopxBase.Application.DTOs.User;
using ShopxBase.Application.DTOs.Common;

namespace ShopxBase.Application.Features.Users.Queries.GetUsers;

public class GetUsersQuery : IRequest<PaginatedResult<AppUserDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public bool? IsActive { get; set; } // null = all, true = active (not deleted), false = deleted
    public DateTime? RegisteredAfter { get; set; }
    public DateTime? RegisteredBefore { get; set; }
}
