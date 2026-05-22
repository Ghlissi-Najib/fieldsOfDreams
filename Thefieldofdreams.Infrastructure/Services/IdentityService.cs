using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Infrastructure.Identity;

namespace Thefieldofdreams.Infrastructure.Services
{
    /// <summary>
    /// Identity-backed implementation of user account management operations.
    /// </summary>
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;

        public IdentityService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<(bool Success, string UserId, string[] Errors)> CreateUserAsync(
            string email, string firstName, string lastName, string password, string role)
        {
            var user = new AppUser
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return (false, string.Empty, result.Errors.Select(e => e.Description).ToArray());
            }

            var roleResult = await _userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                return (false, string.Empty, roleResult.Errors.Select(e => e.Description).ToArray());
            }

            return (true, user.Id, []);
        }

        public async Task<(bool Success, string[] Errors)> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return (false, ["User not found."]);
            }

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded
                ? (true, [])
                : (false, result.Errors.Select(e => e.Description).ToArray());
        }

        public async Task<string?> GetUserEmailAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.Email;
        }

        public async Task<IEnumerable<string>> GetUserIdsByRoleAsync(string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);
            return users.Select(u => u.Id);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersWithRolesAsync()
        {
            var users = await _userManager.Users.OrderBy(u => u.Email).ToListAsync();
            var result = new List<UserDto>(users.Count);

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ProfilePhotoUrl = user.ProfilePhotoUrl,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    Roles = roles
                });
            }

            return result;
        }
    }
}
