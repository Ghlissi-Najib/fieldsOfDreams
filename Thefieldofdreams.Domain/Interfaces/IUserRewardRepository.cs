using Thefieldofdreams.Domain.Entities;

namespace Thefieldofdreams.Domain.Interfaces
{
    public interface IUserRewardRepository
    {
        Task<UserReward> GetUserRewardByIdAsync(Guid userRewardId);
        Task<IEnumerable<UserReward>> GetUserRewardsByUserIdAsync(Guid userId);
        Task CreateUserRewardAsync(UserReward userReward);
        Task UpdateUserRewardAsync(UserReward userReward);
        Task DeleteUserRewardAsync(Guid userRewardId);
    }
}