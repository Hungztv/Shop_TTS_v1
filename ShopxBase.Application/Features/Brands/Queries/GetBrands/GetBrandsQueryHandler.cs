using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Application.DTOs.Brand;
using ShopxBase.Application.DTOs.Common;
using ShopxBase.Domain.Entities;
using System.Linq.Expressions;

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
        // Build filter predicate
        Expression<Func<Brand, bool>> predicate = b => true;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchLower = request.SearchTerm.ToLower();
            predicate = b => b.Name.ToLower().Contains(searchLower) ||
                           b.Description.ToLower().Contains(searchLower);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            var currentPredicate = predicate;
            predicate = b => currentPredicate.Compile()(b) && b.Status == request.Status;
        }

        var allBrands = await _unitOfWork.Brands.FindAsync(predicate);
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
