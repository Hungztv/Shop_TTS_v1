using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Entities;
using ShopxBase.Application.DTOs.Product;
using ShopxBase.Application.DTOs.Common;
using System.Linq.Expressions;

namespace ShopxBase.Application.Features.Shops.Queries.GetShopProducts;

public class GetShopProductsPublicQueryHandler : IRequestHandler<GetShopProductsPublicQuery, PaginationResponse<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetShopProductsPublicQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<ProductDto>> Handle(GetShopProductsPublicQuery request, CancellationToken cancellationToken)
    {
        int shopId = request.ShopId;
        int? categoryId = request.CategoryId;
        string? search = request.Search?.ToLower();

        Expression<Func<Product, bool>> predicate = p =>
            p.ShopId == shopId &&
            !p.IsDeleted &&
            (!categoryId.HasValue || p.CategoryId == categoryId.Value) &&
            (string.IsNullOrEmpty(search) || p.Name.ToLower().Contains(search));

        var (products, totalCount) = await _unitOfWork.ProductRepository.GetFilteredAsync(
            predicate,
            request.PageNumber,
            request.PageSize);

        var productDtos = _mapper.Map<List<ProductDto>>(products);

        return new PaginationResponse<ProductDto>
        {
            Items = productDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
