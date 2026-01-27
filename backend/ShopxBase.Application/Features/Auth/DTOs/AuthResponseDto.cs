namespace ShopxBase.Application.Features.Auth.DTOs;

public class AuthResponseDto
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public List<string> Roles { get; set; } = new();
}
