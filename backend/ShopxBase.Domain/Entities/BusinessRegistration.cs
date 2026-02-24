using ShopxBase.Domain.Enums;

namespace ShopxBase.Domain.Entities;

public class BusinessRegistration : BaseEntity
{
    public string UserId { get; set; }
    public string CompanyName { get; set; }
    public string TaxCode { get; set; }
    public string OwnerName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }

    public BusinessRegistrationStatus Status { get; set; } = BusinessRegistrationStatus.Pending;
    public string? RejectReason { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? ReviewedBy { get; set; }

    public virtual Shop? Shop { get; set; }
}
