using System;
using System.Collections.Generic;
using System.Text;
using Thefieldofdreams.Application.DTOs;

namespace Thefieldofdreams.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<(bool Success, string UserId, string[] Errors)> CreateUserAsync(
            string email, string firstName, string lastName, string password, string role);
        Task<(bool Success, string[] Errors)> DeleteUserAsync(string userId);
        Task<string?> GetUserEmailAsync(string userId);
        Task<IEnumerable<string>> GetUserIdsByRoleAsync(string role);
        Task<IEnumerable<UserDto>> GetAllUsersWithRolesAsync();
    }

}
