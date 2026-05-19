using Thefieldofdreams.Application.DTOs;

namespace Thefieldofdreams.Application.Interfaces
{
    public interface IUserRouteCompletionService
    {
        Task<IEnumerable<UserRouteCompletionDto>> GetAllAsync();
        Task<IEnumerable<UserRouteCompletionDto>> GetByUserIdAsync(Guid userId);
        Task<UserRouteCompletionDto?> GetByIdAsync(Guid id);
        Task<UserRouteCompletionDto> CreateAsync(CreateUserRouteCompletionRequestDto dto, string? createdBy);
        Task<bool> UpdateAsync(Guid id, UpdateUserRouteCompletionRequestDto dto, string? updatedBy);
        Task<bool> DeleteAsync(Guid id);
    }
}
