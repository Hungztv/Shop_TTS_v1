using MediatR;
using ShopxBase.Application.DTOs.Shop;

namespace ShopxBase.Application.Features.Shops.Queries.GetMyShop;

public class GetMyShopQuery : IRequest<ShopDto?>
{
}
