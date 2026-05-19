using Thefieldofdreams.Application.DTOs;

namespace Thefieldofdreams.Application.Interfaces
{
    public interface IPredictionService
    {
        Task<IEnumerable<PredictionDto>> GetAllAsync();
        Task<IEnumerable<PredictionDto>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<PredictionDto>> GetByMatchIdAsync(Guid matchId);
        Task<PredictionDto?> GetByIdAsync(Guid id);
        Task<PredictionDto> CreateAsync(CreatePredictionRequestDto dto, Guid userId, string? createdBy);
        Task<bool> UpdateAsync(Guid id, UpdatePredictionRequestDto dto, string? updatedBy);
        Task<bool> DeleteAsync(Guid id);
    }
}
