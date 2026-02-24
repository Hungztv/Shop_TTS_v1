using AutoMapper;
using MediatR;
using ShopxBase.Application.DTOs.Shop;
using ShopxBase.Application.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.Shops.Commands.UpdateShop;

public class UpdateShopCommandHandler : IRequestHandler<UpdateShopCommand, ShopDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public UpdateShopCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<ShopDto> Handle(UpdateShopCommand request, CancellationToken cancellationToken)
    {
        var shop = await _unitOfWork.Shops.GetByIdAsync(request.Id);
        if (shop == null)
            throw new ShopNotFoundException($"Shop với Id {request.Id} không tồn tại");

        var userId = _currentUserService.UserId
            ?? throw UnauthorizedUserException.UserIdNotFound();

        if (!string.Equals(shop.OwnerUserId, userId, StringComparison.OrdinalIgnoreCase))
            throw ForbiddenAccessException.NotOwner();

        if (!string.Equals(shop.Slug, request.Slug, StringComparison.OrdinalIgnoreCase))
        {
            var exists = await _unitOfWork.Shops.AnyAsync(s => s.Slug == request.Slug && s.Id != shop.Id);
            if (exists)
                throw new DuplicateShopSlugException("Slug shop đã tồn tại");
        }

        _mapper.Map(request, shop);
        await _unitOfWork.Shops.UpdateAsync(shop);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ShopDto>(shop);
    }
}
