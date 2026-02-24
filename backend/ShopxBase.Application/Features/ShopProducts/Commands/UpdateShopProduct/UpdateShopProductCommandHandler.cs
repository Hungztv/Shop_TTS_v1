using AutoMapper;
using MediatR;
using ShopxBase.Application.DTOs.Product;
using ShopxBase.Application.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.ShopProducts.Commands.UpdateShopProduct;

public class UpdateShopProductCommandHandler : IRequestHandler<UpdateShopProductCommand, ProductDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public UpdateShopProductCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<ProductDto> Handle(UpdateShopProductCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw UnauthorizedUserException.UserIdNotFound();

        var shop = await _unitOfWork.Shops.FirstOrDefaultAsync(s => s.OwnerUserId == userId);
        if (shop == null)
            throw new ShopNotFoundException("Bạn chưa có shop được duyệt");

        var product = await _unitOfWork.Products.GetByIdAsync(request.Id);
        if (product == null)
            throw new InvalidProductException($"Sản phẩm với Id {request.Id} không tồn tại");

        if (product.ShopId != shop.Id)
            throw ForbiddenAccessException.NotOwner();

        var brand = await _unitOfWork.Brands.GetByIdAsync(request.BrandId);
        if (brand == null)
            throw new BrandNotFoundException($"Thương hiệu với Id {request.BrandId} không tồn tại");

        var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId);
        if (category == null)
            throw new CategoryNotFoundException($"Danh mục với Id {request.CategoryId} không tồn tại");

        _mapper.Map(request, product);
        product.ShopId = shop.Id;

        await _unitOfWork.Products.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync();

        var dto = _mapper.Map<ProductDto>(product);
        dto.BrandName = brand.Name;
        dto.CategoryName = category.Name;
        dto.ShopName = shop.Name;
        dto.ShopId = shop.Id;
        dto.IsInStock = product.IsInStock();

        return dto;
    }
}
