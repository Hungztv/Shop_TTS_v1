using AutoMapper;
using MediatR;
using ShopxBase.Application.DTOs.Common;
using ShopxBase.Application.DTOs.Product;
using ShopxBase.Application.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.ShopProducts.Queries.GetShopProducts;

public class GetShopProductsQueryHandler : IRequestHandler<GetShopProductsQuery, PaginationResponse<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetShopProductsQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<PaginationResponse<ProductDto>> Handle(GetShopProductsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw UnauthorizedUserException.UserIdNotFound();

        var shop = await _unitOfWork.Shops.FirstOrDefaultAsync(s => s.OwnerUserId == userId);
        if (shop == null)
            throw new ShopNotFoundException("Bạn chưa có shop được duyệt");

        var (items, total) = await _unitOfWork.ProductRepository.GetFilteredAsync(
            p => p.ShopId == shop.Id,
            request.PageNumber,
            request.PageSize);

        var dtos = _mapper.Map<List<ProductDto>>(items);
        foreach (var dto in dtos)
        {
            dto.ShopId = shop.Id;
            dto.ShopName = shop.Name;
        }

        return new PaginationResponse<ProductDto>
        {
            Items = dtos,
            TotalCount = total,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
