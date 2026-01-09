namespace ShopxBase.Application.DTOs.User;

public class AppUserDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string FullName { get; set; }
    public string Occupation { get; set; }
    public string Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Avatar { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}

/// <summary>
/// Update User Profile DTO
/// </summary>
public class UpdateUserProfileDto
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Occupation { get; set; }
    public string Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Avatar { get; set; }
}
