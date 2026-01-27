using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Application.DTOs.Coupon;
using ShopxBase.Application.DTOs.Common;
using ShopxBase.Domain.Entities;
using System.Linq.Expressions;

namespace ShopxBase.Application.Features.Coupons.Queries.GetCoupons;

public class GetCouponsQueryHandler : IRequestHandler<GetCouponsQuery, PaginatedResult<CouponDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCouponsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<CouponDto>> Handle(GetCouponsQuery request, CancellationToken cancellationToken)
    {
        // Build filter predicate
        Expression<Func<Coupon, bool>> predicate = c => true;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchLower = request.SearchTerm.ToLower();
            predicate = c => c.Name.ToLower().Contains(searchLower) ||
                           c.Code.ToLower().Contains(searchLower) ||
                           c.Description.ToLower().Contains(searchLower);
        }

        if (request.Status.HasValue)
        {
            var currentPredicate = predicate;
            predicate = c => currentPredicate.Compile()(c) && c.Status == request.Status.Value;
        }

        var allCoupons = await _unitOfWork.Coupons.FindAsync(predicate);

        // Apply additional filters
        if (request.IsValid.HasValue)
        {
            var now = DateTime.Now;
            if (request.IsValid.Value)
            {
                allCoupons = allCoupons.Where(c => c.Status == 1 &&
                                                   c.Quantity > c.UsedCount &&
                                                   now >= c.DateStart &&
                                                   now <= c.DateExpired);
            }
            else
            {
                allCoupons = allCoupons.Where(c => c.Status == 0 ||
                                                   c.UsedCount >= c.Quantity ||
                                                   now < c.DateStart ||
                                                   now > c.DateExpired);
            }
        }

        if (request.IsExpired.HasValue)
        {
            var now = DateTime.Now;
            if (request.IsExpired.Value)
            {
                allCoupons = allCoupons.Where(c => now > c.DateExpired);
            }
            else
            {
                allCoupons = allCoupons.Where(c => now <= c.DateExpired);
            }
        }

        var totalCount = allCoupons.Count();

        // Apply pagination
        var coupons = allCoupons
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var couponDtos = _mapper.Map<IEnumerable<CouponDto>>(coupons);

        return new PaginatedResult<CouponDto>
        {
            Items = couponDtos.ToList(),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
