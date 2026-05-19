using Thefieldofdreams.Application.DTOs;

namespace Thefieldofdreams.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetAllAsync();
        Task<IEnumerable<TransactionDto>> GetByWalletIdAsync(Guid walletId);
        Task<TransactionDto?> GetByIdAsync(Guid id);
        Task<TransactionDto> CreateAsync(CreateTransactionRequestDto dto, string? createdBy);
        Task<bool> DeleteAsync(Guid id);
    }
}
