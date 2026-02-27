using MediatR;
using ShopxBase.Application.DTOs.Shop;

namespace ShopxBase.Application.Features.Shops.Queries.GetShopBySlug;

public class GetShopBySlugQuery : IRequest<ShopPublicDto>
{
    public string Slug { get; set; }
}
