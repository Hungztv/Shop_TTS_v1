using MediatR;
using ShopxBase.Application.DTOs.Slider;

namespace ShopxBase.Application.Features.Sliders.Queries.GetAllSliders;

public class GetAllSlidersQuery : IRequest<List<SliderDto>>
{
}
