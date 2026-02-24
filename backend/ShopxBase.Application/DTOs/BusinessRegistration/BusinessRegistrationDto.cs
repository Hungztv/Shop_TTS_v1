namespace ShopxBase.Application.DTOs.BusinessRegistration;

public class BusinessRegistrationDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string CompanyName { get; set; }
    public string TaxCode { get; set; }
    public string OwnerName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Status { get; set; }
    public string? RejectReason { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? ReviewedBy { get; set; }
}
