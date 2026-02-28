using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShopxBase.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatBotController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ChatBotController> _logger;

    private const string GROQ_API_URL = "https://api.groq.com/openai/v1/chat/completions";

    public ChatBotController(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<ChatBotController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Gửi tin nhắn đến AI ChatBot (GROQ API)
    /// </summary>
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            return BadRequest(new { success = false, message = "Tin nhắn không được để trống" });

        var apiKey = Environment.GetEnvironmentVariable("GROQ_API_KEY")
            ?? _configuration["Groq:ApiKey"];

        if (string.IsNullOrEmpty(apiKey))
        {
            _logger.LogError("GROQ_API_KEY is not configured");
            return StatusCode(500, new { success = false, message = "ChatBot chưa được cấu hình" });
        }

        try
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            // Xây dựng danh sách messages gửi lên GROQ
            var messages = new List<GroqMessage>
            {
                new()
                {
                    Role = "system",
                    Content = """
                        Bạn là trợ lý AI của ShopTTS - nền tảng thương mại điện tử hàng đầu Việt Nam.
                        Nhiệm vụ của bạn:
                        - Hỗ trợ khách hàng tìm kiếm sản phẩm, giải đáp thắc mắc về đơn hàng, thanh toán, vận chuyển.
                        - Trả lời ngắn gọn, thân thiện, chuyên nghiệp bằng tiếng Việt.
                        - Nếu không biết câu trả lời, hãy gợi ý liên hệ hotline hoặc email hỗ trợ.
                        - Không trả lời những câu hỏi không liên quan đến mua sắm/thương mại điện tử.
                        - Sử dụng emoji phù hợp để tạo cảm giác thân thiện.
                        
                        Thông tin hỗ trợ:
                        - Hotline: 1900-xxxx
                        - Email: support@shoptts.vn
                        - Giờ làm việc: 8:00 - 22:00 hàng ngày
                        """
                }
            };

            // Thêm lịch sử hội thoại (nếu có)
            if (request.History != null && request.History.Count > 0)
            {
                foreach (var msg in request.History.TakeLast(10)) // Giới hạn 10 tin cuối
                {
                    messages.Add(new GroqMessage
                    {
                        Role = msg.Role,
                        Content = msg.Content
                    });
                }
            }

            // Thêm tin nhắn hiện tại
            messages.Add(new GroqMessage { Role = "user", Content = request.Message });

            var groqRequest = new
            {
                model = "llama-3.3-70b-versatile",
                messages,
                temperature = 0.7,
                max_completion_tokens = 1024,
                top_p = 1,
                stream = false
            };

            var json = JsonSerializer.Serialize(groqRequest, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(GROQ_API_URL, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("GROQ API error: {StatusCode} - {Body}", response.StatusCode, responseBody);
                return StatusCode(502, new
                {
                    success = false,
                    message = "Chatbot đang gặp sự cố, vui lòng thử lại sau"
                });
            }

            var groqResponse = JsonSerializer.Deserialize<GroqResponse>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var reply = groqResponse?.Choices?.FirstOrDefault()?.Message?.Content
                ?? "Xin lỗi, tôi không thể trả lời lúc này. Vui lòng thử lại sau.";

            return Ok(new
            {
                success = true,
                data = new
                {
                    reply,
                    model = groqResponse?.Model ?? "unknown",
                    usage = groqResponse?.Usage
                }
            });
        }
        catch (TaskCanceledException)
        {
            return StatusCode(504, new { success = false, message = "Chatbot phản hồi quá lâu, vui lòng thử lại" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling GROQ API");
            return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý tin nhắn" });
        }
    }
}

// ── Request / Response Models ──

public class ChatRequest
{
    public string Message { get; set; } = string.Empty;
    public List<ChatHistoryItem>? History { get; set; }
}

public class ChatHistoryItem
{
    public string Role { get; set; } = "user";
    public string Content { get; set; } = string.Empty;
}

public class GroqMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}

public class GroqResponse
{
    public string? Id { get; set; }
    public string? Model { get; set; }
    public List<GroqChoice>? Choices { get; set; }
    public GroqUsage? Usage { get; set; }
}

public class GroqChoice
{
    public GroqMessageContent? Message { get; set; }
}

public class GroqMessageContent
{
    public string? Role { get; set; }
    public string? Content { get; set; }
}

public class GroqUsage
{
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; set; }

    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; set; }

    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
}
