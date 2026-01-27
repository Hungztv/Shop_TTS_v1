using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Application.DTOs.Coupon;
using ShopxBase.Domain.Entities;
using ShopxBase.Domain.Exceptions;

namespace ShopxBase.Application.Features.Coupons.Commands.CreateCoupon;

public class CreateCouponCommandHandler : IRequestHandler<CreateCouponCommand, CouponDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCouponCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CouponDto> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
    {
        // Check if coupon code already exists
        var existingCoupon = await _unitOfWork.Coupons.FirstOrDefaultAsync(c => c.Code == request.Code);
        if (existingCoupon != null)
            throw new DomainException($"Mã coupon '{request.Code}' đã tồn tại");

        var coupon = _mapper.Map<Coupon>(request);
        coupon.UsedCount = 0;

        await _unitOfWork.Coupons.AddAsync(coupon);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CouponDto>(coupon);
    }
}
