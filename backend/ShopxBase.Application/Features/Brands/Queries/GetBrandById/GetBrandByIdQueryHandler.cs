using AutoMapper;
using MediatR;
using ShopxBase.Application.DTOs.Brand;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.Brands.Queries.GetBrandById;

public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, BrandDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBrandByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BrandDto> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
    {
        var brand = await _unitOfWork.Brands.GetByIdAsync(request.Id);

        if (brand == null)
            throw new KeyNotFoundException($"Brand với ID {request.Id} không tồn tại");

        return _mapper.Map<BrandDto>(brand);
    }
}
