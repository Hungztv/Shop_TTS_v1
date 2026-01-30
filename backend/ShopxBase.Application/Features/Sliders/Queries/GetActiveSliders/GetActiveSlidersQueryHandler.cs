using AutoMapper;
using MediatR;
using ShopxBase.Application.DTOs.Slider;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.Sliders.Queries.GetActiveSliders;

public class GetActiveSlidersQueryHandler : IRequestHandler<GetActiveSlidersQuery, List<SliderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetActiveSlidersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<SliderDto>> Handle(GetActiveSlidersQuery request, CancellationToken cancellationToken)
    {
        var sliders = await _unitOfWork.Sliders.FindAsync(s => s.Status == 1 && !s.IsDeleted);
        var orderedSliders = sliders.OrderBy(s => s.DisplayOrder).ToList();
        return _mapper.Map<List<SliderDto>>(orderedSliders);
    }
}
