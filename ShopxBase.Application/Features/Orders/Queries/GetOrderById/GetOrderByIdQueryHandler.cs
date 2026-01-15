using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.DTOs.Order;

namespace ShopxBase.Application.Features.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        // Get order with details using specialized repository
        var order = await _unitOfWork.OrderRepository.GetWithDetailsAsync(request.OrderId);

        if (order == null)
        {
            throw OrderNotFoundException.ById(request.OrderId);
        }

        // Map to DTO
        var orderDto = _mapper.Map<OrderDto>(order);

        return orderDto;
    }
}
