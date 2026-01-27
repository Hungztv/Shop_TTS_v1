using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Application.DTOs.Brand;
using ShopxBase.Domain.Entities;

namespace ShopxBase.Application.Features.Brands.Commands.CreateBrand;

public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, BrandDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBrandCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BrandDto> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = _mapper.Map<Brand>(request);

        await _unitOfWork.Brands.AddAsync(brand);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<BrandDto>(brand);
    }
}
