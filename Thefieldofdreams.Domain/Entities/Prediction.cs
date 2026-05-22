using System.Text.RegularExpressions;

namespace Thefieldofdreams.Domain.Entities
{

    public class Prediction : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid MatchId { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? AwayTeamScore { get; set; }
        public string? WinnerPrediction { get; set; }
        public int PointsEarned { get; set; } = 0;
        public PredictionStatus Status { get; set; } = PredictionStatus.Pending;
        public bool IsCorrect { get; set; } = false;
        public DateTime PredictedAt { get; set; } = DateTime.UtcNow;

        public virtual User User { get; set; }
        public virtual Match Match { get; set; }
    }

    public enum PredictionStatus
    {
        Pending,
        Completed,
        Cancelled
    }
}