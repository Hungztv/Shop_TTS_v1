using MediatR;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.Sliders.Commands.ReorderSliders;

public class ReorderSlidersCommandHandler : IRequestHandler<ReorderSlidersCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public ReorderSlidersCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ReorderSlidersCommand request, CancellationToken cancellationToken)
    {
        if (request.Items == null || request.Items.Count == 0)
            return false;

        foreach (var item in request.Items)
        {
            var slider = await _unitOfWork.Sliders.GetByIdAsync(item.Id);
            if (slider != null && !slider.IsDeleted)
            {
                slider.DisplayOrder = item.NewOrder;
                await _unitOfWork.Sliders.UpdateAsync(slider);
            }
        }

        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
