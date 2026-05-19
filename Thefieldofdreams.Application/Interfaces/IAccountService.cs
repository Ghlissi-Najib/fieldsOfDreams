using Thefieldofdreams.Application.DTOs;

namespace Thefieldofdreams.Application.Interfaces
{
    /// <summary>
    /// Provides account profile operations for the authenticated user.
    /// </summary>
    public interface IAccountService
    {
        Task<AccountProfileDto?> GetCurrentUserProfileAsync(string userId);
        Task<bool> UpdateCurrentUserProfileAsync(string userId, UpdateAccountRequestDto dto);
    }
}
