using Thefieldofdreams.Domain.Entities;

namespace Thefieldofdreams.Domain.Interfaces
{
    /// <summary>
    /// Repository contract for match persistence operations.
    /// </summary>
    public interface IMatchRepository
    {
        Task<Match?> GetMatchByIdAsync(Guid matchId);
        Task<IEnumerable<Match>> GetAllMatchesAsync();
        Task CreateMatchAsync(Match match);
        Task UpdateMatchAsync(Match match);
        Task DeleteMatchAsync(Guid matchId);
    }
}
