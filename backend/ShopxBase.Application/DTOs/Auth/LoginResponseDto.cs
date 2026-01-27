namespace ShopxBase.Application.DTOs.Auth;

public class LoginResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string TokenType { get; set; } = "Bearer";
    public UserInfoDto User { get; set; }
}

public class UserInfoDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string? FullName { get; set; }
    public string? Avatar { get; set; }
    public string? PhoneNumber { get; set; }
    public IList<string> Roles { get; set; } = new List<string>();
}
