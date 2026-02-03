using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Entities;
using ShopxBase.Application.DTOs.Product;
using ShopxBase.Application.DTOs.Common;
using System.Linq.Expressions;

namespace ShopxBase.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PaginationResponse<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        // Build filter
        int? categoryId = request.CategoryId;
        int? brandId = request.BrandId;
        string? search = request.Search?.ToLower();
        decimal? minPrice = request.MinPrice;
        decimal? maxPrice = request.MaxPrice;

        Expression<Func<Product, bool>> predicate = p =>
            !p.IsDeleted &&
            (!categoryId.HasValue || p.CategoryId == categoryId.Value) &&
            (!brandId.HasValue || p.BrandId == brandId.Value) &&
            (string.IsNullOrEmpty(search) || p.Name.ToLower().Contains(search)) &&
            (!minPrice.HasValue || p.Price >= minPrice.Value) &&
            (!maxPrice.HasValue || p.Price <= maxPrice.Value);

        // Get filtered and paginated products
        var (products, totalCount) = await _unitOfWork.ProductRepository.GetFilteredAsync(
            predicate,
            request.PageNumber,
            request.PageSize);

        // Map entities to DTOs
        var productDtos = _mapper.Map<List<ProductDto>>(products);

        // Return pagination response
        return new PaginationResponse<ProductDto>
        {
            Items = productDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
