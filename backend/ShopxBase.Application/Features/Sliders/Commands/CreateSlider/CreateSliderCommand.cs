using MediatR;
using ShopxBase.Application.DTOs.Slider;

namespace ShopxBase.Application.Features.Sliders.Commands.CreateSlider;

public class CreateSliderCommand : IRequest<SliderDto>
{
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Link { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public int Status { get; set; } = 1;
}
