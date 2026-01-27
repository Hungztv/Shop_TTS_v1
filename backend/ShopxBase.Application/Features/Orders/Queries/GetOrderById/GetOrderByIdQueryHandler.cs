using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.DTOs.Order;
using ShopxBase.Application.Interfaces;

namespace ShopxBase.Application.Features.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetOrderByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        // Get order with details using specialized repository
        var order = await _unitOfWork.OrderRepository.GetWithDetailsAsync(request.OrderId);

        if (order == null)
        {
            throw OrderNotFoundException.ById(request.OrderId);
        }

        // AUTHORIZATION: Check ownership for non-admin/seller users
        if (!_currentUserService.IsAdminOrSeller)
        {
            if (string.IsNullOrEmpty(_currentUserService.UserId))
            {
                throw UnauthorizedUserException.UserIdNotFound();
            }

            if (order.UserId != _currentUserService.UserId)
            {
                throw ForbiddenAccessException.NotOwner();
            }
        }

        // Map to DTO
        var orderDto = _mapper.Map<OrderDto>(order);

        return orderDto;
    }
}
