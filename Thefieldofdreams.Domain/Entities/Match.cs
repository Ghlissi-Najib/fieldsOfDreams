using System;
using System.Collections.Generic;
using System.Text;

namespace Thefieldofdreams.Domain.Entities
{
    

    public class Match : BaseEntity
    {
        public required string Title { get; set; }
        public string? HomeTeam { get; set; }
        public string? AwayTeam { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? AwayTeamScore { get; set; }
        public DateTime MatchDateTime { get; set; }
        public string? Stadium { get; set; }
        public string? Location { get; set; }
        public string? TournamentStage { get; set; }
        public MatchStatus Status { get; set; } = MatchStatus.Scheduled;
        public int TotalPredictions { get; set; } = 0;
        public int PointsForCorrectPrediction { get; set; } = 100;

        public virtual ICollection<Prediction> Predictions { get; set; } = new List<Prediction>();
    }

    public enum MatchStatus
    {
        Scheduled,
        Live,
        Completed,
        Cancelled
    }
}
