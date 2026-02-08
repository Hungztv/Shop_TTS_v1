using MediatR;

namespace ShopxBase.Application.Features.Coupons.Commands.DeleteCoupon;

public class DeleteCouponCommand : IRequest<DeleteCouponResult>
{
    public int Id { get; set; }
}

