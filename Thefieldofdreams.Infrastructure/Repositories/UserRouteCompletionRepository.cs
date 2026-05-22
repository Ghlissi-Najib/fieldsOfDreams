using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Domain.Interfaces;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Repositories
{
    public class UserRouteCompletionRepository : IUserRouteCompletionRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRouteCompletionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserRouteCompletion> GetUserRouteCompletionByIdAsync(Guid userRouteCompletionId)
        {
            return await _context.UserRouteCompletions.FirstOrDefaultAsync(urc => urc.Id == userRouteCompletionId);
        }

        public async Task<IEnumerable<UserRouteCompletion>> GetUserRouteCompletionsByUserIdAsync(Guid userId)
        {
            return await _context.UserRouteCompletions.Where(urc => urc.UserId == userId).ToListAsync();
        }

        public async Task CreateUserRouteCompletionAsync(UserRouteCompletion userRouteCompletion)
        {
            await _context.UserRouteCompletions.AddAsync(userRouteCompletion);
        }

        public async Task UpdateUserRouteCompletionAsync(UserRouteCompletion userRouteCompletion)
        {
            _context.UserRouteCompletions.Update(userRouteCompletion);
            await Task.CompletedTask;
        }

        public async Task DeleteUserRouteCompletionAsync(Guid userRouteCompletionId)
        {
            var userRouteCompletion = await GetUserRouteCompletionByIdAsync(userRouteCompletionId);
            if (userRouteCompletion != null)
            {
                _context.UserRouteCompletions.Remove(userRouteCompletion);
            }
            await Task.CompletedTask;
        }
    }
}