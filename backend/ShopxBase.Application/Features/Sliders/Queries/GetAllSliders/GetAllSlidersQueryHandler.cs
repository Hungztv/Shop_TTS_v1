using AutoMapper;
using MediatR;
using ShopxBase.Application.DTOs.Slider;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.Sliders.Queries.GetAllSliders;

public class GetAllSlidersQueryHandler : IRequestHandler<GetAllSlidersQuery, List<SliderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllSlidersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<SliderDto>> Handle(GetAllSlidersQuery request, CancellationToken cancellationToken)
    {
        var sliders = await _unitOfWork.Sliders.FindAsync(s => !s.IsDeleted);
        var orderedSliders = sliders.OrderBy(s => s.DisplayOrder).ToList();
        return _mapper.Map<List<SliderDto>>(orderedSliders);
    }
}
