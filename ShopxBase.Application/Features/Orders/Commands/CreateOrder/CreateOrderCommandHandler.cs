using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Entities;
using ShopxBase.Application.DTOs.Order;
using ShopxBase.Domain.Exceptions;
using System.Transactions;

namespace ShopxBase.Application.Features.Orders.Commands.CreateOrder;

/// Handler for CreateOrderCommand
/// Implements business logic for order creation with full validation and transaction handling

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private const decimal ShippingCost = 30000m;  // Fixed shipping cost

    public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // 1. VALIDATE USER EXISTS
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
        if (user == null || user.IsDeleted)
            throw OrderNotFoundException.WithMessage($"User '{request.UserId}' not found or is inactive");

        //2. VALIDATE & PREPARE PRODUCTS
        var orderProducts = new List<(Product product, CreateOrderDetailCommand detail)>();
        decimal subtotal = 0;

        foreach (var detail in request.OrderDetails)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(detail.ProductId);

            // Validate product exists
            if (product == null || product.IsDeleted)
                throw new InvalidProductException($"Product '{detail.ProductId}' not found or is inactive");

            // Validate stock
            if (product.Quantity < detail.Quantity)
                throw InsufficientStockException.For(
                    product.Name,
                    detail.Quantity,
                    product.Quantity);

            // Calculate line total
            decimal lineTotal = product.Price * detail.Quantity;
            subtotal += lineTotal;

            orderProducts.Add((product, detail));
        }

        //3. VALIDATE & APPLY COUPON 
        Coupon coupon = null;
        decimal discountAmount = 0;

        if (!string.IsNullOrEmpty(request.CouponCode))
        {
            coupon = await _unitOfWork.CouponRepository.GetByCodeAsync(request.CouponCode);

            // Validate coupon exists
            if (coupon == null || coupon.IsDeleted)
                throw InvalidCouponException.NotFound(request.CouponCode);

            // Validate coupon is valid (active, not expired, has quantity)
            if (!coupon.IsValid())
            {
                if (coupon.IsExpired())
                    throw InvalidCouponException.Expired(request.CouponCode);

                if (coupon.IsOutOfStock())
                    throw InvalidCouponException.OutOfStock(request.CouponCode);

                throw InvalidCouponException.Inactive(request.CouponCode);
            }

            // Validate coupon can be applied to this order
            if (!coupon.CanApply(subtotal))
                throw InvalidCouponException.MinimumOrderNotMet(
                    request.CouponCode,
                    coupon.MinimumOrderValue,
                    subtotal);

            // Calculate discount
            discountAmount = coupon.CalculateDiscount(subtotal);
        }

        // 4. GENERATE UNIQUE ORDER CODE
        string orderCode = await GenerateUniqueOrderCodeAsync();

        //5. CALCULATE FINAL TOTAL
        decimal total = subtotal + ShippingCost - discountAmount;

        // Validate total is positive
        if (total < 0)
            throw OrderNotFoundException.WithMessage("Order total cannot be negative. Check discount amount.");

        //6. CREATE ORDER WITH TRANSACTION
        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                // Create Order entity
                var order = new Order
                {
                    OrderCode = orderCode,
                    UserId = request.UserId,
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    Email = request.Email ?? "",
                    Note = request.Note ?? "",
                    PaymentMethod = request.PaymentMethod,
                    Status = 0,  // Pending
                    PaymentStatus = "0",  // Unpaid
                    ShippingCost = ShippingCost,
                    Subtotal = subtotal,
                    DiscountAmount = discountAmount,
                    Total = total,
                    CouponId = coupon?.Id,
                    CouponCode = request.CouponCode ?? ""
                };

                // Add OrderDetails
                foreach (var (product, detail) in orderProducts)
                {
                    var orderDetail = new OrderDetail
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        ProductImage = product.Image,
                        Price = product.Price,
                        Quantity = detail.Quantity,
                        OrderCode = orderCode
                    };

                    order.OrderDetails.Add(orderDetail);
                }

                // Save Order
                await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.SaveChangesAsync();

                //7. APPLY COUPON (increment usage)
                if (coupon != null)
                {
                    coupon.Use();  // Increment UsedCount
                    await _unitOfWork.Coupons.UpdateAsync(coupon);
                    await _unitOfWork.SaveChangesAsync();
                }

                transaction.Complete();

                //8. MAP TO DTO & RETURN
                var orderDto = _mapper.Map<OrderDto>(order);
                orderDto.StatusText = order.GetStatusText();

                return orderDto;
            }
            catch (Exception ex)
            {
                transaction.Dispose();
                throw;
            }
        }
    }


    /// Generate unique order code with format: ORD-{timestamp}-{random}
    /// Ensures uniqueness through database check

    private async Task<string> GenerateUniqueOrderCodeAsync()
    {
        string orderCode;
        bool exists;
        int attempts = 0;
        const int maxAttempts = 10;

        do
        {
            attempts++;
            if (attempts > maxAttempts)
                throw OrderNotFoundException.WithMessage("Failed to generate unique order code after maximum attempts");

            // Format: ORD-20260115143025-1234
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var random = new Random().Next(1000, 9999);
            orderCode = $"ORD-{timestamp}-{random}";

            exists = await _unitOfWork.OrderRepository.ExistsByCodeAsync(orderCode);

        } while (exists);

        return orderCode;
    }
}
