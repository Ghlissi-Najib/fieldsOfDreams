using System.ComponentModel.DataAnnotations;

namespace Thefieldofdreams.Application.DTOs
{
    public class UserRouteCompletionDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RouteId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsCompleted { get; set; }
        public int CompletionTimeMinutes { get; set; }
        public int LocationsVisited { get; set; }
        public int TotalLocationsInRoute { get; set; }
        public int PointsEarned { get; set; }
        public int BonusPointsEarned { get; set; }
        public bool BonusAwarded { get; set; }
        public string? RewardCode { get; set; }
        public bool IsPerfectCompletion { get; set; }
        public bool IsSpeedRun { get; set; }
        public int? RankPosition { get; set; }
        public bool IsSharedOnSocial { get; set; }
        public int? Rating { get; set; }
        public string? ReviewComment { get; set; }
        public bool EarnedBadge { get; set; }
        public string? BadgeName { get; set; }
        public int ExperiencePointsGained { get; set; }
    }

    public class CreateUserRouteCompletionRequestDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid RouteId { get; set; }

        public int TotalLocationsInRoute { get; set; }
    }

    public class UpdateUserRouteCompletionRequestDto
    {
        public bool IsCompleted { get; set; }
        public int CompletionTimeMinutes { get; set; }
        public int LocationsVisited { get; set; }
        public int PointsEarned { get; set; }
        public int BonusPointsEarned { get; set; }
        public bool BonusAwarded { get; set; }

        [StringLength(100)]
        public string? RewardCode { get; set; }

        public bool IsPerfectCompletion { get; set; }
        public bool IsSpeedRun { get; set; }
        public int? RankPosition { get; set; }
        public bool IsSharedOnSocial { get; set; }

        [Range(1, 5)]
        public int? Rating { get; set; }

        [StringLength(1000)]
        public string? ReviewComment { get; set; }

        public bool EarnedBadge { get; set; }

        [StringLength(100)]
        public string? BadgeName { get; set; }

        public int ExperiencePointsGained { get; set; }
    }
}
