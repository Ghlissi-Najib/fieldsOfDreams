using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Domain.Interfaces;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Repositories
{
    /// <summary>
    /// EF Core repository implementation for match entities.
    /// </summary>
    public class MatchRepository : IMatchRepository
    {
        private readonly ApplicationDbContext _context;

        public MatchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Match?> GetMatchByIdAsync(Guid matchId)
        {
            return await _context.Matchs.FirstOrDefaultAsync(m => m.Id == matchId);
        }

        public async Task<IEnumerable<Match>> GetAllMatchesAsync()
        {
            return await _context.Matchs.OrderBy(m => m.MatchDateTime).ToListAsync();
        }

        public async Task CreateMatchAsync(Match match)
        {
            await _context.Matchs.AddAsync(match);
        }

        public async Task UpdateMatchAsync(Match match)
        {
            _context.Matchs.Update(match);
            await Task.CompletedTask;
        }

        public async Task DeleteMatchAsync(Guid matchId)
        {
            var match = await GetMatchByIdAsync(matchId);
            if (match is not null)
            {
                _context.Matchs.Remove(match);
            }

            await Task.CompletedTask;
        }
    }
}
