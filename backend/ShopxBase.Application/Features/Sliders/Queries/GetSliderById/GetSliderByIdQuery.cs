using MediatR;
using ShopxBase.Application.DTOs.Slider;

namespace ShopxBase.Application.Features.Sliders.Queries.GetSliderById;

public class GetSliderByIdQuery : IRequest<SliderDto?>
{
    public int Id { get; set; }

    public GetSliderByIdQuery(int id)
    {
        Id = id;
    }
}
