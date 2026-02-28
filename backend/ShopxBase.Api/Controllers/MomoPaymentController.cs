using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopxBase.Application.Interfaces;
using ShopxBase.Infrastructure.Data;

namespace ShopxBase.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MomoPaymentController : ControllerBase
{
    private readonly IMomoPaymentService _momoService;
    private readonly ShopxBaseDbContext _dbContext;
    private readonly ILogger<MomoPaymentController> _logger;

    public MomoPaymentController(
        IMomoPaymentService momoService,
        ShopxBaseDbContext dbContext,
        ILogger<MomoPaymentController> logger)
    {
        _momoService = momoService;
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Create MoMo payment for an existing order
    /// </summary>
    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreatePayment([FromBody] CreateMomoPaymentRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { success = false, message = "Không xác định được người dùng" });

        // Find the order
        var order = await _dbContext.Orders
            .FirstOrDefaultAsync(o => o.Id == request.OrderId && o.UserId == userId);

        if (order == null)
            return NotFound(new { success = false, message = "Không tìm thấy đơn hàng" });

        if (order.PaymentStatus == "2") // Already paid
            return BadRequest(new { success = false, message = "Đơn hàng đã được thanh toán" });

        if (order.Status == 4) // Cancelled
            return BadRequest(new { success = false, message = "Đơn hàng đã bị hủy" });

        // Create MoMo payment
        var momoRequest = new MomoPaymentRequest
        {
            OrderId = order.Id.ToString(),
            OrderCode = order.OrderCode,
            Amount = (long)order.Total,
            OrderInfo = $"Thanh toán đơn hàng {order.OrderCode}"
        };

        var result = await _momoService.CreatePaymentAsync(momoRequest);

        if (!result.Success)
        {
            _logger.LogError("MoMo payment creation failed for order {OrderCode}: {Message}",
                order.OrderCode, result.Message);
            return BadRequest(new
            {
                success = false,
                message = $"Không thể tạo thanh toán MoMo: {result.Message}"
            });
        }

        // Update order payment status to Pending
        order.PaymentStatus = "1"; // Pending
        order.PaymentMethod = "MOMO";
        await _dbContext.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            message = "Tạo thanh toán MoMo thành công",
            data = new
            {
                payUrl = result.PayUrl,
                qrCodeUrl = result.QrCodeUrl,
                deeplink = result.Deeplink,
                orderCode = order.OrderCode,
                amount = order.Total
            }
        });
    }

    /// <summary>
    /// MoMo IPN (Instant Payment Notification) - MoMo server calls this
    /// </summary>
    [HttpPost("ipn")]
    [AllowAnonymous]
    public async Task<IActionResult> MomoIpn([FromBody] MomoIpnRequest request)
    {
        _logger.LogInformation("MoMo IPN received for orderId: {OrderId}, resultCode: {ResultCode}",
            request.OrderId, request.ResultCode);

        // Verify signature
        if (!_momoService.VerifySignature(request))
        {
            _logger.LogWarning("MoMo IPN signature verification failed for orderId: {OrderId}", request.OrderId);
            return BadRequest(new { success = false, message = "Invalid signature" });
        }

        // Find order by OrderCode (we used OrderCode as MoMo orderId)
        var order = await _dbContext.Orders
            .FirstOrDefaultAsync(o => o.OrderCode == request.OrderId);

        if (order == null)
        {
            _logger.LogWarning("Order not found for MoMo IPN, orderId: {OrderId}", request.OrderId);
            return NotFound(new { success = false, message = "Order not found" });
        }

        if (request.ResultCode == 0) // Payment successful
        {
            order.PaymentStatus = "2"; // Paid
            order.Status = 1; // Confirmed
            _logger.LogInformation("MoMo payment SUCCESS for order {OrderCode}, transId: {TransId}",
                order.OrderCode, request.TransId);
        }
        else // Payment failed
        {
            order.PaymentStatus = "3"; // Failed
            _logger.LogWarning("MoMo payment FAILED for order {OrderCode}, resultCode: {ResultCode}, message: {Message}",
                order.OrderCode, request.ResultCode, request.Message);
        }

        await _dbContext.SaveChangesAsync();

        // MoMo expects 204 No Content on success
        return NoContent();
    }

    /// <summary>
    /// Check payment status of an order (called from frontend after redirect)
    /// </summary>
    [HttpGet("status/{orderCode}")]
    [Authorize]
    public async Task<IActionResult> CheckPaymentStatus(string orderCode)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { success = false, message = "Không xác định được người dùng" });

        var order = await _dbContext.Orders
            .FirstOrDefaultAsync(o => o.OrderCode == orderCode && o.UserId == userId);

        if (order == null)
            return NotFound(new { success = false, message = "Không tìm thấy đơn hàng" });

        return Ok(new
        {
            success = true,
            data = new
            {
                orderCode = order.OrderCode,
                paymentStatus = order.PaymentStatus,
                paymentMethod = order.PaymentMethod,
                status = order.Status,
                total = order.Total,
                isPaid = order.PaymentStatus == "2"
            }
        });
    }
}

// ==================== Request DTO ====================

public class CreateMomoPaymentRequest
{
    public int OrderId { get; set; }
}
