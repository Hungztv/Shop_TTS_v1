namespace ShopxBase.Application.Interfaces;

public interface IMomoPaymentService
{
    /// <summary>
    /// Create MoMo payment URL for an order
    /// </summary>
    Task<MomoCreatePaymentResponse> CreatePaymentAsync(MomoPaymentRequest request);

    /// <summary>
    /// Verify IPN (Instant Payment Notification) signature from MoMo
    /// </summary>
    bool VerifySignature(MomoIpnRequest request);
}

// ==================== Request/Response Models ====================

public class MomoPaymentRequest
{
    public string OrderId { get; set; } = string.Empty;
    public string OrderCode { get; set; } = string.Empty;
    public long Amount { get; set; }
    public string OrderInfo { get; set; } = string.Empty;
}

public class MomoCreatePaymentResponse
{
    public bool Success { get; set; }
    public string PayUrl { get; set; } = string.Empty;
    public string QrCodeUrl { get; set; } = string.Empty;
    public string Deeplink { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int ResultCode { get; set; }
    public string RequestId { get; set; } = string.Empty;
}

public class MomoIpnRequest
{
    public string PartnerCode { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public string RequestId { get; set; } = string.Empty;
    public long Amount { get; set; }
    public string OrderInfo { get; set; } = string.Empty;
    public string OrderType { get; set; } = string.Empty;
    public long TransId { get; set; }
    public int ResultCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string PayType { get; set; } = string.Empty;
    public long ResponseTime { get; set; }
    public string ExtraData { get; set; } = string.Empty;
    public string Signature { get; set; } = string.Empty;
}
