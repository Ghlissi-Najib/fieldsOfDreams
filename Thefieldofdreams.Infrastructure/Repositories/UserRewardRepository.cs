using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Domain.Interfaces;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Repositories
{
    public class UserRewardRepository : IUserRewardRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRewardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserReward> GetUserRewardByIdAsync(Guid userRewardId)
        {
            return await _context.UserRewards.FirstOrDefaultAsync(ur => ur.Id == userRewardId);
        }

        public async Task<IEnumerable<UserReward>> GetUserRewardsByUserIdAsync(Guid userId)
        {
            return await _context.UserRewards.Where(ur => ur.UserId == userId).ToListAsync();
        }

        public async Task CreateUserRewardAsync(UserReward userReward)
        {
            await _context.UserRewards.AddAsync(userReward);
        }

        public async Task UpdateUserRewardAsync(UserReward userReward)
        {
            _context.UserRewards.Update(userReward);
            await Task.CompletedTask;
        }

        public async Task DeleteUserRewardAsync(Guid userRewardId)
        {
            var userReward = await GetUserRewardByIdAsync(userRewardId);
            if (userReward != null)
            {
                _context.UserRewards.Remove(userReward);
            }
            await Task.CompletedTask;
        }
    }
}