using Thefieldofdreams.Application.DTOs;

namespace Thefieldofdreams.Application.Interfaces
{
    /// <summary>
    /// Provides leaderboard business operations for the API layer.
    /// </summary>
    public interface ILeaderboardService
    {
        Task<IEnumerable<LeaderboardEntryDto>> GetTopAsync(string? leaderboardType, int limit);
        Task<LeaderboardEntryDto?> GetByIdAsync(Guid id);
        Task<LeaderboardEntryDto> CreateAsync(CreateLeaderboardEntryRequestDto dto, string? createdBy);
        Task<bool> UpdateAsync(Guid id, UpdateLeaderboardEntryRequestDto dto, string? updatedBy);
        Task<bool> DeleteAsync(Guid id);
    }
}
