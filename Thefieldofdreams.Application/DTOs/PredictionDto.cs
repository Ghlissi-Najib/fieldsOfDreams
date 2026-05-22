using System.ComponentModel.DataAnnotations;
using Thefieldofdreams.Domain.Entities;

namespace Thefieldofdreams.Application.DTOs
{
    public class PredictionDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid MatchId { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? AwayTeamScore { get; set; }
        public string? WinnerPrediction { get; set; }
        public int PointsEarned { get; set; }
        public PredictionStatus Status { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime PredictedAt { get; set; }
    }

    public class CreatePredictionRequestDto
    {
        [Required]
        public Guid MatchId { get; set; }

        public int? HomeTeamScore { get; set; }
        public int? AwayTeamScore { get; set; }

        [StringLength(100)]
        public string? WinnerPrediction { get; set; }
    }

    public class UpdatePredictionRequestDto
    {
        public int? HomeTeamScore { get; set; }
        public int? AwayTeamScore { get; set; }

        [StringLength(100)]
        public string? WinnerPrediction { get; set; }

        public PredictionStatus Status { get; set; }
        public bool IsCorrect { get; set; }
        public int PointsEarned { get; set; }
    }
}
