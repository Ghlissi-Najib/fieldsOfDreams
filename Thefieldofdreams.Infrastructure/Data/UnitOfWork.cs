using System;
using System.Collections.Generic;
using Thefieldofdreams.Domain.Interfaces;

namespace Thefieldofdreams.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed;

        public IUserRepository Users { get; }
       
        public ILocationRepository Locations { get; }
        public IRewardRepository Rewards { get; }
        public IUserRouteCompletionRepository UserRouteCompletions { get; }
        public IUserRewardRepository UserRewards { get; }
        public IMatchRepository Matchs { get; }
        public ILeaderboardRepository LeaderboardEntries { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IUserRepository userRepository,
            ILocationRepository locationRepository,
            IRewardRepository rewardRepository,
            IUserRouteCompletionRepository userRouteCompletionRepository,
            IUserRewardRepository userRewardRepository,
            IMatchRepository matchRepository,
            ILeaderboardRepository leaderboardRepository)
        {
            _context = context;
            Users = userRepository;
            Locations = locationRepository;
            Rewards = rewardRepository;
            UserRouteCompletions = userRouteCompletionRepository;
            UserRewards = userRewardRepository;
            Matchs = matchRepository;
            LeaderboardEntries = leaderboardRepository;
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
