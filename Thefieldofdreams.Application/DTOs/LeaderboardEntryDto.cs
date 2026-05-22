using System.ComponentModel.DataAnnotations;

namespace Thefieldofdreams.Application.DTOs
{
    public class LeaderboardEntryDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? Username { get; set; }
        public int TotalPoints { get; set; }
        public int WeeklyPoints { get; set; }
        public int MonthlyPoints { get; set; }
        public int Rank { get; set; }
        public string? LeaderboardType { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class CreateLeaderboardEntryRequestDto
    {
        [Required]
        public Guid UserId { get; set; }

        [StringLength(100)]
        public string? Username { get; set; }

        [Range(0, int.MaxValue)]
        public int TotalPoints { get; set; }

        [Range(0, int.MaxValue)]
        public int WeeklyPoints { get; set; }

        [Range(0, int.MaxValue)]
        public int MonthlyPoints { get; set; }

        [Range(1, int.MaxValue)]
        public int Rank { get; set; }

        [StringLength(50)]
        public string? LeaderboardType { get; set; } = "Global";
    }

    public class UpdateLeaderboardEntryRequestDto
    {
        [StringLength(100)]
        public string? Username { get; set; }

        [Range(0, int.MaxValue)]
        public int TotalPoints { get; set; }

        [Range(0, int.MaxValue)]
        public int WeeklyPoints { get; set; }

        [Range(0, int.MaxValue)]
        public int MonthlyPoints { get; set; }

        [Range(1, int.MaxValue)]
        public int Rank { get; set; }

        [StringLength(50)]
        public string? LeaderboardType { get; set; } = "Global";
    }
}
