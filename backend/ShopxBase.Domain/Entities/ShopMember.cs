using ShopxBase.Domain.Enums;

namespace ShopxBase.Domain.Entities;

public class ShopMember : BaseEntity
{
    public int ShopId { get; set; }
    public string UserId { get; set; }
    public ShopMemberRole Role { get; set; } = ShopMemberRole.Staff;

    public virtual Shop Shop { get; set; }
}
