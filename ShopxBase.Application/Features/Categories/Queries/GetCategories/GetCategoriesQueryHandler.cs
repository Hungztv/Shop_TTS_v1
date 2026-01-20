using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Application.DTOs.Category;
using ShopxBase.Application.DTOs.Common;
using ShopxBase.Domain.Entities;
using System.Linq.Expressions;

namespace ShopxBase.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, PaginatedResult<CategoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        // Build filter predicate
        Expression<Func<Category, bool>> predicate = c => true;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchLower = request.SearchTerm.ToLower();
            predicate = c => c.Name.ToLower().Contains(searchLower) ||
                           c.Description.ToLower().Contains(searchLower);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            var currentPredicate = predicate;
            predicate = c => currentPredicate.Compile()(c) && c.Status == request.Status;
        }

        var allCategories = await _unitOfWork.Categories.FindAsync(predicate);
        var totalCount = allCategories.Count();

        // Apply pagination
        var categories = allCategories
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

        return new PaginatedResult<CategoryDto>
        {
            Items = categoryDtos.ToList(),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
