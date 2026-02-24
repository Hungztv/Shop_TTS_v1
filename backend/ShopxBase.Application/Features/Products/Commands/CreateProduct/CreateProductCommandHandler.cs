using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Entities;
using ShopxBase.Application.DTOs.Product;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.Interfaces;

namespace ShopxBase.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CreateProductCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
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

        // 3. Validate Shop exists
        var shop = await _unitOfWork.Shops.GetByIdAsync(request.ShopId);
        if (shop == null)
            throw new ShopNotFoundException($"Shop với Id {request.ShopId} không tồn tại");

        // 4. Ownership check for seller
        if (!_currentUserService.IsAdmin)
        {
            var userId = _currentUserService.UserId
                ?? throw UnauthorizedUserException.UserIdNotFound();

            if (!string.Equals(shop.OwnerUserId, userId, StringComparison.OrdinalIgnoreCase))
                throw ForbiddenAccessException.NotOwner();
        }

        // 5. Map Command → Entity
        var product = _mapper.Map<Product>(request);

        // 6. Add to repository
        await _unitOfWork.Products.AddAsync(product);

        // 7. Save changes
        await _unitOfWork.SaveChangesAsync();

        // 8. Map Entity → DTO
        var productDto = _mapper.Map<ProductDto>(product);

        // 9. Enrich with related data
        productDto.BrandName = brand.Name;
        productDto.CategoryName = category.Name;
        productDto.ShopName = shop.Name;
        productDto.IsInStock = product.IsInStock();

        return productDto;
    }
}
