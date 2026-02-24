using MediatR;
using ShopxBase.Application.DTOs.Shop;

namespace ShopxBase.Application.Features.Shops.Commands.UpdateShop;

public class UpdateShopCommand : IRequest<ShopDto>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string? CoverUrl { get; set; }
}
