using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Application.DTOs.Brand;
using ShopxBase.Application.DTOs.Common;
using ShopxBase.Domain.Entities;
using System.Linq.Expressions;
using System.Linq;

namespace ShopxBase.Application.Features.Brands.Queries.GetBrands;

public class GetBrandsQueryHandler : IRequestHandler<GetBrandsQuery, PaginatedResult<BrandDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBrandsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<BrandDto>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
    {
        // EF Core không translate được Invoke/Compile; filter in-memory sau khi lấy danh sách
        var allBrands = (await _unitOfWork.Brands.GetAllAsync()).AsEnumerable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchLower = request.SearchTerm.ToLower();
            allBrands = allBrands.Where(b =>
                (b.Name ?? string.Empty).ToLower().Contains(searchLower) ||
                (b.Description ?? string.Empty).ToLower().Contains(searchLower));
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            allBrands = allBrands.Where(b => b.Status == request.Status);
        }

        var totalCount = allBrands.Count();

        // Apply pagination
        var brands = allBrands
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var brandDtos = _mapper.Map<IEnumerable<BrandDto>>(brands);

        return new PaginatedResult<BrandDto>
        {
            Items = brandDtos.ToList(),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
