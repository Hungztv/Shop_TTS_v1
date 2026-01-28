namespace ShopxBase.Application.Interfaces;


public interface ICurrentUserService
{
    string? UserId { get; }
    string? Role { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
    bool IsAdmin { get; }
    bool IsSeller { get; }
    bool IsAdminOrSeller { get; }
}
