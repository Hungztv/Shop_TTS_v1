using MediatR;
using ShopxBase.Application.DTOs.Order;

namespace ShopxBase.Application.Features.Orders.Queries.GetSellerOrderDetail;

public class GetSellerOrderDetailQuery : IRequest<SellerOrderDetailDto>
{
    public int OrderId { get; set; }
}
