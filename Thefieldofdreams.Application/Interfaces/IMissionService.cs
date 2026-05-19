using Thefieldofdreams.Application.DTOs;

namespace Thefieldofdreams.Application.Interfaces
{
    public interface IMissionService
    {
        Task<IEnumerable<MissionDto>> GetAllAsync();
        Task<MissionDto?> GetByIdAsync(Guid id);
        Task<MissionDto> CreateAsync(CreateMissionRequestDto dto, string? createdBy);
        Task<bool> UpdateAsync(Guid id, UpdateMissionRequestDto dto, string? updatedBy);
        Task<bool> DeleteAsync(Guid id);
    }
}
