using System.ComponentModel.DataAnnotations;
using Thefieldofdreams.Domain.Entities;

namespace Thefieldofdreams.Application.DTOs
{
    public class MatchDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? HomeTeam { get; set; }
        public string? AwayTeam { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? AwayTeamScore { get; set; }
        public DateTime MatchDateTime { get; set; }
        public string? Stadium { get; set; }
        public string? Location { get; set; }
        public string? TournamentStage { get; set; }
        public MatchStatus Status { get; set; }
        public int TotalPredictions { get; set; }
        public int PointsForCorrectPrediction { get; set; }
    }

    public class CreateMatchRequestDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(100)]
        public string? HomeTeam { get; set; }

        [StringLength(100)]
        public string? AwayTeam { get; set; }

        [Required]
        public DateTime MatchDateTime { get; set; }

        [StringLength(150)]
        public string? Stadium { get; set; }

        [StringLength(150)]
        public string? Location { get; set; }

        [StringLength(100)]
        public string? TournamentStage { get; set; }

        public MatchStatus Status { get; set; } = MatchStatus.Scheduled;

        [Range(0, int.MaxValue)]
        public int PointsForCorrectPrediction { get; set; } = 100;
    }

    public class UpdateMatchRequestDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(100)]
        public string? HomeTeam { get; set; }

        [StringLength(100)]
        public string? AwayTeam { get; set; }

        public int? HomeTeamScore { get; set; }
        public int? AwayTeamScore { get; set; }

        [Required]
        public DateTime MatchDateTime { get; set; }

        [StringLength(150)]
        public string? Stadium { get; set; }

        [StringLength(150)]
        public string? Location { get; set; }

        [StringLength(100)]
        public string? TournamentStage { get; set; }

        public MatchStatus Status { get; set; } = MatchStatus.Scheduled;

        [Range(0, int.MaxValue)]
        public int TotalPredictions { get; set; }

        [Range(0, int.MaxValue)]
        public int PointsForCorrectPrediction { get; set; } = 100;
    }
}
