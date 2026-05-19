using Thefieldofdreams.Application.DTOs;

namespace Thefieldofdreams.Application.Interfaces
{
    public interface IUserMissionService
    {
        Task<IEnumerable<UserMissionDto>> GetAllAsync();
        Task<IEnumerable<UserMissionDto>> GetByUserIdAsync(Guid userId);
        Task<UserMissionDto?> GetByIdAsync(Guid id);
        Task<UserMissionDto> CreateAsync(CreateUserMissionRequestDto dto, string? createdBy);
        Task<bool> UpdateProgressAsync(Guid id, UpdateUserMissionProgressDto dto, string? updatedBy);
        Task<bool> DeleteAsync(Guid id);
    }
}
