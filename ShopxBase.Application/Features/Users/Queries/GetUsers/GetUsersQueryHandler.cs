using AutoMapper;
using MediatR;
using ShopxBase.Application.DTOs.User;
using ShopxBase.Application.DTOs.Common;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.Users.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PaginatedResult<AppUserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<AppUserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        // Build filter predicate
        var allUsers = await _unitOfWork.Users.FindAsync(user =>
            // Search term filter (FullName, Email, UserName)
            (string.IsNullOrEmpty(request.SearchTerm) ||
             user.FullName.Contains(request.SearchTerm) ||
             user.Email.Contains(request.SearchTerm) ||
             user.UserName.Contains(request.SearchTerm)) &&

            // IsActive filter
            (!request.IsActive.HasValue ||
             (request.IsActive.Value ? !user.IsDeleted : user.IsDeleted)) &&

            // RegisteredAfter filter
            (!request.RegisteredAfter.HasValue ||
             user.CreatedAt >= request.RegisteredAfter.Value) &&

            // RegisteredBefore filter
            (!request.RegisteredBefore.HasValue ||
             user.CreatedAt <= request.RegisteredBefore.Value)
        );

        var usersList = allUsers.ToList();
        var totalCount = usersList.Count;

        // Apply pagination
        var paginatedUsers = usersList
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var userDtos = _mapper.Map<List<AppUserDto>>(paginatedUsers);

        return new PaginatedResult<AppUserDto>
        {
            Items = userDtos,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}
