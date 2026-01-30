namespace ShopxBase.Application.DTOs.Slider;

public class ReorderSliderDto
{
    public List<SliderOrderItem> Items { get; set; } = new();
}

public class SliderOrderItem
{
    public int Id { get; set; }
    public int NewOrder { get; set; }
}
