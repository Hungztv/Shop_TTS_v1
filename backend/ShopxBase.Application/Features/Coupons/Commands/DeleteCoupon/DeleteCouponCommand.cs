using MediatR;

namespace ShopxBase.Application.Features.Coupons.Commands.DeleteCoupon;

public class DeleteCouponCommand : IRequest<bool>
{
    public int Id { get; set; }
}
