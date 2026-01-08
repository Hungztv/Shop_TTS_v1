namespace ShopxBase.Domain.Entities;

/// <summary>
/// User entity
/// </summary>
public class User : BaseEntity
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string PasswordHash { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; }

    public User()
    {
        IsActive = true;
    }

    public User(string username, string email, string fullName, string phoneNumber) : this()
    {
        Username = username;
        Email = email;
        FullName = fullName;
        PhoneNumber = phoneNumber;
    }
}
