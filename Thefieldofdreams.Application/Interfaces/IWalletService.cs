using Thefieldofdreams.Application.DTOs;

namespace Thefieldofdreams.Application.Interfaces
{
    public interface IWalletService
    {
        Task<WalletDto?> GetByIdAsync(Guid id);
        Task<WalletDto?> GetByUserIdAsync(Guid userId);
        Task<WalletDto> CreateForUserAsync(Guid userId, string? createdBy);
        Task<bool> DeleteAsync(Guid id);
    }
}
