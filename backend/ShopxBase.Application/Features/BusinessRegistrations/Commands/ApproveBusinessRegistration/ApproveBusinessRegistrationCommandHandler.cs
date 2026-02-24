using System.Text.RegularExpressions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ShopxBase.Application.DTOs.BusinessRegistration;
using ShopxBase.Application.Interfaces;
using ShopxBase.Domain.Entities;
using ShopxBase.Domain.Enums;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.BusinessRegistrations.Commands.ApproveBusinessRegistration;

public class ApproveBusinessRegistrationCommandHandler : IRequestHandler<ApproveBusinessRegistrationCommand, BusinessRegistrationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<AppUser> _userManager;

    public ApproveBusinessRegistrationCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService,
        UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _userManager = userManager;
    }

    public async Task<BusinessRegistrationDto> Handle(ApproveBusinessRegistrationCommand request, CancellationToken cancellationToken)
    {
        var registration = await _unitOfWork.BusinessRegistrations.GetByIdAsync(request.Id);
        if (registration == null)
            throw new BusinessRegistrationNotFoundException($"Đơn đăng ký với Id {request.Id} không tồn tại");

        if (registration.Status == BusinessRegistrationStatus.Approved)
        {
            await EnsureSellerRoleAsync(registration);
            return _mapper.Map<BusinessRegistrationDto>(registration);
        }

        if (registration.Status == BusinessRegistrationStatus.Rejected)
            throw new DomainException("Đơn đăng ký đã bị từ chối", "BUSINESS_REGISTRATION_STATUS_INVALID");

        var reviewerId = _currentUserService.UserId
            ?? throw UnauthorizedUserException.UserIdNotFound();

        registration.Status = BusinessRegistrationStatus.Approved;
        registration.ReviewedAt = DateTime.UtcNow;
        registration.ReviewedBy = reviewerId;

        var baseSlug = GenerateSlug(registration.CompanyName);
        var shopSlug = baseSlug;

        if (await _unitOfWork.Shops.AnyAsync(s => s.Slug == shopSlug))
        {
            shopSlug = $"{baseSlug}-{registration.Id}";
        }

        if (await _unitOfWork.Shops.AnyAsync(s => s.Slug == shopSlug))
            throw new DuplicateShopSlugException("Slug shop đã tồn tại, vui lòng cập nhật sau khi duyệt");

        var shop = new Shop
        {
            OwnerUserId = registration.UserId,
            BusinessRegistrationId = registration.Id,
            Name = registration.CompanyName,
            Slug = shopSlug,
            Status = ShopStatus.Active
        };

        await _unitOfWork.BusinessRegistrations.UpdateAsync(registration);
        await _unitOfWork.Shops.AddAsync(shop);
        await _unitOfWork.SaveChangesAsync();

        await EnsureSellerRoleAsync(registration);

        return _mapper.Map<BusinessRegistrationDto>(registration);
    }

    private static string GenerateSlug(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return "shop";

        var slug = input.Trim().ToLowerInvariant();
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", string.Empty);
        slug = Regex.Replace(slug, @"\s+", "-");
        slug = Regex.Replace(slug, @"-+", "-");
        slug = slug.Trim('-');

        return string.IsNullOrWhiteSpace(slug) ? "shop" : slug;
    }

    private async Task EnsureSellerRoleAsync(BusinessRegistration registration)
    {
        var user = await _userManager.FindByIdAsync(registration.UserId);
        if (user == null && !string.IsNullOrWhiteSpace(registration.Email))
        {
            user = await _userManager.FindByEmailAsync(registration.Email);
        }

        if (user == null)
            throw new DomainException("Không tìm thấy người dùng để gán vai trò Seller", "USER_NOT_FOUND_FOR_ROLE_ASSIGN");

        if (!await _userManager.IsInRoleAsync(user, "Seller"))
        {
            var result = await _userManager.AddToRoleAsync(user, "Seller");
            if (!result.Succeeded)
                throw new DomainException("Không thể gán vai trò Seller cho người dùng", "ROLE_ASSIGN_FAILED");
        }
    }
}
