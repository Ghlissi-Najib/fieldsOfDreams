using NetTopologySuite.Geometries;
using Location = Thefieldofdreams.Domain.Entities.Location;

namespace Thefieldofdreams.Domain.Entities
{
    // Represents one cruise ship's port-day visit.
    // Created automatically when ST_DWithin detects a ship entering the port radius.
    public class CruiseSession : BaseEntity
    {
        public required string ShipName { get; set; }
        public string? CruiseLine { get; set; }
        public string? VoyageNumber { get; set; }

        public Guid PortLocationId { get; set; }

        public DateTime ArrivalTime { get; set; }
        public DateTime? DepartureTime { get; set; }
        public int EstimatedPassengers { get; set; }

        public CruiseSessionStatus Status { get; set; } = CruiseSessionStatus.Approaching;

        // Last GPS fix from MarineTraffic (GeoJSON → PostGIS Point)
        public Point? LastKnownPosition { get; set; }
        public double? DistanceToPortMeters { get; set; }

        // True once port-day missions have been pushed to passengers
        public bool MissionsAssigned { get; set; } = false;

        public virtual Location PortLocation { get; set; } = null!;
        public virtual ICollection<LeaderboardEntry> ShipLeaderboard { get; set; } = new List<LeaderboardEntry>();
    }

    public enum CruiseSessionStatus
    {
        Approaching,
        InPort,
        Departed
    }
}
