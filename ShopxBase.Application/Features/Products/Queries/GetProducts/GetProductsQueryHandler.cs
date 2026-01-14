using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Application.DTOs.Product;
using ShopxBase.Application.DTOs.Common;

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
        // 1. Get paginated products
        var (products, totalCount) = await _unitOfWork.ProductRepository.GetPaginatedAsync(
            request.PageNumber,
            request.PageSize);

        // 2. Map entities to DTOs
        var productDtos = _mapper.Map<List<ProductDto>>(products);

        // 3. Calculate pagination info
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);
        var hasNextPage = request.PageNumber < totalPages;
        var hasPreviousPage = request.PageNumber > 1;

        // 4. Return pagination response
        return new PaginationResponse<ProductDto>
        {
            Items = productDtos,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasNext = hasNextPage,
            HasPrevious = hasPreviousPage
        };
    }
}
