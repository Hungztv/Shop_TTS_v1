using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Entities;
using ShopxBase.Application.DTOs.Product;
using ShopxBase.Domain.Exceptions;

namespace ShopxBase.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate Brand exists
        var brand = await _unitOfWork.Brands.GetByIdAsync(request.BrandId);
        if (brand == null)
            throw new BrandNotFoundException($"Thương hiệu với Id {request.BrandId} không tồn tại");

        // 2. Validate Category exists
        var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId);
        if (category == null)
            throw new CategoryNotFoundException($"Danh mục với Id {request.CategoryId} không tồn tại");

        // 3. Map Command → Entity
        var product = _mapper.Map<Product>(request);

        // 4. Add to repository
        await _unitOfWork.Products.AddAsync(product);

        // 5. Save changes
        await _unitOfWork.SaveChangesAsync();

        // 6. Map Entity → DTO
        var productDto = _mapper.Map<ProductDto>(product);

        // 7. Enrich with related data
        productDto.BrandName = brand.Name;
        productDto.CategoryName = category.Name;
        productDto.IsInStock = product.IsInStock();

        return productDto;
    }
}
