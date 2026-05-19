using System;
using System.Collections.Generic;
using System.Text;

namespace Thefieldofdreams.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
      
        ILocationRepository Locations { get; }
        IRewardRepository Rewards { get; }
        IUserRouteCompletionRepository UserRouteCompletions { get; }
        IUserRewardRepository UserRewards { get; }
        IMatchRepository Matchs { get; }
        ILeaderboardRepository LeaderboardEntries { get; }

        int Complete();

    }
}
