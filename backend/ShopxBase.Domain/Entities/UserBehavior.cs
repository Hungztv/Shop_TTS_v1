namespace ShopxBase.Domain.Entities;


public class UserBehavior : BaseEntity
{

    public string? UserId { get; set; }


    public string? SessionId { get; set; }


    public BehaviorType BehaviorType { get; set; }


    public int? ProductId { get; set; }


    public int? CategoryId { get; set; }


    public int? BrandId { get; set; }


    public string? SearchQuery { get; set; }


    public int? RatingScore { get; set; }


    public int? DwellTimeSeconds { get; set; }


    public string? SourcePage { get; set; }


    public AppUser? User { get; set; }
    public Product? Product { get; set; }
    public Category? Category { get; set; }
    public Brand? Brand { get; set; }
}


public enum BehaviorType
{

    View = 0,


    Search = 1,


    AddToCart = 2,


    Purchase = 3,


    Wishlist = 4,


    Rating = 5,


    Compare = 6
}
