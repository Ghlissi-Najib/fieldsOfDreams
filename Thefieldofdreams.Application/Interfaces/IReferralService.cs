using Thefieldofdreams.Application.DTOs;

namespace Thefieldofdreams.Application.Interfaces
{
    public interface IReferralService
    {
        Task<IEnumerable<ReferralDto>> GetAllAsync();
        Task<IEnumerable<ReferralDto>> GetByReferrerIdAsync(Guid referrerUserId);
        Task<ReferralDto?> GetByIdAsync(Guid id);
        Task<ReferralDto> CreateAsync(CreateReferralRequestDto dto, string? createdBy);
        Task<bool> DeleteAsync(Guid id);
    }
}
