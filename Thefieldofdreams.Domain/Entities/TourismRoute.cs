namespace Thefieldofdreams.Domain.Entities
{
    
    public class TourismRoute : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int TotalPointsReward { get; set; }
        public int EstimatedDurationMinutes { get; set; }
        public bool IsHiddenGemRoute { get; set; } = false;

        public virtual ICollection<RouteLocation> RouteLocations { get; set; } = new List<RouteLocation>();
        public virtual ICollection<UserRouteCompletion> UserCompletions { get; set; } = new List<UserRouteCompletion>();
    }

    public class RouteLocation : BaseEntity
    {
        public Guid RouteId { get; set; }
        public Guid LocationId { get; set; }
        public int Order { get; set; }

        public virtual TourismRoute Route { get; set; }
        public virtual Location Location { get; set; }
    }
}