using MediatR;
using ShopxBase.Application.DTOs.Brand;

namespace ShopxBase.Application.Features.Brands.Commands.CreateBrand;

public class CreateBrandCommand : IRequest<BrandDto>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Slug { get; set; }
    public string Status { get; set; } = "Active";
    public string? Logo { get; set; }
}
