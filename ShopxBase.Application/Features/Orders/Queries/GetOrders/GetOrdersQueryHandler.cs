using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Application.DTOs.Order;
using ShopxBase.Application.DTOs.Common;

namespace ShopxBase.Application.Features.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, PaginationResponse<OrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOrdersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implement handler logic
        throw new NotImplementedException();
    }
}
