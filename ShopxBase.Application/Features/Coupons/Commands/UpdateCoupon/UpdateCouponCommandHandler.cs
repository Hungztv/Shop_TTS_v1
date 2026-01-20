using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Application.DTOs.Coupon;
using ShopxBase.Domain.Exceptions;

namespace ShopxBase.Application.Features.Coupons.Commands.UpdateCoupon;

public class UpdateCouponCommandHandler : IRequestHandler<UpdateCouponCommand, CouponDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCouponCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CouponDto> Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = await _unitOfWork.Coupons.GetByIdAsync(request.Id);
        if (coupon == null)
            throw new InvalidCouponException($"Coupon với Id {request.Id} không tồn tại");

        // Check if code changed and new code already exists
        if (coupon.Code != request.Code)
        {
            var existingCoupon = await _unitOfWork.Coupons.FirstOrDefaultAsync(c => c.Code == request.Code);
            if (existingCoupon != null)
                throw new DomainException($"Mã coupon '{request.Code}' đã tồn tại");
        }

        // Don't allow reducing Quantity below UsedCount
        if (request.Quantity < coupon.UsedCount)
            throw new DomainException($"Số lượng không thể nhỏ hơn số lượng đã sử dụng ({coupon.UsedCount})");

        _mapper.Map(request, coupon);

        await _unitOfWork.Coupons.UpdateAsync(coupon);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CouponDto>(coupon);
    }
}
