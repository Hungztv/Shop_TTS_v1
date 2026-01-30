using MediatR;

namespace ShopxBase.Application.Features.Sliders.Commands.DeleteSlider;

public class DeleteSliderCommand : IRequest<bool>
{
    public int Id { get; set; }

    public DeleteSliderCommand(int id)
    {
        Id = id;
    }
}
