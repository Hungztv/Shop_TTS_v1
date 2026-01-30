using MediatR;
using ShopxBase.Application.DTOs.Slider;

namespace ShopxBase.Application.Features.Sliders.Commands.UpdateSlider;

public class UpdateSliderCommand : IRequest<SliderDto?>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Title { get; set; }
    public string? Image { get; set; }
    public string? Description { get; set; }
    public string? Link { get; set; }
    public int? DisplayOrder { get; set; }
    public int? Status { get; set; }
}
