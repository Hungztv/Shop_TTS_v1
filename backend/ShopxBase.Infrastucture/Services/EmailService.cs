namespace ShopxBase.Infrastructure.Services;

using Microsoft.Extensions.Logging;

/// <summary>
/// Email service interface
/// </summary>
public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendEmailAsync(List<string> to, string subject, string body);
}

/// <summary>
/// Email service implementation
/// </summary>
public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        await SendEmailAsync(new List<string> { to }, subject, body);
    }

    public async Task SendEmailAsync(List<string> to, string subject, string body)
    {
        try
        {
            _logger.LogInformation($"Sending email to {string.Join(", ", to)} with subject: {subject}");

            // TODO: Implement email sending logic using SMTP or email service provider
            // Example: Using SmtpClient or SendGrid, etc.

            await Task.Delay(100); // Placeholder delay
            _logger.LogInformation("Email sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email");
            throw;
        }
    }
}
