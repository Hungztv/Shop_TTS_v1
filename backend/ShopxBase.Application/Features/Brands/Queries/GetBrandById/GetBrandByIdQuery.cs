using MediatR;
using ShopxBase.Application.DTOs.Brand;

namespace ShopxBase.Application.Features.Brands.Queries.GetBrandById;

public class GetBrandByIdQuery : IRequest<BrandDto>
{
    public int Id { get; set; }
}
