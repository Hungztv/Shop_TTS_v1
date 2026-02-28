using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ShopxBase.Application.Interfaces;
using ShopxBase.Application.Settings;

namespace ShopxBase.Infrastructure.Services;

public class MomoPaymentService : IMomoPaymentService
{
    private readonly MomoSettings _settings;
    private readonly HttpClient _httpClient;
    private readonly ILogger<MomoPaymentService> _logger;

    public MomoPaymentService(
        IOptions<MomoSettings> settings,
        HttpClient httpClient,
        ILogger<MomoPaymentService> logger)
    {
        _settings = settings.Value;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<MomoCreatePaymentResponse> CreatePaymentAsync(MomoPaymentRequest request)
    {
        try
        {
            var requestId = Guid.NewGuid().ToString();
            var orderId = request.OrderCode; // Use OrderCode as MoMo orderId
            var extraData = ""; // Base64 encoded extra data

            // Build raw signature string (MoMo v2 API)
            var rawSignature = $"accessKey={_settings.AccessKey}" +
                               $"&amount={request.Amount}" +
                               $"&extraData={extraData}" +
                               $"&ipnUrl={_settings.NotifyUrl}" +
                               $"&orderId={orderId}" +
                               $"&orderInfo={request.OrderInfo}" +
                               $"&partnerCode={_settings.PartnerCode}" +
                               $"&redirectUrl={_settings.ReturnUrl}" +
                               $"&requestId={requestId}" +
                               $"&requestType={_settings.RequestType}";

            _logger.LogInformation("MoMo raw signature: {RawSignature}", rawSignature);

            // HMAC SHA256 signature
            var signature = ComputeHmacSha256(rawSignature, _settings.SecretKey);

            // Build request body
            var requestBody = new
            {
                partnerCode = _settings.PartnerCode,
                partnerName = "ShopX",
                storeId = _settings.PartnerCode,
                requestId,
                amount = request.Amount,
                orderId,
                orderInfo = request.OrderInfo,
                redirectUrl = _settings.ReturnUrl,
                ipnUrl = _settings.NotifyUrl,
                lang = "vi",
                requestType = _settings.RequestType,
                autoCapture = true,
                extraData,
                signature
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);
            _logger.LogInformation("MoMo request body: {Body}", jsonContent);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_settings.ApiUrl, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("MoMo response: {Response}", responseBody);

            var momoResponse = JsonSerializer.Deserialize<JsonElement>(responseBody);

            var resultCode = momoResponse.GetProperty("resultCode").GetInt32();

            return new MomoCreatePaymentResponse
            {
                Success = resultCode == 0,
                PayUrl = momoResponse.TryGetProperty("payUrl", out var payUrl) ? payUrl.GetString() ?? "" : "",
                QrCodeUrl = momoResponse.TryGetProperty("qrCodeUrl", out var qrCode) ? qrCode.GetString() ?? "" : "",
                Deeplink = momoResponse.TryGetProperty("deeplink", out var deeplink) ? deeplink.GetString() ?? "" : "",
                Message = momoResponse.TryGetProperty("message", out var msg) ? msg.GetString() ?? "" : "",
                ResultCode = resultCode,
                RequestId = requestId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create MoMo payment for order {OrderCode}", request.OrderCode);
            return new MomoCreatePaymentResponse
            {
                Success = false,
                Message = $"Lỗi tạo thanh toán MoMo: {ex.Message}"
            };
        }
    }

    public bool VerifySignature(MomoIpnRequest request)
    {
        try
        {
            var rawSignature = $"accessKey={_settings.AccessKey}" +
                               $"&amount={request.Amount}" +
                               $"&extraData={request.ExtraData}" +
                               $"&message={request.Message}" +
                               $"&orderId={request.OrderId}" +
                               $"&orderInfo={request.OrderInfo}" +
                               $"&orderType={request.OrderType}" +
                               $"&partnerCode={request.PartnerCode}" +
                               $"&payType={request.PayType}" +
                               $"&requestId={request.RequestId}" +
                               $"&responseTime={request.ResponseTime}" +
                               $"&resultCode={request.ResultCode}" +
                               $"&transId={request.TransId}";

            var computedSignature = ComputeHmacSha256(rawSignature, _settings.SecretKey);
            var isValid = computedSignature.Equals(request.Signature, StringComparison.OrdinalIgnoreCase);

            if (!isValid)
            {
                _logger.LogWarning("MoMo IPN signature mismatch. Expected: {Expected}, Got: {Got}",
                    computedSignature, request.Signature);
            }

            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying MoMo IPN signature");
            return false;
        }
    }

    private static string ComputeHmacSha256(string data, string key)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}
