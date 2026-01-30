using MediatR;
using ShopxBase.Application.DTOs.Slider;

namespace ShopxBase.Application.Features.Sliders.Queries.GetActiveSliders;

public class GetActiveSlidersQuery : IRequest<List<SliderDto>>
{
}
