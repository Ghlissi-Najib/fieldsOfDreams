using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Domain.Interfaces;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Repositories
{
    public class RewardRepository : IRewardRepository
    {
        private readonly ApplicationDbContext _context;

        public RewardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Reward> GetRewardByIdAsync(Guid rewardId)
        {
            return await _context.Rewards.FirstOrDefaultAsync(r => r.Id == rewardId);
        }

        public async Task<IEnumerable<Reward>> GetAllRewardsAsync()
        {
            return await _context.Rewards.ToListAsync();
        }

        public async Task CreateRewardAsync(Reward reward)
        {
            await _context.Rewards.AddAsync(reward);
        }

        public async Task UpdateRewardAsync(Reward reward)
        {
            _context.Rewards.Update(reward);
            await Task.CompletedTask;
        }

        public async Task DeleteRewardAsync(Guid rewardId)
        {
            var reward = await GetRewardByIdAsync(rewardId);
            if (reward != null)
            {
                _context.Rewards.Remove(reward);
            }
            await Task.CompletedTask;
        }
    }
}       