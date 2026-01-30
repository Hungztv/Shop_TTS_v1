using MediatR;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.Sliders.Commands.DeleteSlider;

public class DeleteSliderCommandHandler : IRequestHandler<DeleteSliderCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSliderCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteSliderCommand request, CancellationToken cancellationToken)
    {
        var slider = await _unitOfWork.Sliders.GetByIdAsync(request.Id);
        if (slider == null || slider.IsDeleted)
            return false;

        // Soft delete
        slider.IsDeleted = true;
        await _unitOfWork.Sliders.UpdateAsync(slider);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
