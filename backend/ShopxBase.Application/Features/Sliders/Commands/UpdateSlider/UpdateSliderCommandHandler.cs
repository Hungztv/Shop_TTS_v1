using AutoMapper;
using MediatR;
using ShopxBase.Application.DTOs.Slider;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.Sliders.Commands.UpdateSlider;

public class UpdateSliderCommandHandler : IRequestHandler<UpdateSliderCommand, SliderDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateSliderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SliderDto?> Handle(UpdateSliderCommand request, CancellationToken cancellationToken)
    {
        var slider = await _unitOfWork.Sliders.GetByIdAsync(request.Id);
        if (slider == null || slider.IsDeleted)
            return null;

        // Partial update - only update non-null fields
        if (request.Name != null) slider.Name = request.Name;
        if (request.Title != null) slider.Title = request.Title;
        if (request.Image != null) slider.Image = request.Image;
        if (request.Description != null) slider.Description = request.Description;
        if (request.Link != null) slider.Link = request.Link;
        if (request.DisplayOrder.HasValue) slider.DisplayOrder = request.DisplayOrder.Value;
        if (request.Status.HasValue) slider.Status = request.Status.Value;

        await _unitOfWork.Sliders.UpdateAsync(slider);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<SliderDto>(slider);
    }
}
