using NetTopologySuite.Geometries;

namespace Thefieldofdreams.Domain.Entities
{
    public class Location : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // PostGIS geography point — SRID 4326 (WGS84/GPS)
        // X = Longitude, Y = Latitude (NTS convention)
        public Point? GeoLocation { get; set; }

        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public LocationType Type { get; set; }
        public string? ImageUrl { get; set; }
        public int PointsBonus { get; set; } = 0;
        public bool IsTourismSpot { get; set; } = false;

        // Geo-zone: radius in metres used for ST_DWithin QR validation & campaign activation
        public double ProximityRadiusMeters { get; set; } = 200;

        public virtual ICollection<QRCode> QRCodes { get; set; } = new List<QRCode>();
        public virtual ICollection<TourismRoute> TourismRoutes { get; set; } = new List<TourismRoute>();
        public virtual ICollection<CruiseSession> CruiseSessions { get; set; } = new List<CruiseSession>();
    }

    public enum LocationType
    {
        Stadium,
        TouristAttraction,
        Restaurant,
        Hotel,
        SponsorBooth,
        HiddenGem,
        TransportationHub,
        CruisePort        // Port with ship-arrival detection
    }
}
