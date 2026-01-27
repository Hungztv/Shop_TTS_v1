using AutoMapper;
using MediatR;
using ShopxBase.Application.DTOs.Category;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(request.Id);

        if (category == null)
            throw new KeyNotFoundException($"Category với ID {request.Id} không tồn tại");

        return _mapper.Map<CategoryDto>(category);
    }
}
