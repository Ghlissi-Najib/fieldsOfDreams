using Thefieldofdreams.Application.DTOs;

namespace Thefieldofdreams.Application.Interfaces
{
    public interface IRewardService
    {
        Task<IEnumerable<RewardDto>> GetAllAsync();
        Task<RewardDto?> GetByIdAsync(Guid id);
        Task<RewardDto> CreateAsync(CreateRewardRequestDto dto, string? createdBy);
        Task<bool> UpdateAsync(Guid id, UpdateRewardRequestDto dto, string? updatedBy);
        Task<bool> DeleteAsync(Guid id);
    }
}
