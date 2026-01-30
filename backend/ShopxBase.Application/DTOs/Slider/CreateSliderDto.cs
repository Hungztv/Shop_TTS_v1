namespace ShopxBase.Application.DTOs.Slider;

public class CreateSliderDto
{
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Link { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public int Status { get; set; } = 1;
}
