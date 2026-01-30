namespace ShopxBase.Application.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true);
    Task<bool> SendContactReplyAsync(string to, string customerName, string replyMessage);
    Task<bool> SendOrderConfirmationAsync(string to, string orderNumber, decimal totalAmount);
    Task<bool> SendWelcomeEmailAsync(string to, string userName);
}
