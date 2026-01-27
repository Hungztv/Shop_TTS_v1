using FluentValidation;
using ShopxBase.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ShopxBase.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        //USER VALIDATION
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID cannot be empty")
            .MustAsync(UserExists)
            .WithMessage("User does not exist or is deleted");

        // CUSTOMER INFO VALIDATION
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Recipient name is required")
            .MinimumLength(2)
            .WithMessage("Name must be at least 2 characters")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters")
            .Matches(@"^[a-zA-Z0-9\s\u00C0-\u024F\u1E00-\u1EFF]+$")
            .WithMessage("Name contains invalid characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .Matches(@"^\d{10,11}$|^\+84\d{9,10}$")
            .WithMessage("Invalid phone number format (should be 10-11 digits or +84...)");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required")
            .MinimumLength(5)
            .WithMessage("Address must be at least 5 characters")
            .MaximumLength(500)
            .WithMessage("Address must not exceed 500 characters");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Invalid email format")
            .When(x => !string.IsNullOrEmpty(x.Email));

        //PAYMENT METHOD VALIDATION
        RuleFor(x => x.PaymentMethod)
            .NotEmpty()
            .WithMessage("Payment method is required")
            .Must(IsValidPaymentMethod)
            .WithMessage("Payment method must be one of: COD, MoMo, VnPay, Paypal");

        //ORDER DETAILS VALIDATION 
        RuleFor(x => x.OrderDetails)
            .NotEmpty()
            .WithMessage("Order must contain at least one item")
            .Must(x => x.Count > 0)
            .WithMessage("Order must contain at least one item")
            .Must(x => x.Count <= 50)
            .WithMessage("Order cannot contain more than 50 items");

        RuleForEach(x => x.OrderDetails)
            .SetValidator(new CreateOrderDetailValidator(_unitOfWork));

        // COUPON VALIDATION (if provided)
        RuleFor(x => x.CouponCode)
            .MustAsync(CouponExists)
            .WithMessage("Coupon code does not exist")
            .When(x => !string.IsNullOrEmpty(x.CouponCode?.Trim()));
    }


    /// Validate user exists and is not deleted

    private async Task<bool> UserExists(string userId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(userId))
            return false;

        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        return user != null && !user.IsDeleted;
    }


    /// Validate payment method is in whitelist

    private static bool IsValidPaymentMethod(string paymentMethod)
    {
        var validMethods = new[] { "COD", "MoMo", "VnPay", "Paypal" };
        return !string.IsNullOrEmpty(paymentMethod) &&
               validMethods.Contains(paymentMethod, StringComparer.OrdinalIgnoreCase);
    }


    /// Validate coupon exists (if provided)

    private async Task<bool> CouponExists(string couponCode, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(couponCode))
            return true;  // Optional field - coupon is not required

        var coupon = await _unitOfWork.CouponRepository.GetByCodeAsync(couponCode.Trim());
        return coupon != null && !coupon.IsDeleted;
    }
}


/// Validator for CreateOrderDetailCommand

public class CreateOrderDetailValidator : AbstractValidator<CreateOrderDetailCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderDetailValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required")
            .GreaterThan(0)
            .WithMessage("Product ID must be greater than 0")
            .MustAsync(ProductExists)
            .WithMessage("Product does not exist or is deleted");

        RuleFor(x => x.Quantity)
            .NotEmpty()
            .WithMessage("Quantity is required")
            .GreaterThan(0)
            .WithMessage("Quantity must be at least 1")
            .LessThanOrEqualTo(1000)
            .WithMessage("Quantity cannot exceed 1000 items")
            .MustAsync(StockAvailable)
            .WithMessage("Insufficient stock for this product");
    }


    /// Validate product exists and is not deleted

    private async Task<bool> ProductExists(int productId, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(productId);
        return product != null && !product.IsDeleted;
    }


    /// Validate product has sufficient stock

    private async Task<bool> StockAvailable(CreateOrderDetailCommand item, int quantity, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
        if (product == null)
            return false;

        return product.Quantity >= item.Quantity;
    }
}