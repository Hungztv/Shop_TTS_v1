using MediatR;
using ShopxBase.Application.DTOs.Brand;

namespace ShopxBase.Application.Features.Brands.Commands.UpdateBrand;

public class UpdateBrandCommand : IRequest<BrandDto>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Slug { get; set; }
    public string Status { get; set; }
    public string? Logo { get; set; }
}
