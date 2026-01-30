namespace ShopxBase.Application.DTOs.Slider;

public class UpdateSliderDto
{
    public string? Name { get; set; }
    public string? Title { get; set; }
    public string? Image { get; set; }
    public string? Description { get; set; }
    public string? Link { get; set; }
    public int? DisplayOrder { get; set; }
    public int? Status { get; set; }
}
