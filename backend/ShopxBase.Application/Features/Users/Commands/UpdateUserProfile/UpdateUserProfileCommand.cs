using MediatR;
using ShopxBase.Application.DTOs.User;

namespace ShopxBase.Application.Features.Users.Commands.UpdateUserProfile;

public class UpdateUserProfileCommand : IRequest<AppUserDto>
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Occupation { get; set; }
    public string Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Avatar { get; set; }
}
