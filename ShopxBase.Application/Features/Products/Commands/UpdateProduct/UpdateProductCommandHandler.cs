using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Entities;
using ShopxBase.Application.DTOs.Product;
using ShopxBase.Domain.Exceptions;

namespace ShopxBase.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        // 1. Get product by id
        var product = await _unitOfWork.Products.GetByIdAsync(request.Id);
        if (product == null)
            throw new InvalidProductException($"Sản phẩm với Id {request.Id} không tồn tại");

        // 2. Validate Brand exists
        var brand = await _unitOfWork.Brands.GetByIdAsync(request.BrandId);
        if (brand == null)
            throw new BrandNotFoundException($"Thương hiệu với Id {request.BrandId} không tồn tại");

        // 3. Validate Category exists
        var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId);
        if (category == null)
            throw new CategoryNotFoundException($"Danh mục với Id {request.CategoryId} không tồn tại");

        // 4. Map command to entity (update existing)
        _mapper.Map(request, product);

        // 5. Update repository
        await _unitOfWork.Products.UpdateAsync(product);

        // 6. Save changes
        await _unitOfWork.SaveChangesAsync();

        // 7. Map entity to DTO
        var productDto = _mapper.Map<ProductDto>(product);

        // 8. Enrich with related data
        productDto.BrandName = brand.Name;
        productDto.CategoryName = category.Name;
        productDto.IsInStock = product.IsInStock();

        return productDto;
    }
}
