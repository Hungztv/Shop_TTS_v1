using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Application.DTOs.Coupon;

namespace ShopxBase.Application.Features.Coupons.Queries.GetCouponByCode;

public class GetCouponByCodeQueryHandler : IRequestHandler<GetCouponByCodeQuery, CouponDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCouponByCodeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CouponDto?> Handle(GetCouponByCodeQuery request, CancellationToken cancellationToken)
    {
        var coupon = await _unitOfWork.Coupons.FirstOrDefaultAsync(c => c.Code == request.Code);

        return coupon == null ? null : _mapper.Map<CouponDto>(coupon);
    }
}
