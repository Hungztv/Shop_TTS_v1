using MediatR;
using ShopxBase.Application.DTOs.Slider;

namespace ShopxBase.Application.Features.Sliders.Commands.ReorderSliders;

public class ReorderSlidersCommand : IRequest<bool>
{
    public List<SliderOrderItem> Items { get; set; } = new();
}
