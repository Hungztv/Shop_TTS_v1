using AutoMapper;
using MediatR;
using ShopxBase.Application.DTOs.Slider;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.Sliders.Queries.GetSliderById;

public class GetSliderByIdQueryHandler : IRequestHandler<GetSliderByIdQuery, SliderDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSliderByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SliderDto?> Handle(GetSliderByIdQuery request, CancellationToken cancellationToken)
    {
        var slider = await _unitOfWork.Sliders.GetByIdAsync(request.Id);
        if (slider == null || slider.IsDeleted)
            return null;

        return _mapper.Map<SliderDto>(slider);
    }
}
