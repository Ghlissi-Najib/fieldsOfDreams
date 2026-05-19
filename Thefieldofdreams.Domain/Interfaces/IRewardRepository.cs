using Thefieldofdreams.Domain.Entities;

namespace Thefieldofdreams.Domain.Interfaces
{
    public interface IRewardRepository
    {
            Task<Reward> GetRewardByIdAsync(Guid rewardId);
            Task<IEnumerable<Reward>> GetAllRewardsAsync();
            Task CreateRewardAsync(Reward reward);
            Task UpdateRewardAsync(Reward reward);
            Task DeleteRewardAsync(Guid rewardId);
    }
}