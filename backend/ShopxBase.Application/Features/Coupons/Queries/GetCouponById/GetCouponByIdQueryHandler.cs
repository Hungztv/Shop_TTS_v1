using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Application.DTOs.Coupon;

namespace ShopxBase.Application.Features.Coupons.Queries.GetCouponById;

public class GetCouponByIdQueryHandler : IRequestHandler<GetCouponByIdQuery, CouponDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCouponByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CouponDto?> Handle(GetCouponByIdQuery request, CancellationToken cancellationToken)
    {
        var coupon = await _unitOfWork.Coupons.GetByIdAsync(request.Id);

        return coupon == null ? null : _mapper.Map<CouponDto>(coupon);
    }
}
