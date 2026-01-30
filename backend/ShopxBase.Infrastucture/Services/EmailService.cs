using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ShopxBase.Application.Interfaces;
using ShopxBase.Application.Settings;

namespace ShopxBase.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true)
    {
        try
        {
            using var client = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
            {
                EnableSsl = _emailSettings.EnableSsl,
                Credentials = new NetworkCredential(_emailSettings.Mail, _emailSettings.Password)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.Mail, _emailSettings.DisplayName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };
            mailMessage.To.Add(to);

            await client.SendMailAsync(mailMessage);
            _logger.LogInformation("Email sent successfully to {To}", to);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}", to);
            return false;
        }
    }

    public async Task<bool> SendContactReplyAsync(string to, string customerName, string replyMessage)
    {
        var subject = "Phản hồi từ ShopX - Tin nhắn liên hệ của bạn";
        var body = GetContactReplyTemplate(customerName, replyMessage);
        return await SendEmailAsync(to, subject, body);
    }

    public async Task<bool> SendOrderConfirmationAsync(string to, string orderNumber, decimal totalAmount)
    {
        var subject = $"ShopX - Xác nhận đơn hàng #{orderNumber}";
        var body = GetOrderConfirmationTemplate(orderNumber, totalAmount);
        return await SendEmailAsync(to, subject, body);
    }

    public async Task<bool> SendWelcomeEmailAsync(string to, string userName)
    {
        var subject = "Chào mừng bạn đến với ShopX!";
        var body = GetWelcomeTemplate(userName);
        return await SendEmailAsync(to, subject, body);
    }

    private static string GetContactReplyTemplate(string customerName, string replyMessage)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: 'Segoe UI', Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ background: #f9fafb; padding: 30px; border-radius: 0 0 10px 10px; }}
        .message-box {{ background: white; padding: 20px; border-radius: 8px; border-left: 4px solid #667eea; margin: 20px 0; }}
        .footer {{ text-align: center; color: #6b7280; font-size: 12px; margin-top: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🛍️ ShopX</h1>
            <p>Phản hồi tin nhắn của bạn</p>
        </div>
        <div class='content'>
            <p>Xin chào <strong>{customerName}</strong>,</p>
            <p>Cảm ơn bạn đã liên hệ với chúng tôi. Dưới đây là phản hồi từ đội ngũ hỗ trợ:</p>
            <div class='message-box'>
                {replyMessage.Replace("\n", "<br>")}
            </div>
            <p>Nếu bạn có thêm câu hỏi, vui lòng trả lời email này hoặc liên hệ qua website.</p>
            <p>Trân trọng,<br><strong>Đội ngũ ShopX</strong></p>
        </div>
        <div class='footer'>
            <p>© 2024 ShopX. Tất cả quyền được bảo lưu.</p>
        </div>
    </div>
</body>
</html>";
    }

    private static string GetOrderConfirmationTemplate(string orderNumber, decimal totalAmount)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: 'Segoe UI', Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #10b981 0%, #059669 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ background: #f9fafb; padding: 30px; border-radius: 0 0 10px 10px; }}
        .order-box {{ background: white; padding: 20px; border-radius: 8px; text-align: center; margin: 20px 0; }}
        .order-number {{ font-size: 24px; font-weight: bold; color: #10b981; }}
        .total {{ font-size: 28px; font-weight: bold; color: #1f2937; }}
        .footer {{ text-align: center; color: #6b7280; font-size: 12px; margin-top: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>✅ Đặt hàng thành công!</h1>
            <p>Cảm ơn bạn đã mua sắm tại ShopX</p>
        </div>
        <div class='content'>
            <div class='order-box'>
                <p>Mã đơn hàng</p>
                <p class='order-number'>#{orderNumber}</p>
                <hr style='border: none; border-top: 1px solid #e5e7eb; margin: 20px 0;'>
                <p>Tổng thanh toán</p>
                <p class='total'>{totalAmount:N0}₫</p>
            </div>
            <p>Chúng tôi đã nhận được đơn hàng của bạn và đang xử lý. Bạn sẽ nhận được thông báo khi đơn hàng được giao cho đơn vị vận chuyển.</p>
            <p>Trân trọng,<br><strong>Đội ngũ ShopX</strong></p>
        </div>
        <div class='footer'>
            <p>© 2024 ShopX. Tất cả quyền được bảo lưu.</p>
        </div>
    </div>
</body>
</html>";
    }

    private static string GetWelcomeTemplate(string userName)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: 'Segoe UI', Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ background: #f9fafb; padding: 30px; border-radius: 0 0 10px 10px; }}
        .feature {{ display: flex; align-items: center; margin: 15px 0; padding: 15px; background: white; border-radius: 8px; }}
        .feature-icon {{ font-size: 24px; margin-right: 15px; }}
        .cta-button {{ display: inline-block; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 15px 30px; text-decoration: none; border-radius: 8px; font-weight: bold; margin: 20px 0; }}
        .footer {{ text-align: center; color: #6b7280; font-size: 12px; margin-top: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🎉 Chào mừng đến ShopX!</h1>
            <p>Tài khoản của bạn đã được tạo thành công</p>
        </div>
        <div class='content'>
            <p>Xin chào <strong>{userName}</strong>,</p>
            <p>Cảm ơn bạn đã đăng ký tài khoản tại ShopX. Bạn đã sẵn sàng khám phá hàng ngàn sản phẩm chất lượng!</p>
            
            <div class='feature'>
                <span class='feature-icon'>🛒</span>
                <span>Mua sắm với giá ưu đãi nhất</span>
            </div>
            <div class='feature'>
                <span class='feature-icon'>🚚</span>
                <span>Giao hàng nhanh chóng toàn quốc</span>
            </div>
            <div class='feature'>
                <span class='feature-icon'>💳</span>
                <span>Thanh toán an toàn, bảo mật</span>
            </div>
            
            <div style='text-align: center;'>
                <a href='#' class='cta-button'>Bắt đầu mua sắm</a>
            </div>
            
            <p>Trân trọng,<br><strong>Đội ngũ ShopX</strong></p>
        </div>
        <div class='footer'>
            <p>© 2024 ShopX. Tất cả quyền được bảo lưu.</p>
        </div>
    </div>
</body>
</html>";
    }
}
