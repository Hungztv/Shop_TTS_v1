using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Application.DTOs.Product;
using ShopxBase.Domain.Exceptions;

namespace ShopxBase.Application.Features.Products.Queries.GetProductBySlug;

public class GetProductBySlugQueryHandler : IRequestHandler<GetProductBySlugQuery, ProductDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductBySlugQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(GetProductBySlugQuery request, CancellationToken cancellationToken)
    {
        // 1. Get product by slug (using specialized repository)
        var product = await _unitOfWork.ProductRepository.GetBySlugAsync(request.Slug);
        if (product == null)
            throw new InvalidProductException($"Sản phẩm với slug '{request.Slug}' không tồn tại");

        // 2. Map entity to DTO
        var productDto = _mapper.Map<ProductDto>(product);

        // 3. Set calculated fields
        productDto.IsInStock = product.IsInStock();

        return productDto;
    }
}
