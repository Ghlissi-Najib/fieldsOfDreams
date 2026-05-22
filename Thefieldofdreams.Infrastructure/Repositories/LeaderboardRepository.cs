using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Domain.Interfaces;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Repositories
{
    /// <summary>
    /// EF Core repository implementation for leaderboard entries.
    /// </summary>
    public class LeaderboardRepository : ILeaderboardRepository
    {
        private readonly ApplicationDbContext _context;

        public LeaderboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LeaderboardEntry?> GetByIdAsync(Guid leaderboardEntryId)
        {
            return await _context.LeaderboardEntries.FirstOrDefaultAsync(e => e.Id == leaderboardEntryId);
        }

        public async Task<IEnumerable<LeaderboardEntry>> GetTopAsync(string? leaderboardType, int limit)
        {
            var query = _context.LeaderboardEntries.AsQueryable();

            if (!string.IsNullOrWhiteSpace(leaderboardType))
            {
                query = query.Where(e => e.LeaderboardType == leaderboardType);
            }

            return await query
                .OrderBy(e => e.Rank)
                .ThenByDescending(e => e.TotalPoints)
                .Take(limit)
                .ToListAsync();
        }

        public async Task CreateAsync(LeaderboardEntry entry)
        {
            await _context.LeaderboardEntries.AddAsync(entry);
        }

        public async Task UpdateAsync(LeaderboardEntry entry)
        {
            _context.LeaderboardEntries.Update(entry);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid leaderboardEntryId)
        {
            var entry = await GetByIdAsync(leaderboardEntryId);
            if (entry is not null)
            {
                _context.LeaderboardEntries.Remove(entry);
            }

            await Task.CompletedTask;
        }
    }
}
