using Microsoft.AspNetCore.Identity;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Infrastructure.Identity;

namespace Thefieldofdreams.Infrastructure.Services
{
    /// <summary>
    /// Service implementation for current-user account operations.
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AccountProfileDto?> GetCurrentUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);
            return new AccountProfileDto
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                ProfilePhotoUrl = user.ProfilePhotoUrl,
                IsActive = user.IsActive,
                Role = user.Role.ToString(),
                MerchantId = user.MerchantId,
                PartnerId = user.PartnerId,
                CreatedAt = user.CreatedAt,
                Roles = roles
            };
        }

        public async Task<bool> UpdateCurrentUserProfileAsync(string userId, UpdateAccountRequestDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return false;
            }

            if (dto.FirstName is not null)
                user.FirstName = dto.FirstName.Trim();

            if (dto.LastName is not null)
                user.LastName = dto.LastName.Trim();

            if (dto.PhoneNumber is not null)
                user.PhoneNumber = string.IsNullOrWhiteSpace(dto.PhoneNumber) ? null : dto.PhoneNumber.Trim();

            if (dto.ProfilePhotoUrl is not null)
                user.ProfilePhotoUrl = string.IsNullOrWhiteSpace(dto.ProfilePhotoUrl) ? null : dto.ProfilePhotoUrl.Trim();

            user.UpdatedAt = DateTime.UtcNow;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
