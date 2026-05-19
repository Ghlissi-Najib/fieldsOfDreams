using Thefieldofdreams.Application.DTOs;

namespace Thefieldofdreams.Application.Interfaces
{
    public interface IUserRewardService
    {
        Task<IEnumerable<UserRewardDto>> GetAllAsync();
        Task<IEnumerable<UserRewardDto>> GetByUserIdAsync(Guid userId);
        Task<UserRewardDto?> GetByIdAsync(Guid id);
        Task<UserRewardDto> CreateAsync(CreateUserRewardRequestDto dto, string? createdBy);
        Task<bool> UpdateAsync(Guid id, UpdateUserRewardRequestDto dto, string? updatedBy);
        Task<bool> DeleteAsync(Guid id);
    }
}
