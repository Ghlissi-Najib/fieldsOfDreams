using System;
using System.Collections.Generic;
using System.Text;

namespace Thefieldofdreams.Domain.Entities
{
    

    public class Mission : BaseEntity
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public MissionType Type { get; set; }
        public int RequiredProgress { get; set; } = 1;
        public int PointsReward { get; set; }
        public int? XPValue { get; set; }
        public MissionDifficulty Difficulty { get; set; } = MissionDifficulty.Medium;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsDaily { get; set; } = false;
        public int? RequiredUserLevel { get; set; }

        public virtual ICollection<UserMission> UserMissions { get; set; } = new List<UserMission>();
    }

    public enum MissionType
    {
        PredictMatch,
        ScanQRCode,
        ShareReferral,
        CompleteTourismRoute,
        InviteFriend,
        DailyLogin,
        ReachLeaderboardRank,
        MakePredictionStreak
    }

    public enum MissionDifficulty
    {
        Easy,
        Medium,
        Hard,
        Expert
    }
}
