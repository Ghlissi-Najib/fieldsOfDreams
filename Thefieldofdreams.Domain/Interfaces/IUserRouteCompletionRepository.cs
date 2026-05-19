using Thefieldofdreams.Domain.Entities;

namespace Thefieldofdreams.Domain.Interfaces
{
    public interface IUserRouteCompletionRepository
    {
        Task<UserRouteCompletion> GetUserRouteCompletionByIdAsync(Guid userRouteCompletionId);
        Task<IEnumerable<UserRouteCompletion>> GetUserRouteCompletionsByUserIdAsync(Guid userId);
        Task CreateUserRouteCompletionAsync(UserRouteCompletion userRouteCompletion);
        Task UpdateUserRouteCompletionAsync(UserRouteCompletion userRouteCompletion);
        Task DeleteUserRouteCompletionAsync(Guid userRouteCompletionId);
    }
}