using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Entities;
using ShopxBase.Application.DTOs.Product;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.Interfaces;

namespace ShopxBase.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public UpdateProductCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
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

        // 4. Validate Shop exists
        var shop = await _unitOfWork.Shops.GetByIdAsync(request.ShopId);
        if (shop == null)
            throw new ShopNotFoundException($"Shop với Id {request.ShopId} không tồn tại");

        // 5. Ownership check for seller
        if (!_currentUserService.IsAdmin)
        {
            var userId = _currentUserService.UserId
                ?? throw UnauthorizedUserException.UserIdNotFound();

            if (!string.Equals(shop.OwnerUserId, userId, StringComparison.OrdinalIgnoreCase))
                throw ForbiddenAccessException.NotOwner();
        }

        if (product.ShopId != request.ShopId)
            throw ForbiddenAccessException.WithMessage("ShopId không khớp với sản phẩm");

        // 6. Map command to entity (update existing)
        _mapper.Map(request, product);

        // 7. Update repository
        await _unitOfWork.Products.UpdateAsync(product);

        // 8. Save changes
        await _unitOfWork.SaveChangesAsync();

        // 9. Map entity to DTO
        var productDto = _mapper.Map<ProductDto>(product);

        // 10. Enrich with related data
        productDto.BrandName = brand.Name;
        productDto.CategoryName = category.Name;
        productDto.ShopName = shop.Name;
        productDto.IsInStock = product.IsInStock();

        return productDto;
    }
}
