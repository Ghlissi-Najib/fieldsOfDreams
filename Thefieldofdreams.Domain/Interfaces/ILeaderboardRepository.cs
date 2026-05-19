using Thefieldofdreams.Domain.Entities;

namespace Thefieldofdreams.Domain.Interfaces
{
    /// <summary>
    /// Repository contract for leaderboard entry persistence operations.
    /// </summary>
    public interface ILeaderboardRepository
    {
        Task<LeaderboardEntry?> GetByIdAsync(Guid leaderboardEntryId);
        Task<IEnumerable<LeaderboardEntry>> GetTopAsync(string? leaderboardType, int limit);
        Task CreateAsync(LeaderboardEntry entry);
        Task UpdateAsync(LeaderboardEntry entry);
        Task DeleteAsync(Guid leaderboardEntryId);
    }
}
