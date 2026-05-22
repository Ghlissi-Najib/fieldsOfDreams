namespace Thefieldofdreams.Domain.Entities
{
    public class LeaderboardEntry : BaseEntity
    {
        public Guid UserId { get; set; }
        public string? Username { get; set; }
        public int TotalPoints { get; set; }
        public int WeeklyPoints { get; set; }
        public int MonthlyPoints { get; set; }
        public int Rank { get; set; }
        public string? LeaderboardType { get; set; } // Global, Weekly, Monthly, Ship

        // Set when LeaderboardType = "Ship" — links to a cruise port-day
        public Guid? CruiseSessionId { get; set; }
        public string? ShipName { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        public virtual User User { get; set; } = null!;
        public virtual CruiseSession? CruiseSession { get; set; }
    }
}
