using MediatR;
using ShopxBase.Application.DTOs.Brand;

namespace ShopxBase.Application.Features.Brands.Queries.GetBrands;

public class GetBrandsQuery : IRequest<IEnumerable<BrandDto>>
{
    // TODO: Add properties
}
