namespace Thefieldofdreams.Domain.Entities
{
    public class UserRouteCompletion : BaseEntity
    {
        // Foreign keys
        public Guid UserId { get; set; }
        public Guid RouteId { get; set; }

        // Completion data
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public bool IsCompleted { get; set; } = false;
        public int CompletionTimeMinutes { get; set; } // Time taken to complete route

        // Progress tracking
        public int LocationsVisited { get; set; } = 0;
        public int TotalLocationsInRoute { get; set; }
        public string? VisitedLocationIds { get; set; } // JSON array of visited location IDs
        public string? MissedLocationIds { get; set; } // JSON array of missed location IDs

        // Rewards
        public int PointsEarned { get; set; } = 0;
        public int BonusPointsEarned { get; set; } = 0;
        public bool BonusAwarded { get; set; } = false;
        public string? RewardCode { get; set; }

        // Achievement tracking
        public bool IsPerfectCompletion { get; set; } = false; // Visited all locations
        public bool IsSpeedRun { get; set; } = false; // Completed under estimated time
        public int? RankPosition { get; set; } // Leaderboard position for this route

        // Sharing & social
        public bool IsSharedOnSocial { get; set; } = false;
        public DateTime? SharedAt { get; set; }
        public string? SharePlatform { get; set; }

        // Feedback
        public int? Rating { get; set; } // 1-5 stars
        public string? ReviewComment { get; set; }
        public string? FavoriteSpot { get; set; }

        // Gamification
        public bool EarnedBadge { get; set; } = false;
        public string? BadgeName { get; set; }
        public int ExperiencePointsGained { get; set; } = 0;

        // Navigation properties
        public virtual User? User { get; set; }
        public virtual TourismRoute? Route { get; set; }
    }
}