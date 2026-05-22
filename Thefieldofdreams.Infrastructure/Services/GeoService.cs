using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Location = Thefieldofdreams.Domain.Entities.Location;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Services
{
    // PostGIS-backed spatial service.
    // All distances are in metres; coordinates are WGS84 (GPS lat/lon).
    public class GeoService
    {
        private readonly ApplicationDbContext _db;
        private static readonly GeometryFactory _factory =
            new GeometryFactory(new PrecisionModel(), 4326);

        public GeoService(ApplicationDbContext db) => _db = db;

        // ── 1. ST_DWithin ─────────────────────────────────────────────────────
        // Returns true if the user's GPS position is within `radiusMeters` of the
        // QR code's location. Used to validate every QR scan.
        public async Task<bool> IsUserNearLocationAsync(
            double userLat, double userLon,
            Guid locationId,
            double? overrideRadiusMeters = null)
        {
            var userPoint = MakePoint(userLon, userLat);

            var location = await _db.Locations
                .Where(l => l.Id == locationId && l.GeoLocation != null)
                .Select(l => new { l.GeoLocation, l.ProximityRadiusMeters })
                .FirstOrDefaultAsync();

            if (location?.GeoLocation is null) return false;

            var radius = overrideRadiusMeters ?? location.ProximityRadiusMeters;
            return location.GeoLocation.IsWithinDistance(userPoint, radius);
        }

        // ── 2. ST_Within — geo-campaign zone activation ───────────────────────
        // Returns all active campaigns whose geo-zone polygon contains the user.
        // CampaignRule with RuleType = GeoZone stores a WKT polygon in ParametersJson:
        //   {"wkt":"POLYGON((lon lat, lon lat, ...))"}
        public async Task<List<Campaign>> GetCampaignsForUserPositionAsync(
            double userLat, double userLon)
        {
            var userPoint = MakePoint(userLon, userLat);
            var wktReader  = new NetTopologySuite.IO.WKTReader { DefaultSRID = 4326 };

            var geoRules = await _db.CampaignRules
                .Include(r => r.Campaign)
                .Where(r => r.RuleType == CampaignRuleType.GeoZone
                         && r.Campaign.Status == CampaignStatus.Active)
                .ToListAsync();

            var matched = new List<Campaign>();
            foreach (var rule in geoRules)
            {
                try
                {
                    var json = System.Text.Json.JsonDocument.Parse(rule.ParametersJson);
                    if (!json.RootElement.TryGetProperty("wkt", out var wktEl)) continue;

                    var zone = wktReader.Read(wktEl.GetString());
                    if (zone.Contains(userPoint))
                        matched.Add(rule.Campaign);
                }
                catch { /* malformed rule — skip */ }
            }
            return matched;
        }

        // ── 3. Port-day auto-trigger ──────────────────────────────────────────
        // Checks whether a ship at `shipLat/shipLon` has entered the port radius.
        // Called by the background polling service every 30 min.
        public async Task<Location?> FindPortInRangeAsync(double shipLat, double shipLon)
        {
            var shipPoint = MakePoint(shipLon, shipLat);

            return await _db.Locations
                .Where(l => l.Type == LocationType.CruisePort
                         && l.GeoLocation != null
                         && l.GeoLocation.IsWithinDistance(shipPoint, l.ProximityRadiusMeters))
                .FirstOrDefaultAsync();
        }

        // ── 4. MarineTraffic GeoJSON → PostGIS Point ──────────────────────────
        // Converts a raw MarineTraffic position response to a NTS Point
        // so it can be stored directly in CruiseSession.LastKnownPosition.
        public static Point PointFromGeoJson(double longitude, double latitude)
            => MakePoint(longitude, latitude);

        // ── 5. Nearest locations (for discovery feed) ─────────────────────────
        public async Task<List<Location>> GetNearbyLocationsAsync(
            double userLat, double userLon,
            double radiusMeters = 2000,
            int limit = 10)
        {
            var userPoint = MakePoint(userLon, userLat);

            return await _db.Locations
                .Where(l => l.GeoLocation != null
                         && l.IsActive
                         && l.GeoLocation.IsWithinDistance(userPoint, radiusMeters))
                .OrderBy(l => l.GeoLocation!.Distance(userPoint))
                .Take(limit)
                .ToListAsync();
        }

        private static Point MakePoint(double lon, double lat)
            => _factory.CreatePoint(new Coordinate(lon, lat));
    }
}
