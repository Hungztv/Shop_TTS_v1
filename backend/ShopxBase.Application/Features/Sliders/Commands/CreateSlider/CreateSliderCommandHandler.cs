using AutoMapper;
using MediatR;
using ShopxBase.Application.DTOs.Slider;
using ShopxBase.Domain.Entities;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.Sliders.Commands.CreateSlider;

public class CreateSliderCommandHandler : IRequestHandler<CreateSliderCommand, SliderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateSliderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SliderDto> Handle(CreateSliderCommand request, CancellationToken cancellationToken)
    {
        var slider = new Slider
        {
            Name = request.Name,
            Title = request.Title,
            Image = request.Image,
            Description = request.Description ?? string.Empty,
            Link = request.Link ?? string.Empty,
            DisplayOrder = request.DisplayOrder,
            Status = request.Status
        };

        await _unitOfWork.Sliders.AddAsync(slider);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<SliderDto>(slider);
    }
}
