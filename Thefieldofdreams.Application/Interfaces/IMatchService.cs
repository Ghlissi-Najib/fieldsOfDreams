using Thefieldofdreams.Application.DTOs;

namespace Thefieldofdreams.Application.Interfaces
{
    /// <summary>
    /// Provides match business operations for the API layer.
    /// </summary>
    public interface IMatchService
    {
        Task<IEnumerable<MatchDto>> GetAllAsync();
        Task<MatchDto?> GetByIdAsync(Guid id);
        Task<MatchDto> CreateAsync(CreateMatchRequestDto dto, string? createdBy);
        Task<bool> UpdateAsync(Guid id, UpdateMatchRequestDto dto, string? updatedBy);
        Task<bool> DeleteAsync(Guid id);
    }
}
