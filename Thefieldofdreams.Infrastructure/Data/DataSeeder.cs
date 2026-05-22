using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetTopologySuite.Geometries;
using Location = Thefieldofdreams.Domain.Entities.Location;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Infrastructure.Identity;
using SecurityUserRole = Thefieldofdreams.Application.Security.UserRole;

namespace Thefieldofdreams.Infrastructure.Data
{
    public class DataSeeder
    {
        // ── Fixed GUIDs keep seeding idempotent across restarts ──────────────
        private static readonly Guid SponsorKyriakakisId  = new("11111111-0000-0000-0000-000000000001");
        private static readonly Guid SponsorLaPrimaId     = new("11111111-0000-0000-0000-000000000002");

        private static readonly Guid LocationPortId       = new("22222222-0000-0000-0000-000000000001");
        private static readonly Guid LocationKnossosId    = new("22222222-0000-0000-0000-000000000002");
        private static readonly Guid LocationKyriakakisId = new("22222222-0000-0000-0000-000000000003");
        private static readonly Guid LocationLaPrimaId    = new("22222222-0000-0000-0000-000000000004");
        private static readonly Guid LocationLionsId      = new("22222222-0000-0000-0000-000000000005");

        private static readonly Guid QRCodeKnossosId      = new("33333333-0000-0000-0000-000000000001");
        private static readonly Guid QRCodeKyriakakisId   = new("33333333-0000-0000-0000-000000000002");
        private static readonly Guid QRCodePortId         = new("33333333-0000-0000-0000-000000000003");

        private static readonly Guid CampaignSummerWelcomeId = new("44444444-0000-0000-0000-000000000001");
        private static readonly Guid CampaignPortExpId       = new("44444444-0000-0000-0000-000000000002");

        private static readonly Guid QRCampaignKyriakakisId = new("55555555-0000-0000-0000-000000000001");
        private static readonly Guid QRCampaignPortId       = new("55555555-0000-0000-0000-000000000002");

        private static readonly Guid MissionPredictId    = new("77777777-0000-0000-0000-000000000001");
        private static readonly Guid MissionScanQRId     = new("77777777-0000-0000-0000-000000000002");
        private static readonly Guid MissionReferralId   = new("77777777-0000-0000-0000-000000000003");
        private static readonly Guid MissionRouteId      = new("77777777-0000-0000-0000-000000000004");
        private static readonly Guid MissionDailyId      = new("77777777-0000-0000-0000-000000000005");
        private static readonly Guid MissionStreakId     = new("77777777-0000-0000-0000-000000000006");

        private static readonly Guid RewardCoffeeId      = new("88888888-0000-0000-0000-000000000001");
        private static readonly Guid RewardDiscountId    = new("88888888-0000-0000-0000-000000000002");
        private static readonly Guid RewardVIPId         = new("88888888-0000-0000-0000-000000000003");
        private static readonly Guid RewardMerchId       = new("88888888-0000-0000-0000-000000000004");
        private static readonly Guid RewardCashbackId    = new("88888888-0000-0000-0000-000000000005");

        private static readonly Guid RouteHeraklionId    = new("99999999-0000-0000-0000-000000000001");
        private static readonly Guid RouteHiddenGemsId   = new("99999999-0000-0000-0000-000000000002");

        private static readonly Guid LBEntry1Id          = new("AAAAAAAA-0000-0000-0000-000000000001");
        private static readonly Guid LBEntry2Id          = new("AAAAAAAA-0000-0000-0000-000000000002");
        private static readonly Guid LBEntry3Id          = new("AAAAAAAA-0000-0000-0000-000000000003");

        // ── Passenger user-specific seed data ─────────────────────────────────
        private static readonly Guid PassengerPrediction1Id  = new("CCCCCCCC-0000-0000-0000-000000000001");
        private static readonly Guid PassengerPrediction2Id  = new("CCCCCCCC-0000-0000-0000-000000000002");
        private static readonly Guid PassengerUserMission1Id = new("DDDDDDDD-0000-0000-0000-000000000001");
        private static readonly Guid PassengerUserMission2Id = new("DDDDDDDD-0000-0000-0000-000000000002");
        private static readonly Guid PassengerUserReward1Id  = new("EEEEEEEE-0000-0000-0000-000000000001");
        private static readonly Guid PassengerWalletId       = new("FFFFFFFF-0000-0000-0000-000000000001");
        private static readonly Guid PassengerTxn1Id         = new("FFFFFFFF-0000-0000-0000-000000000011");
        private static readonly Guid PassengerTxn2Id         = new("FFFFFFFF-0000-0000-0000-000000000012");
        private static readonly Guid PassengerTxn3Id         = new("FFFFFFFF-0000-0000-0000-000000000013");
        private static readonly Guid PassengerQRScan1Id      = new("CCCCCCCC-1111-0000-0000-000000000001");
        private static readonly Guid PassengerQRScan2Id      = new("CCCCCCCC-1111-0000-0000-000000000002");
        private static readonly Guid PassengerLBEntryId      = new("AAAAAAAA-0000-0000-0000-000000000004");
        private static readonly Guid PassengerReferralId     = new("CCCCCCCC-2222-0000-0000-000000000001");
        private static readonly Guid PassengerNotif1Id       = new("CCCCCCCC-3333-0000-0000-000000000001");
        private static readonly Guid PassengerNotif2Id       = new("CCCCCCCC-3333-0000-0000-000000000002");

        // ── Merchant user-specific seed data ──────────────────────────────────
        private static readonly Guid MerchantWalletId        = new("FFFFFFFF-0000-0000-0000-000000000002");
        private static readonly Guid MerchantNotif1Id        = new("CCCCCCCC-3333-0000-0000-000000000003");

        // ── Campaign orchestration seed data ──────────────────────────────────
        private static readonly Guid CampaignAutumnUpcomingId = new("44444444-0000-0000-0000-000000000003");
        private static readonly Guid CampaignRule1Id          = new("44440000-AAAA-0000-0000-000000000001");
        private static readonly Guid CampaignRule2Id          = new("44440000-AAAA-0000-0000-000000000002");
        private static readonly Guid CampaignRule3Id          = new("44440000-AAAA-0000-0000-000000000003");
        private static readonly Guid CampaignRule4Id          = new("44440000-AAAA-0000-0000-000000000004"); // GeoZone
        private static readonly Guid CampaignRule5Id          = new("44440000-AAAA-0000-0000-000000000005"); // PortArrival

        // ── Geo locations ─────────────────────────────────────────────────────
        private static readonly Guid LocationChaniaPortId     = new("22222222-0000-0000-0000-000000000006");

        // ── Cruise sessions ───────────────────────────────────────────────────
        private static readonly Guid CruiseSessionCostaDiademaId = new("66666666-0000-0000-0000-000000000001");
        private static readonly Guid CruiseSessionMSCSeaviewId   = new("66666666-0000-0000-0000-000000000002");

        // ─────────────────────────────────────────────────────────────────────

        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var userManager  = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager  = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context      = serviceProvider.GetRequiredService<ApplicationDbContext>();

            await SeedRolesAsync(roleManager);
            var userIds = await SeedUsersAsync(userManager, context);
            await SeedSponsorsAsync(context);
            await SeedLocationsAsync(context);
            await SeedQRCodesAsync(context);
            await SeedCampaignsAsync(context);
            await SeedCampaignRulesAsync(context);
            await SeedQRCampaignsAsync(context);
            await SeedWorldCup2026MatchesAsync(context);
            await SeedMissionsAsync(context);
            await SeedRewardsAsync(context);
            await SeedTourismRoutesAsync(context);
            await SeedLeaderboardAsync(context, userIds);
            await SeedCruiseSessionsAsync(context);

            if (userIds.TryGetValue("passenger@demo.com", out var passengerUserId))
                await SeedPassengerDataAsync(context, passengerUserId);

            if (userIds.TryGetValue("merchant@demo.com", out var merchantUserId))
                await SeedMerchantDataAsync(context, merchantUserId);
        }

        // ── 1. Roles ─────────────────────────────────────────────────────────

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { "Admin", "Merchant", "Partner", "Passenger" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // ── 2. Users (one per role) ───────────────────────────────────────────

        private static async Task<Dictionary<string, Guid>> SeedUsersAsync(
            UserManager<AppUser> userManager, ApplicationDbContext ctx)
        {
            await EnsureUserAsync(userManager,
                email:     "admin@thefieldofdreams.com",
                password:  "Admin@123456",
                firstName: "Admin",
                lastName:  "Admin",
                role:      SecurityUserRole.Admin,
                roleName:  "Admin");

            await EnsureUserAsync(userManager,
                email:     "merchant@demo.com",
                password:  "Merchant@123456",
                firstName: "Nikos",
                lastName:  "Kyriakakis",
                role:      SecurityUserRole.Merchant,
                roleName:  "Merchant");

            await EnsureUserAsync(userManager,
                email:     "partner@demo.com",
                password:  "Partner@123456",
                firstName: "Welcome",
                lastName:  "Ashore",
                role:      SecurityUserRole.Partner,
                roleName:  "Partner");

            await EnsureUserAsync(userManager,
                email:     "passenger@demo.com",
                password:  "Passenger@123456",
                firstName: "John",
                lastName:  "Traveler",
                role:      SecurityUserRole.Passenger,
                roleName:  "Passenger");

            // Collect Identity user IDs and ensure matching domain User records exist
            var ids = new Dictionary<string, Guid>();
            var domainUserMap = new[]
            {
                ("admin@thefieldofdreams.com",  "Admin",   "Admin",      UserRole.Admin),
                ("merchant@demo.com",            "Nikos",   "Kyriakakis", UserRole.User),
                ("partner@demo.com",             "Welcome", "Ashore",     UserRole.User),
                ("passenger@demo.com",           "John",    "Traveler",   UserRole.User),
            };

            foreach (var (email, firstName, lastName, role) in domainUserMap)
            {
                var appUser = await userManager.FindByEmailAsync(email);
                if (appUser is null || !Guid.TryParse(appUser.Id, out var guid)) continue;

                ids[email] = guid;

                // Create domain User row if it doesn't exist (same GUID as Identity)
                if (!await ctx.Users.AnyAsync(u => u.Id == guid))
                {
                    ctx.Users.Add(new User
                    {
                        Id        = guid,
                        Email     = email,
                        Username  = email,
                        FirstName = firstName,
                        LastName  = lastName,
                        Role      = role,
                        Status    = UserStatus.Active,
                        CreatedBy = "seeder"
                    });
                }
            }

            await ctx.SaveChangesAsync();
            return ids;
        }

        private static async Task EnsureUserAsync(
            UserManager<AppUser> userManager,
            string email, string password,
            string firstName, string lastName,
            SecurityUserRole role, string roleName)
        {
            if (await userManager.FindByEmailAsync(email) is not null)
                return;

            var user = new AppUser
            {
                UserName             = email,
                Email                = email,
                FirstName            = firstName,
                LastName             = lastName,
                EmailConfirmed       = true,
                IsActive             = true,
                Role                 = role,
                CreatedAt            = DateTime.UtcNow,
                TotalPoints          = 0,
                CurrentLevel         = 1,
                ExperiencePoints     = 0,
                TicketCount          = 0,
                ReferralCode         = BuildReferralCode(email),
                ReferralCount        = 0
            };

            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(user, roleName);
        }

        // ── 3. Sponsors ───────────────────────────────────────────────────────

        private static async Task SeedSponsorsAsync(ApplicationDbContext ctx)
        {
            if (await ctx.Sponsors.AnyAsync(s => s.Id == SponsorKyriakakisId))
                return;

            ctx.Sponsors.AddRange(
                new Sponsor
                {
                    Id            = SponsorKyriakakisId,
                    CompanyName   = "Kyriakakis Restaurant",
                    LogoUrl       = "https://placehold.co/200x100?text=Kyriakakis",
                    Website       = "https://kyriakakis.gr",
                    ContactEmail  = "info@kyriakakis.gr",
                    IsActive      = true,
                    Budget        = 5000m,
                    TotalCampaigns = 1,
                    CreatedBy     = "seeder"
                },
                new Sponsor
                {
                    Id            = SponsorLaPrimaId,
                    CompanyName   = "La Prima Coffee",
                    LogoUrl       = "https://placehold.co/200x100?text=LaPrima",
                    Website       = "https://laprimacoffee.gr",
                    ContactEmail  = "info@laprimacoffee.gr",
                    IsActive      = true,
                    Budget        = 3000m,
                    TotalCampaigns = 1,
                    CreatedBy     = "seeder"
                }
            );

            await ctx.SaveChangesAsync();
        }

        // ── 4. Locations ──────────────────────────────────────────────────────

        private static Point Pt(double lat, double lon)
            => new Point(lon, lat) { SRID = 4326 }; // NTS: X=lon, Y=lat

        private static async Task SeedLocationsAsync(ApplicationDbContext ctx)
        {
            if (await ctx.Locations.AnyAsync(l => l.Id == LocationPortId))
                return;

            ctx.Locations.AddRange(
                new Location
                {
                    Id                    = LocationPortId,
                    Name                  = "Heraklion Port",
                    Description           = "The main cruise port of Heraklion, Crete.",
                    Latitude              = 35.3420,
                    Longitude             = 25.1442,
                    GeoLocation           = Pt(35.3420, 25.1442),
                    Address               = "Port of Heraklion",
                    City                  = "Heraklion",
                    Country               = "Greece",
                    Type                  = LocationType.CruisePort,
                    PointsBonus           = 50,
                    ProximityRadiusMeters = 1500, // 1.5 km port detection radius
                    IsTourismSpot         = false,
                    CreatedBy             = "seeder"
                },
                new Location
                {
                    Id                    = LocationKnossosId,
                    Name                  = "Palace of Knossos",
                    Description           = "The largest Bronze Age archaeological site on Crete.",
                    Latitude              = 35.2985,
                    Longitude             = 25.1631,
                    GeoLocation           = Pt(35.2985, 25.1631),
                    Address               = "Knossos Archaeological Site",
                    City                  = "Heraklion",
                    Country               = "Greece",
                    Type                  = LocationType.TouristAttraction,
                    ImageUrl              = "https://placehold.co/400x300?text=Knossos",
                    PointsBonus           = 150,
                    ProximityRadiusMeters = 300,
                    IsTourismSpot         = true,
                    CreatedBy             = "seeder"
                },
                new Location
                {
                    Id                    = LocationKyriakakisId,
                    Name                  = "Kyriakakis Restaurant",
                    Description           = "Authentic Cretan cuisine in the heart of Heraklion.",
                    Latitude              = 35.3397,
                    Longitude             = 25.1340,
                    GeoLocation           = Pt(35.3397, 25.1340),
                    Address               = "25 Koroneou Street, Heraklion",
                    City                  = "Heraklion",
                    Country               = "Greece",
                    Type                  = LocationType.Restaurant,
                    PointsBonus           = 75,
                    ProximityRadiusMeters = 150,
                    IsTourismSpot         = false,
                    CreatedBy             = "seeder"
                },
                new Location
                {
                    Id                    = LocationLaPrimaId,
                    Name                  = "La Prima Coffee",
                    Description           = "Premium coffee experience near the Venetian harbour.",
                    Latitude              = 35.3418,
                    Longitude             = 25.1331,
                    GeoLocation           = Pt(35.3418, 25.1331),
                    Address               = "Venetian Harbour, Heraklion",
                    City                  = "Heraklion",
                    Country               = "Greece",
                    Type                  = LocationType.SponsorBooth,
                    PointsBonus           = 50,
                    ProximityRadiusMeters = 150,
                    IsTourismSpot         = false,
                    CreatedBy             = "seeder"
                },
                new Location
                {
                    Id                    = LocationLionsId,
                    Name                  = "Lions Square (Plateia Eleftherias)",
                    Description           = "The central square of Heraklion featuring the iconic Morosini Fountain.",
                    Latitude              = 35.3392,
                    Longitude             = 25.1333,
                    GeoLocation           = Pt(35.3392, 25.1333),
                    Address               = "Lions Square, Heraklion",
                    City                  = "Heraklion",
                    Country               = "Greece",
                    Type                  = LocationType.TouristAttraction,
                    ImageUrl              = "https://placehold.co/400x300?text=LionsSquare",
                    PointsBonus           = 100,
                    ProximityRadiusMeters = 200,
                    IsTourismSpot         = true,
                    CreatedBy             = "seeder"
                },
                new Location
                {
                    Id                    = LocationChaniaPortId,
                    Name                  = "Chania Port (Souda Bay)",
                    Description           = "The main cruise terminal of Chania, western Crete.",
                    Latitude              = 35.5188,
                    Longitude             = 24.0169,
                    GeoLocation           = Pt(35.5188, 24.0169),
                    Address               = "Port of Chania",
                    City                  = "Chania",
                    Country               = "Greece",
                    Type                  = LocationType.CruisePort,
                    PointsBonus           = 50,
                    ProximityRadiusMeters = 2000, // Souda Bay is wide — 2 km detection radius
                    IsTourismSpot         = false,
                    CreatedBy             = "seeder"
                }
            );

            await ctx.SaveChangesAsync();
        }

        // ── 5. QR Codes ───────────────────────────────────────────────────────

        private static async Task SeedQRCodesAsync(ApplicationDbContext ctx)
        {
            if (await ctx.QRCodes.AnyAsync(q => q.Id == QRCodeKnossosId))
                return;

            ctx.QRCodes.AddRange(
                new QRCode
                {
                    Id           = QRCodeKnossosId,
                    Code         = "QR-KNOSSOS-2026",
                    Title        = "Knossos Palace QR",
                    Description  = "Scan at the palace entrance for bonus points.",
                    Type         = QRCodeType.Tourism,
                    LocationId   = LocationKnossosId,
                    PointsReward = 150,
                    MaxScans     = 0,
                    IsLimitedTime = false,
                    CreatedBy    = "seeder"
                },
                new QRCode
                {
                    Id           = QRCodeKyriakakisId,
                    Code         = "QR-KYRIAKAKIS-2026",
                    Title        = "Kyriakakis Sponsor QR",
                    Description  = "Scan for your exclusive dining discount.",
                    Type         = QRCodeType.Sponsor,
                    SponsorId    = SponsorKyriakakisId,
                    LocationId   = LocationKyriakakisId,
                    PointsReward = 75,
                    MaxScans     = 0,
                    IsLimitedTime = false,
                    CreatedBy    = "seeder"
                },
                new QRCode
                {
                    Id           = QRCodePortId,
                    Code         = "QR-PORT-CHECKPOINT-2026",
                    Title        = "Port Welcome Checkpoint",
                    Description  = "Welcome to Heraklion! Scan here to start your adventure.",
                    Type         = QRCodeType.Checkpoint,
                    LocationId   = LocationPortId,
                    PointsReward = 50,
                    MaxScans     = 0,
                    IsLimitedTime = false,
                    CreatedBy    = "seeder"
                }
            );

            await ctx.SaveChangesAsync();
        }

        // ── 6. Campaigns ──────────────────────────────────────────────────────

        private static async Task SeedCampaignsAsync(ApplicationDbContext ctx)
        {
            if (await ctx.Campaigns.AnyAsync(c => c.Id == CampaignSummerWelcomeId))
                return;

            ctx.Campaigns.AddRange(
                new Campaign
                {
                    Id          = CampaignSummerWelcomeId,
                    Name        = "World Cup Welcome 2026",
                    Description = "Welcome cruise passengers with exclusive Kyriakakis discounts during FIFA World Cup 2026.",
                    SponsorId   = SponsorKyriakakisId,
                    Type        = CampaignType.QRBased,
                    StartDate   = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate     = new DateTime(2026, 8, 31, 0, 0, 0, DateTimeKind.Utc),
                    Budget      = 3000m,
                    Status      = CampaignStatus.Active,
                    Priority    = 10,
                    CreatedBy   = "seeder"
                },
                new Campaign
                {
                    Id          = CampaignPortExpId,
                    Name        = "World Cup Predictions 2026",
                    Description = "Predict FIFA World Cup 2026 match results and earn points redeemable at La Prima Coffee.",
                    SponsorId   = SponsorLaPrimaId,
                    Type        = CampaignType.PredictionBased,
                    StartDate   = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate     = new DateTime(2026, 7, 19, 0, 0, 0, DateTimeKind.Utc),
                    Budget      = 1500m,
                    Status      = CampaignStatus.Active,
                    Priority    = 9,
                    CreatedBy   = "seeder"
                },
                new Campaign
                {
                    Id          = CampaignAutumnUpcomingId,
                    Name        = "Heraklion Discovery Summer 2026",
                    Description = "Explore Heraklion's hidden gems — QR, tourism routes, and referrals.",
                    SponsorId   = SponsorKyriakakisId,
                    Type        = CampaignType.Hybrid,
                    StartDate   = new DateTime(2026, 5, 15, 0, 0, 0, DateTimeKind.Utc),
                    EndDate     = new DateTime(2026, 9, 30, 0, 0, 0, DateTimeKind.Utc),
                    Budget      = 2000m,
                    Status      = CampaignStatus.Active,
                    Priority    = 8,
                    CreatedBy   = "seeder"
                }
            );

            await ctx.SaveChangesAsync();
        }

        private static async Task SeedCampaignRulesAsync(ApplicationDbContext ctx)
        {
            if (await ctx.CampaignRules.AnyAsync(r => r.Id == CampaignRule1Id))
                return;

            ctx.CampaignRules.AddRange(
                new CampaignRule
                {
                    Id             = CampaignRule1Id,
                    CampaignId     = CampaignSummerWelcomeId,
                    RuleType       = CampaignRuleType.RequireRole,
                    ParametersJson = "{\"role\":\"passenger\"}",
                    CreatedBy      = "seeder"
                },
                new CampaignRule
                {
                    Id             = CampaignRule2Id,
                    CampaignId     = CampaignPortExpId,
                    RuleType       = CampaignRuleType.RequireRole,
                    ParametersJson = "{\"role\":\"passenger\"}",
                    CreatedBy      = "seeder"
                },
                new CampaignRule
                {
                    Id             = CampaignRule3Id,
                    CampaignId     = CampaignPortExpId,
                    RuleType       = CampaignRuleType.MinimumPoints,
                    ParametersJson = "{\"points\":50}",
                    CreatedBy      = "seeder"
                },
                // GeoZone rule: campaign activates only inside Heraklion port polygon (ST_Within)
                new CampaignRule
                {
                    Id             = CampaignRule4Id,
                    CampaignId     = CampaignSummerWelcomeId,
                    RuleType       = CampaignRuleType.GeoZone,
                    ParametersJson = "{\"wkt\":\"POLYGON((25.130 35.330, 25.160 35.330, 25.160 35.355, 25.130 35.355, 25.130 35.330))\"}",
                    CreatedBy      = "seeder"
                },
                // PortArrival rule: campaign auto-triggers when a ship enters Heraklion port (ST_DWithin)
                new CampaignRule
                {
                    Id             = CampaignRule5Id,
                    CampaignId     = CampaignAutumnUpcomingId,
                    RuleType       = CampaignRuleType.PortArrival,
                    ParametersJson = $"{{\"portLocationId\":\"{LocationPortId}\"}}",
                    CreatedBy      = "seeder"
                }
            );

            await ctx.SaveChangesAsync();
        }

        // ── 7. QR Campaigns ───────────────────────────────────────────────────

        private static async Task SeedQRCampaignsAsync(ApplicationDbContext ctx)
        {
            if (await ctx.QRCampaigns.AnyAsync(q => q.Id == QRCampaignKyriakakisId))
                return;

            ctx.QRCampaigns.AddRange(
                new QRCampaign
                {
                    Id                    = QRCampaignKyriakakisId,
                    CampaignName          = "Kyriakakis Dining Discount",
                    Description           = "Get 15% off your meal at Kyriakakis Restaurant.",
                    CampaignId            = CampaignSummerWelcomeId,
                    QRCodeId              = QRCodeKyriakakisId,
                    MaxScansPerUser       = 1,
                    TotalScanLimit        = 500,
                    BasePointsReward      = 75,
                    BonusPointsForFirstScan = 25,
                    RewardCode            = "KYRIA15",
                    DiscountPercentage    = "15%",
                    StartDate             = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate               = new DateTime(2026, 8, 31, 0, 0, 0, DateTimeKind.Utc),
                    IsActive              = true,
                    TargetUserGroup       = "All",
                    SuccessMessage        = "Enjoy 15% off at Kyriakakis! Show this screen to your waiter.",
                    SponsorOfferCode      = "KYRIA-SPONSOR-2026",
                    IsSponsorExclusive    = false,
                    CreatedBy             = "seeder"
                },
                new QRCampaign
                {
                    Id                    = QRCampaignPortId,
                    CampaignName          = "Port Arrival Coffee",
                    Description           = "Claim a free coffee at La Prima when you arrive.",
                    CampaignId            = CampaignPortExpId,
                    QRCodeId              = QRCodePortId,
                    MaxScansPerUser       = 1,
                    TotalScanLimit        = 1000,
                    BasePointsReward      = 50,
                    BonusPointsForFirstScan = 10,
                    RewardCode            = "LAPRIMAFREE",
                    StartDate             = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate               = new DateTime(2026, 9, 30, 0, 0, 0, DateTimeKind.Utc),
                    IsActive              = true,
                    TargetUserGroup       = "All",
                    SuccessMessage        = "Enjoy a free coffee at La Prima! Show this at the counter.",
                    SponsorOfferCode      = "LAPRIMACOFFEE-2026",
                    IsSponsorExclusive    = false,
                    CreatedBy             = "seeder"
                }
            );

            await ctx.SaveChangesAsync();
        }

        // ── 8. FIFA World Cup 2026 Matches ───────────────────────────────────

        private static async Task SeedWorldCup2026MatchesAsync(ApplicationDbContext ctx)
        {
            if (await ctx.Matchs.AnyAsync(m => m.Id == WcGuid(1)))
                return;

            Match M(int n, string home, string away, string stage, DateTime dt, string stad, string loc, int pts = 150)
                => new()
                {
                    Id = WcGuid(n), Title = $"{home} vs {away}", HomeTeam = home, AwayTeam = away,
                    MatchDateTime = dt, Stadium = stad, Location = loc, TournamentStage = stage,
                    Status = MatchStatus.Scheduled, PointsForCorrectPrediction = pts, CreatedBy = "seeder"
                };

            DateTime U(int mo, int d, int h, int mi = 0) => new(2026, mo, d, h, mi, 0, DateTimeKind.Utc);

            ctx.Matchs.AddRange(

                // ══ GROUP STAGE – chronological order (all times UTC, ET+4) ══

                // Thursday, June 11
                M(1,  "🇲🇽 Mexico",               "🇿🇦 South Africa",         "Group A – Matchday 1", U(6,11,19),    "Estadio Azteca",          "Mexico City, Mexico",      250),
                M(2,  "🇰🇷 Korea Republic",        "🇨🇿 Czechia",              "Group A – Matchday 1", U(6,12,2),     "Estadio Akron",           "Guadalajara, Mexico",      150),

                // Friday, June 12
                M(3,  "🇨🇦 Canada",               "🇧🇦 Bosnia-Herzegovina",   "Group B – Matchday 1", U(6,12,19),    "BMO Field",               "Toronto, Canada",          200),
                M(4,  "🇺🇸 United States",         "🇵🇾 Paraguay",             "Group D – Matchday 1", U(6,13,1),     "SoFi Stadium",            "Los Angeles, USA",         200),

                // Saturday, June 13
                M(5,  "🇦🇺 Australia",             "🇹🇷 Türkiye",              "Group D – Matchday 1", U(6,13,4),     "BC Place",                "Vancouver, Canada",        150),
                M(6,  "🇶🇦 Qatar",                 "🇨🇭 Switzerland",          "Group B – Matchday 1", U(6,13,19),    "Levi's Stadium",          "San Francisco, USA",       150),
                M(7,  "🇧🇷 Brazil",                "🇲🇦 Morocco",              "Group C – Matchday 1", U(6,13,22),    "MetLife Stadium",         "New York/New Jersey, USA", 250),
                M(8,  "🇭🇹 Haiti",                 "🏴󠁧󠁢󠁳󠁣󠁴󠁿 Scotland",           "Group C – Matchday 1", U(6,14,1),     "Gillette Stadium",        "Boston, USA",              150),

                // Sunday, June 14
                M(9,  "🇩🇪 Germany",               "🇨🇼 Curaçao",              "Group E – Matchday 1", U(6,14,17),    "NRG Stadium",             "Houston, USA",             200),
                M(10, "🇳🇱 Netherlands",            "🇯🇵 Japan",                "Group F – Matchday 1", U(6,14,20),    "AT&T Stadium",            "Dallas, USA",              200),
                M(11, "🇨🇮 Côte d'Ivoire",          "🇪🇨 Ecuador",              "Group E – Matchday 1", U(6,14,23),    "Lincoln Financial Field", "Philadelphia, USA",        150),
                M(12, "🇸🇪 Sweden",                "🇹🇳 Tunisia",              "Group F – Matchday 1", U(6,15,2),     "Estadio BBVA",            "Monterrey, Mexico",        150),

                // Monday, June 15
                M(13, "🇪🇸 Spain",                 "🇨🇻 Cabo Verde",           "Group H – Matchday 1", U(6,15,16),    "Mercedes-Benz Stadium",   "Atlanta, USA",             200),
                M(14, "🇧🇪 Belgium",               "🇪🇬 Egypt",                "Group G – Matchday 1", U(6,15,19),    "Lumen Field",             "Seattle, USA",             200),
                M(15, "🇸🇦 Saudi Arabia",           "🇺🇾 Uruguay",              "Group H – Matchday 1", U(6,15,22),    "Hard Rock Stadium",       "Miami, USA",               200),
                M(16, "🇮🇷 IR Iran",               "🇳🇿 New Zealand",          "Group G – Matchday 1", U(6,16,1),     "SoFi Stadium",            "Los Angeles, USA",         150),

                // Tuesday, June 16
                M(17, "🇦🇹 Austria",               "🇯🇴 Jordan",               "Group J – Matchday 1", U(6,16,4),     "Levi's Stadium",          "San Francisco, USA",       150),
                M(18, "🇫🇷 France",                "🇸🇳 Senegal",              "Group I – Matchday 1", U(6,16,19),    "MetLife Stadium",         "New York/New Jersey, USA", 250),
                M(19, "🇮🇶 Iraq",                  "🇳🇴 Norway",               "Group I – Matchday 1", U(6,16,22),    "Gillette Stadium",        "Boston, USA",              150),
                M(20, "🇦🇷 Argentina",             "🇩🇿 Algeria",              "Group J – Matchday 1", U(6,17,1),     "Arrowhead Stadium",       "Kansas City, USA",         250),

                // Wednesday, June 17
                M(21, "🇵🇹 Portugal",              "🇨🇩 Congo DR",             "Group K – Matchday 1", U(6,17,17),    "NRG Stadium",             "Houston, USA",             250),
                M(22, "🏴󠁧󠁢󠁥󠁮󠁧󠁿 England",              "🇭🇷 Croatia",              "Group L – Matchday 1", U(6,17,20),    "AT&T Stadium",            "Dallas, USA",              200),
                M(23, "🇬🇭 Ghana",                 "🇵🇦 Panama",               "Group L – Matchday 1", U(6,17,23),    "BMO Field",               "Toronto, Canada",          150),
                M(24, "🇺🇿 Uzbekistan",            "🇨🇴 Colombia",             "Group K – Matchday 1", U(6,18,2),     "Estadio Azteca",          "Mexico City, Mexico",      150),

                // Thursday, June 18
                M(25, "🇨🇿 Czechia",               "🇿🇦 South Africa",         "Group A – Matchday 2", U(6,18,16),    "Mercedes-Benz Stadium",   "Atlanta, USA",             150),
                M(26, "🇨🇭 Switzerland",            "🇧🇦 Bosnia-Herzegovina",   "Group B – Matchday 2", U(6,18,19),    "SoFi Stadium",            "Los Angeles, USA",         150),
                M(27, "🇨🇦 Canada",                "🇶🇦 Qatar",                "Group B – Matchday 2", U(6,18,22),    "BC Place",                "Vancouver, Canada",        150),
                M(28, "🇲🇽 Mexico",                "🇰🇷 Korea Republic",        "Group A – Matchday 2", U(6,19,1),     "Estadio Akron",           "Guadalajara, Mexico",      200),

                // Friday, June 19
                M(29, "🇹🇷 Türkiye",               "🇵🇾 Paraguay",             "Group D – Matchday 2", U(6,19,4),     "Levi's Stadium",          "San Francisco, USA",       150),
                M(30, "🇺🇸 United States",         "🇦🇺 Australia",             "Group D – Matchday 2", U(6,19,19),    "Lumen Field",             "Seattle, USA",             200),
                M(31, "🏴󠁧󠁢󠁳󠁣󠁴󠁿 Scotland",             "🇲🇦 Morocco",              "Group C – Matchday 2", U(6,19,22),    "Gillette Stadium",        "Boston, USA",              150),
                M(32, "🇧🇷 Brazil",                "🇭🇹 Haiti",                "Group C – Matchday 2", U(6,20,0,30),  "Lincoln Financial Field", "Philadelphia, USA",        200),

                // Saturday, June 20
                M(33, "🇹🇳 Tunisia",               "🇯🇵 Japan",                "Group F – Matchday 2", U(6,20,4),     "Estadio BBVA",            "Monterrey, Mexico",        150),
                M(34, "🇳🇱 Netherlands",            "🇸🇪 Sweden",               "Group F – Matchday 2", U(6,20,17),    "NRG Stadium",             "Houston, USA",             200),
                M(35, "🇩🇪 Germany",               "🇨🇮 Côte d'Ivoire",         "Group E – Matchday 2", U(6,20,20),    "BMO Field",               "Toronto, Canada",          200),
                M(36, "🇪🇨 Ecuador",               "🇨🇼 Curaçao",              "Group E – Matchday 2", U(6,21,0),     "Arrowhead Stadium",       "Kansas City, USA",         150),

                // Sunday, June 21
                M(37, "🇪🇸 Spain",                 "🇸🇦 Saudi Arabia",         "Group H – Matchday 2", U(6,21,16),    "Mercedes-Benz Stadium",   "Atlanta, USA",             200),
                M(38, "🇧🇪 Belgium",               "🇮🇷 IR Iran",              "Group G – Matchday 2", U(6,21,19),    "SoFi Stadium",            "Los Angeles, USA",         200),
                M(39, "🇺🇾 Uruguay",               "🇨🇻 Cabo Verde",           "Group H – Matchday 2", U(6,21,22),    "Hard Rock Stadium",       "Miami, USA",               150),
                M(40, "🇳🇿 New Zealand",            "🇪🇬 Egypt",                "Group G – Matchday 2", U(6,22,1),     "BC Place",                "Vancouver, Canada",        150),

                // Monday, June 22
                M(41, "🇦🇷 Argentina",             "🇦🇹 Austria",              "Group J – Matchday 2", U(6,22,17),    "AT&T Stadium",            "Dallas, USA",              200),
                M(42, "🇫🇷 France",                "🇮🇶 Iraq",                 "Group I – Matchday 2", U(6,22,21),    "Lincoln Financial Field", "Philadelphia, USA",        200),
                M(43, "🇳🇴 Norway",                "🇸🇳 Senegal",              "Group I – Matchday 2", U(6,23,0),     "MetLife Stadium",         "New York/New Jersey, USA", 150),
                M(44, "🇯🇴 Jordan",                "🇩🇿 Algeria",              "Group J – Matchday 2", U(6,23,3),     "Levi's Stadium",          "San Francisco, USA",       150),

                // Tuesday, June 23
                M(45, "🇵🇹 Portugal",              "🇺🇿 Uzbekistan",           "Group K – Matchday 2", U(6,23,17),    "NRG Stadium",             "Houston, USA",             200),
                M(46, "🏴󠁧󠁢󠁥󠁮󠁧󠁿 England",              "🇬🇭 Ghana",                "Group L – Matchday 2", U(6,23,20),    "Gillette Stadium",        "Boston, USA",              200),
                M(47, "🇵🇦 Panama",                "🇭🇷 Croatia",              "Group L – Matchday 2", U(6,23,23),    "BMO Field",               "Toronto, Canada",          150),
                M(48, "🇨🇴 Colombia",              "🇨🇩 Congo DR",             "Group K – Matchday 2", U(6,24,2),     "Estadio Akron",           "Guadalajara, Mexico",      150),

                // Wednesday, June 24 – Matchday 3 (simultaneous within groups)
                M(49, "🇨🇭 Switzerland",            "🇨🇦 Canada",               "Group B – Matchday 3", U(6,24,19),    "BC Place",                "Vancouver, Canada",        200),
                M(50, "🇧🇦 Bosnia-Herzegovina",     "🇶🇦 Qatar",                "Group B – Matchday 3", U(6,24,19),    "Lumen Field",             "Seattle, USA",             150),
                M(51, "🏴󠁧󠁢󠁳󠁣󠁴󠁿 Scotland",             "🇧🇷 Brazil",               "Group C – Matchday 3", U(6,24,22),    "Hard Rock Stadium",       "Miami, USA",               250),
                M(52, "🇲🇦 Morocco",               "🇭🇹 Haiti",                "Group C – Matchday 3", U(6,24,22),    "Mercedes-Benz Stadium",   "Atlanta, USA",             150),
                M(53, "🇨🇿 Czechia",               "🇲🇽 Mexico",               "Group A – Matchday 3", U(6,25,1),     "Estadio Azteca",          "Mexico City, Mexico",      200),
                M(54, "🇿🇦 South Africa",           "🇰🇷 Korea Republic",        "Group A – Matchday 3", U(6,25,1),     "Estadio BBVA",            "Monterrey, Mexico",        200),

                // Thursday, June 25
                M(55, "🇨🇼 Curaçao",               "🇨🇮 Côte d'Ivoire",         "Group E – Matchday 3", U(6,25,20),    "Lincoln Financial Field", "Philadelphia, USA",        150),
                M(56, "🇪🇨 Ecuador",               "🇩🇪 Germany",              "Group E – Matchday 3", U(6,25,20),    "MetLife Stadium",         "New York/New Jersey, USA", 200),
                M(57, "🇯🇵 Japan",                 "🇸🇪 Sweden",               "Group F – Matchday 3", U(6,25,23),    "AT&T Stadium",            "Dallas, USA",              200),
                M(58, "🇹🇳 Tunisia",               "🇳🇱 Netherlands",          "Group F – Matchday 3", U(6,25,23),    "Arrowhead Stadium",       "Kansas City, USA",         150),
                M(59, "🇵🇾 Paraguay",              "🇦🇺 Australia",             "Group D – Matchday 3", U(6,26,2),     "Levi's Stadium",          "San Francisco, USA",       150),
                M(60, "🇹🇷 Türkiye",               "🇺🇸 United States",         "Group D – Matchday 3", U(6,26,2),     "SoFi Stadium",            "Los Angeles, USA",         200),

                // Friday, June 26
                M(61, "🇳🇴 Norway",                "🇫🇷 France",               "Group I – Matchday 3", U(6,26,19),    "Gillette Stadium",        "Boston, USA",              200),
                M(62, "🇸🇳 Senegal",               "🇮🇶 Iraq",                 "Group I – Matchday 3", U(6,26,19),    "BMO Field",               "Toronto, Canada",          150),
                M(63, "🇺🇾 Uruguay",               "🇪🇸 Spain",                "Group H – Matchday 3", U(6,27,0),     "NRG Stadium",             "Houston, USA",             250),
                M(64, "🇨🇻 Cabo Verde",             "🇸🇦 Saudi Arabia",         "Group H – Matchday 3", U(6,27,0),     "Estadio Akron",           "Guadalajara, Mexico",      150),
                M(65, "🇪🇬 Egypt",                 "🇮🇷 IR Iran",              "Group G – Matchday 3", U(6,27,3),     "Lumen Field",             "Seattle, USA",             150),
                M(66, "🇳🇿 New Zealand",            "🇧🇪 Belgium",              "Group G – Matchday 3", U(6,27,3),     "BC Place",                "Vancouver, Canada",        200),

                // Saturday, June 27
                M(67, "🇵🇦 Panama",                "🏴󠁧󠁢󠁥󠁮󠁧󠁿 England",            "Group L – Matchday 3", U(6,27,21),    "MetLife Stadium",         "New York/New Jersey, USA", 200),
                M(68, "🇭🇷 Croatia",               "🇬🇭 Ghana",                "Group L – Matchday 3", U(6,27,21),    "Lincoln Financial Field", "Philadelphia, USA",        150),
                M(69, "🇨🇴 Colombia",              "🇵🇹 Portugal",             "Group K – Matchday 3", U(6,27,23,30), "Hard Rock Stadium",       "Miami, USA",               250),
                M(70, "🇨🇩 Congo DR",              "🇺🇿 Uzbekistan",           "Group K – Matchday 3", U(6,27,23,30), "Mercedes-Benz Stadium",   "Atlanta, USA",             150),
                M(71, "🇯🇴 Jordan",                "🇦🇷 Argentina",            "Group J – Matchday 3", U(6,28,2),     "AT&T Stadium",            "Dallas, USA",              200),
                M(72, "🇩🇿 Algeria",               "🇦🇹 Austria",              "Group J – Matchday 3", U(6,28,2),     "Arrowhead Stadium",       "Kansas City, USA",         150),

                // ══ ROUND OF 32 ══════════════════════════════════════════════

                // Sunday, June 28
                M(73,  "TBD", "TBD", "Round of 32", U(6,28,20),  "SoFi Stadium",          "Los Angeles, USA",         300),
                // Monday, June 29
                M(74,  "TBD", "TBD", "Round of 32", U(6,29,16),  "Gillette Stadium",       "Boston, USA",              300),
                M(75,  "TBD", "TBD", "Round of 32", U(6,29,21),  "Estadio BBVA",           "Monterrey, Mexico",        300),
                M(76,  "TBD", "TBD", "Round of 32", U(6,30,0),   "NRG Stadium",            "Houston, USA",             300),
                // Tuesday, June 30
                M(77,  "TBD", "TBD", "Round of 32", U(6,30,16),  "MetLife Stadium",        "New York/New Jersey, USA", 300),
                M(78,  "TBD", "TBD", "Round of 32", U(6,30,21),  "AT&T Stadium",           "Dallas, USA",              300),
                M(79,  "TBD", "TBD", "Round of 32", U(7,1,1),    "Estadio Azteca",         "Mexico City, Mexico",      300),
                // Wednesday, July 1
                M(80,  "TBD", "TBD", "Round of 32", U(7,1,16),   "Mercedes-Benz Stadium",  "Atlanta, USA",             300),
                M(81,  "TBD", "TBD", "Round of 32", U(7,1,21),   "Levi's Stadium",         "San Francisco, USA",       300),
                M(82,  "TBD", "TBD", "Round of 32", U(7,2,1),    "Lumen Field",            "Seattle, USA",             300),
                // Thursday, July 2
                M(83,  "TBD", "TBD", "Round of 32", U(7,2,16),   "BMO Field",              "Toronto, Canada",          300),
                M(84,  "TBD", "TBD", "Round of 32", U(7,2,21),   "SoFi Stadium",           "Los Angeles, USA",         300),
                M(85,  "TBD", "TBD", "Round of 32", U(7,3,1),    "BC Place",               "Vancouver, Canada",        300),
                // Friday, July 3
                M(86,  "TBD", "TBD", "Round of 32", U(7,3,19),   "Hard Rock Stadium",      "Miami, USA",               300),
                M(87,  "TBD", "TBD", "Round of 32", U(7,3,22),   "Arrowhead Stadium",      "Kansas City, USA",         300),
                M(88,  "TBD", "TBD", "Round of 32", U(7,4,1),    "AT&T Stadium",           "Dallas, USA",              300),

                // ══ ROUND OF 16 ══════════════════════════════════════════════

                // Saturday, July 4
                M(89,  "TBD", "TBD", "Round of 16", U(7,4,16),   "Lincoln Financial Field","Philadelphia, USA",        400),
                M(90,  "TBD", "TBD", "Round of 16", U(7,4,20),   "NRG Stadium",            "Houston, USA",             400),
                // Sunday, July 5
                M(91,  "TBD", "TBD", "Round of 16", U(7,5,16),   "MetLife Stadium",        "New York/New Jersey, USA", 400),
                M(92,  "TBD", "TBD", "Round of 16", U(7,5,20),   "Estadio Azteca",         "Mexico City, Mexico",      400),
                // Monday, July 6
                M(93,  "TBD", "TBD", "Round of 16", U(7,6,16),   "AT&T Stadium",           "Dallas, USA",              400),
                M(94,  "TBD", "TBD", "Round of 16", U(7,6,20),   "Lumen Field",            "Seattle, USA",             400),
                // Tuesday, July 7
                M(95,  "TBD", "TBD", "Round of 16", U(7,7,16),   "Mercedes-Benz Stadium",  "Atlanta, USA",             400),
                M(96,  "TBD", "TBD", "Round of 16", U(7,7,20),   "BC Place",               "Vancouver, Canada",        400),

                // ══ QUARTER-FINALS ════════════════════════════════════════════

                M(97,  "TBD", "TBD", "Quarter-Final", U(7,9,19),  "Gillette Stadium",       "Boston, USA",              500),
                M(98,  "TBD", "TBD", "Quarter-Final", U(7,10,19), "SoFi Stadium",           "Los Angeles, USA",         500),
                M(99,  "TBD", "TBD", "Quarter-Final", U(7,11,16), "Hard Rock Stadium",      "Miami, USA",               500),
                M(100, "TBD", "TBD", "Quarter-Final", U(7,11,20), "Arrowhead Stadium",      "Kansas City, USA",         500),

                // ══ SEMI-FINALS ═══════════════════════════════════════════════

                M(101, "TBD", "TBD", "Semi-Final",  U(7,14,19),   "AT&T Stadium",           "Dallas, USA",              600),
                M(102, "TBD", "TBD", "Semi-Final",  U(7,15,19),   "Mercedes-Benz Stadium",  "Atlanta, USA",             600),

                // ══ THIRD PLACE ═══════════════════════════════════════════════

                M(103, "TBD", "TBD", "Third Place", U(7,18,20),   "Hard Rock Stadium",      "Miami, USA",               400),

                // ══ FINAL ═════════════════════════════════════════════════════

                M(104, "TBD", "TBD", "Final",       U(7,19,19),   "MetLife Stadium",        "New York/New Jersey, USA", 1000)
            );

            await ctx.SaveChangesAsync();
        }

        // ── 9. Missions ───────────────────────────────────────────────────────

        private static async Task SeedMissionsAsync(ApplicationDbContext ctx)
        {
            if (await ctx.Set<Mission>().AnyAsync(m => m.Id == MissionPredictId))
                return;

            ctx.Set<Mission>().AddRange(
                new Mission
                {
                    Id               = MissionPredictId,
                    Title            = "First Prediction",
                    Description      = "Make your first match prediction to earn points.",
                    Type             = MissionType.PredictMatch,
                    RequiredProgress = 1,
                    PointsReward     = 100,
                    XPValue          = 50,
                    Difficulty       = MissionDifficulty.Easy,
                    IsDaily          = false,
                    CreatedBy        = "seeder"
                },
                new Mission
                {
                    Id               = MissionScanQRId,
                    Title            = "QR Explorer",
                    Description      = "Scan 3 different QR codes across Heraklion.",
                    Type             = MissionType.ScanQRCode,
                    RequiredProgress = 3,
                    PointsReward     = 250,
                    XPValue          = 120,
                    Difficulty       = MissionDifficulty.Medium,
                    IsDaily          = false,
                    CreatedBy        = "seeder"
                },
                new Mission
                {
                    Id               = MissionReferralId,
                    Title            = "Bring a Friend",
                    Description      = "Invite a friend using your referral code.",
                    Type             = MissionType.ShareReferral,
                    RequiredProgress = 1,
                    PointsReward     = 300,
                    XPValue          = 150,
                    Difficulty       = MissionDifficulty.Easy,
                    IsDaily          = false,
                    CreatedBy        = "seeder"
                },
                new Mission
                {
                    Id               = MissionRouteId,
                    Title            = "City Explorer",
                    Description      = "Complete the Heraklion City Tour route.",
                    Type             = MissionType.CompleteTourismRoute,
                    RequiredProgress = 1,
                    PointsReward     = 500,
                    XPValue          = 250,
                    Difficulty       = MissionDifficulty.Hard,
                    IsDaily          = false,
                    CreatedBy        = "seeder"
                },
                new Mission
                {
                    Id               = MissionDailyId,
                    Title            = "Daily Login",
                    Description      = "Log in every day to keep your streak alive.",
                    Type             = MissionType.DailyLogin,
                    RequiredProgress = 1,
                    PointsReward     = 25,
                    XPValue          = 10,
                    Difficulty       = MissionDifficulty.Easy,
                    IsDaily          = true,
                    CreatedBy        = "seeder"
                },
                new Mission
                {
                    Id               = MissionStreakId,
                    Title            = "Prediction Streak",
                    Description      = "Make correct predictions 5 times in a row.",
                    Type             = MissionType.MakePredictionStreak,
                    RequiredProgress = 5,
                    PointsReward     = 750,
                    XPValue          = 400,
                    Difficulty       = MissionDifficulty.Expert,
                    IsDaily          = false,
                    RequiredUserLevel = 3,
                    CreatedBy        = "seeder"
                }
            );

            await ctx.SaveChangesAsync();
        }

        // ── 10. Rewards ───────────────────────────────────────────────────────

        private static async Task SeedRewardsAsync(ApplicationDbContext ctx)
        {
            if (await ctx.Rewards.AnyAsync(r => r.Id == RewardCoffeeId))
                return;

            ctx.Rewards.AddRange(
                new Reward
                {
                    Id             = RewardCoffeeId,
                    Name           = "Free Coffee at La Prima",
                    Description    = "Claim a free coffee at any La Prima Coffee location in Heraklion.",
                    PointsRequired = 200,
                    Type           = RewardType.FreeDrink,
                    ImageUrl       = "https://placehold.co/300x200?text=FreeCoffee",
                    Quantity       = 500,
                    ExpiryDate     = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                    IsVIPOnly      = false,
                    CreatedBy      = "seeder"
                },
                new Reward
                {
                    Id             = RewardDiscountId,
                    Name           = "15% Discount at Kyriakakis",
                    Description    = "Enjoy 15% off your total bill at Kyriakakis Restaurant.",
                    PointsRequired = 350,
                    Type           = RewardType.DiscountCoupon,
                    ImageUrl       = "https://placehold.co/300x200?text=Discount15",
                    Quantity       = 200,
                    ExpiryDate     = new DateTime(2026, 10, 31, 0, 0, 0, DateTimeKind.Utc),
                    IsVIPOnly      = false,
                    CreatedBy      = "seeder"
                },
                new Reward
                {
                    Id             = RewardVIPId,
                    Name           = "VIP Lounge Access",
                    Description    = "Exclusive VIP lounge access at Heraklion Port on your next visit.",
                    PointsRequired = 1000,
                    Type           = RewardType.VIPAccess,
                    ImageUrl       = "https://placehold.co/300x200?text=VIPAccess",
                    Quantity       = 50,
                    ExpiryDate     = new DateTime(2026, 6, 30, 0, 0, 0, DateTimeKind.Utc),
                    IsVIPOnly      = true,
                    CreatedBy      = "seeder"
                },
                new Reward
                {
                    Id             = RewardMerchId,
                    Name           = "The Field of Dreams T-Shirt",
                    Description    = "Exclusive branded merchandise — a souvenir of your Crete adventure.",
                    PointsRequired = 600,
                    Type           = RewardType.Merchandise,
                    ImageUrl       = "https://placehold.co/300x200?text=TShirt",
                    Quantity       = 100,
                    IsVIPOnly      = false,
                    CreatedBy      = "seeder"
                },
                new Reward
                {
                    Id             = RewardCashbackId,
                    Name           = "€5 Port Shop Cashback",
                    Description    = "Receive €5 cashback at any participating port shop.",
                    PointsRequired = 500,
                    Type           = RewardType.Cashback,
                    ImageUrl       = "https://placehold.co/300x200?text=Cashback5",
                    Quantity       = 300,
                    ExpiryDate     = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                    IsVIPOnly      = false,
                    CreatedBy      = "seeder"
                }
            );

            await ctx.SaveChangesAsync();
        }

        // ── 11. Tourism Routes ────────────────────────────────────────────────

        private static async Task SeedTourismRoutesAsync(ApplicationDbContext ctx)
        {
            if (await ctx.TourismRoutes.AnyAsync(r => r.Id == RouteHeraklionId))
                return;

            ctx.TourismRoutes.AddRange(
                new TourismRoute
                {
                    Id                       = RouteHeraklionId,
                    Name                     = "Heraklion City Tour",
                    Description              = "Discover the highlights of Heraklion: port, Lions Square, and the Venetian harbour.",
                    TotalPointsReward        = 400,
                    EstimatedDurationMinutes = 120,
                    IsHiddenGemRoute         = false,
                    CreatedBy                = "seeder"
                },
                new TourismRoute
                {
                    Id                       = RouteHiddenGemsId,
                    Name                     = "Hidden Gems of Crete",
                    Description              = "Go off the beaten path and discover the secret spots only locals know.",
                    TotalPointsReward        = 750,
                    EstimatedDurationMinutes = 240,
                    IsHiddenGemRoute         = true,
                    CreatedBy                = "seeder"
                }
            );

            await ctx.SaveChangesAsync();

            // Seed RouteLocations (junction table)
            await SeedRouteLocationsAsync(ctx);
        }

        private static async Task SeedRouteLocationsAsync(ApplicationDbContext ctx)
        {
            if (await ctx.Set<RouteLocation>().AnyAsync(rl => rl.RouteId == RouteHeraklionId))
                return;

            ctx.Set<RouteLocation>().AddRange(
                // Heraklion City Tour stops
                new RouteLocation { Id = Guid.NewGuid(), RouteId = RouteHeraklionId, LocationId = LocationPortId,    Order = 1, CreatedBy = "seeder" },
                new RouteLocation { Id = Guid.NewGuid(), RouteId = RouteHeraklionId, LocationId = LocationLionsId,   Order = 2, CreatedBy = "seeder" },
                new RouteLocation { Id = Guid.NewGuid(), RouteId = RouteHeraklionId, LocationId = LocationLaPrimaId, Order = 3, CreatedBy = "seeder" },

                // Hidden Gems of Crete stops
                new RouteLocation { Id = Guid.NewGuid(), RouteId = RouteHiddenGemsId, LocationId = LocationKnossosId,    Order = 1, CreatedBy = "seeder" },
                new RouteLocation { Id = Guid.NewGuid(), RouteId = RouteHiddenGemsId, LocationId = LocationKyriakakisId, Order = 2, CreatedBy = "seeder" },
                new RouteLocation { Id = Guid.NewGuid(), RouteId = RouteHiddenGemsId, LocationId = LocationLionsId,     Order = 3, CreatedBy = "seeder" }
            );

            await ctx.SaveChangesAsync();
        }

        // ── 12. Leaderboard ───────────────────────────────────────────────────

        private static async Task SeedLeaderboardAsync(ApplicationDbContext ctx, Dictionary<string, Guid> userIds)
        {
            if (await ctx.LeaderboardEntries.AnyAsync(l => l.Id == LBEntry1Id))
                return;

            // Use real seeded user IDs — FK requires a matching AspNetUsers row
            userIds.TryGetValue("passenger@demo.com", out var uid1);
            userIds.TryGetValue("merchant@demo.com",  out var uid2);
            userIds.TryGetValue("partner@demo.com",   out var uid3);

            // Skip entries whose user ID couldn't be resolved
            var entries = new List<LeaderboardEntry>();
            if (uid1 != Guid.Empty)
                entries.Add(new LeaderboardEntry
                {
                    Id              = LBEntry1Id,
                    UserId          = uid1,
                    Username        = "john_traveler",
                    TotalPoints     = 1850,
                    WeeklyPoints    = 420,
                    MonthlyPoints   = 1100,
                    Rank            = 1,
                    LeaderboardType = "Global",
                    LastUpdated     = DateTime.UtcNow,
                    CreatedBy       = "seeder"
                });
            if (uid2 != Guid.Empty)
                entries.Add(new LeaderboardEntry
                {
                    Id              = LBEntry2Id,
                    UserId          = uid2,
                    Username        = "crete_explorer",
                    TotalPoints     = 1420,
                    WeeklyPoints    = 310,
                    MonthlyPoints   = 890,
                    Rank            = 2,
                    LeaderboardType = "Global",
                    LastUpdated     = DateTime.UtcNow,
                    CreatedBy       = "seeder"
                });
            if (uid3 != Guid.Empty)
                entries.Add(new LeaderboardEntry
                {
                    Id              = LBEntry3Id,
                    UserId          = uid3,
                    Username        = "cruise_star",
                    TotalPoints     = 980,
                    WeeklyPoints    = 180,
                    MonthlyPoints   = 650,
                    Rank            = 3,
                    LeaderboardType = "Global",
                    LastUpdated     = DateTime.UtcNow,
                    CreatedBy       = "seeder"
                });

            if (entries.Count > 0)
            {
                ctx.LeaderboardEntries.AddRange(entries);
                await ctx.SaveChangesAsync();
            }
        }

        // ── 13. Passenger role-specific data ──────────────────────────────────

        private static async Task SeedPassengerDataAsync(ApplicationDbContext ctx, Guid userId)
        {
            await SeedPassengerPredictionsAsync(ctx, userId);
            await SeedPassengerUserMissionsAsync(ctx, userId);
            await SeedPassengerUserRewardAsync(ctx, userId);
            await SeedPassengerWalletAsync(ctx, userId);
            await SeedPassengerQRScansAsync(ctx, userId);
            await SeedPassengerReferralAsync(ctx, userId);
            await SeedPassengerLeaderboardEntryAsync(ctx, userId);
            await SeedPassengerNotificationsAsync(ctx, userId.ToString());
        }

        private static async Task SeedPassengerPredictionsAsync(ApplicationDbContext ctx, Guid userId)
        {
            if (await ctx.Predictions.AnyAsync(p => p.Id == PassengerPrediction1Id))
                return;

            ctx.Predictions.AddRange(
                new Prediction
                {
                    Id              = PassengerPrediction1Id,
                    UserId          = userId,
                    MatchId         = WcGuid(1), // Mexico vs Brazil – Group A MD1
                    HomeTeamScore   = 2,
                    AwayTeamScore   = 1,
                    WinnerPrediction = "Home",
                    Status          = PredictionStatus.Pending,
                    IsCorrect       = false,
                    PointsEarned    = 0,
                    PredictedAt     = DateTime.UtcNow.AddDays(-3),
                    CreatedBy       = "seeder"
                },
                new Prediction
                {
                    Id              = PassengerPrediction2Id,
                    UserId          = userId,
                    MatchId         = WcGuid(7), // USA vs Argentina – Group B MD1
                    HomeTeamScore   = 1,
                    AwayTeamScore   = 0,
                    WinnerPrediction = "Home",
                    Status          = PredictionStatus.Pending,
                    IsCorrect       = false,
                    PointsEarned    = 0,
                    PredictedAt     = DateTime.UtcNow.AddDays(-1),
                    CreatedBy       = "seeder"
                }
            );
            await ctx.SaveChangesAsync();
        }

        private static async Task SeedPassengerUserMissionsAsync(ApplicationDbContext ctx, Guid userId)
        {
            if (await ctx.UserMissions.AnyAsync(um => um.Id == PassengerUserMission1Id))
                return;

            ctx.UserMissions.AddRange(
                new UserMission
                {
                    Id              = PassengerUserMission1Id,
                    UserId          = userId,
                    MissionId       = MissionPredictId,
                    CurrentProgress = 1,
                    IsCompleted     = true,
                    CompletedAt     = DateTime.UtcNow.AddDays(-3),
                    PointsAwarded   = 100,
                    CreatedBy       = "seeder"
                },
                new UserMission
                {
                    Id              = PassengerUserMission2Id,
                    UserId          = userId,
                    MissionId       = MissionScanQRId,
                    CurrentProgress = 1,
                    IsCompleted     = false,
                    PointsAwarded   = 0,
                    CreatedBy       = "seeder"
                }
            );
            await ctx.SaveChangesAsync();
        }

        private static async Task SeedPassengerUserRewardAsync(ApplicationDbContext ctx, Guid userId)
        {
            if (await ctx.UserRewards.AnyAsync(ur => ur.Id == PassengerUserReward1Id))
                return;

            ctx.UserRewards.Add(new UserReward
            {
                Id                  = PassengerUserReward1Id,
                UserId              = userId,
                RewardId            = RewardCoffeeId,
                ClaimedAt           = DateTime.UtcNow.AddDays(-2),
                IsUsed              = false,
                IsExpired           = false,
                RewardName          = "Free Coffee at La Prima",
                RewardDescription   = "Claim a free coffee at any La Prima Coffee location in Heraklion.",
                RewardType          = RewardType.FreeDrink,
                PointsSpent         = 200,
                UsageCode           = "COFFEE-JOHN-2026",
                DeliveryMethod      = "QR",
                QrCodeValue         = "QR-COFFEE-JOHN-2026",
                PartnerName         = "La Prima Coffee",
                ExpiryDate          = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                AcquisitionChannel  = "Prediction",
                CreatedBy           = "seeder"
            });
            await ctx.SaveChangesAsync();
        }

        private static async Task SeedPassengerWalletAsync(ApplicationDbContext ctx, Guid userId)
        {
            if (await ctx.Wallets.AnyAsync(w => w.Id == PassengerWalletId))
                return;

            ctx.Wallets.Add(new Wallet
            {
                Id        = PassengerWalletId,
                UserId    = userId,
                Balance   = 125.50m,
                Points    = 450,
                CreatedBy = "seeder"
            });
            await ctx.SaveChangesAsync();

            ctx.Set<Transaction>().AddRange(
                new Transaction
                {
                    Id          = PassengerTxn1Id,
                    WalletId    = PassengerWalletId,
                    Amount      = 100m,
                    Type        = TransactionType.Reward,
                    Description = "QR scan reward — Knossos Palace",
                    Status      = TransactionStatus.Completed,
                    CreatedBy   = "seeder"
                },
                new Transaction
                {
                    Id          = PassengerTxn2Id,
                    WalletId    = PassengerWalletId,
                    Amount      = 50m,
                    Type        = TransactionType.Reward,
                    Description = "Welcome port check-in bonus",
                    Status      = TransactionStatus.Completed,
                    CreatedBy   = "seeder"
                },
                new Transaction
                {
                    Id          = PassengerTxn3Id,
                    WalletId    = PassengerWalletId,
                    Amount      = 24.50m,
                    Type        = TransactionType.Debit,
                    Description = "Free coffee reward redemption — La Prima",
                    Status      = TransactionStatus.Completed,
                    CreatedBy   = "seeder"
                }
            );
            await ctx.SaveChangesAsync();
        }

        private static async Task SeedPassengerQRScansAsync(ApplicationDbContext ctx, Guid userId)
        {
            if (await ctx.QRScans.AnyAsync(s => s.Id == PassengerQRScan1Id))
                return;

            ctx.QRScans.AddRange(
                new QRScan
                {
                    Id           = PassengerQRScan1Id,
                    UserId       = userId,
                    QRCodeId     = QRCodeKnossosId,
                    PointsAwarded = 150,
                    LocationData  = "Knossos Archaeological Site, Heraklion",
                    ScannedAt    = DateTime.UtcNow.AddDays(-5),
                    CreatedBy    = "seeder"
                },
                new QRScan
                {
                    Id           = PassengerQRScan2Id,
                    UserId       = userId,
                    QRCodeId     = QRCodePortId,
                    PointsAwarded = 50,
                    LocationData  = "Heraklion Port",
                    ScannedAt    = DateTime.UtcNow.AddDays(-6),
                    CreatedBy    = "seeder"
                }
            );
            await ctx.SaveChangesAsync();
        }

        private static async Task SeedPassengerReferralAsync(ApplicationDbContext ctx, Guid userId)
        {
            if (await ctx.Referrals.AnyAsync(r => r.Id == PassengerReferralId))
                return;

            // Use merchant as the referred user (a real seeded account)
            var merchant = await ctx.Set<AppUser>().FirstOrDefaultAsync(u => u.Email == "merchant@demo.com");
            if (merchant is null || !Guid.TryParse(merchant.Id, out var merchantGuid)) return;

            ctx.Referrals.Add(new Referral
            {
                Id              = PassengerReferralId,
                ReferrerUserId  = userId,
                ReferredUserId  = merchantGuid,
                PointsAwarded   = 300,
                IsCompleted     = true,
                CreatedBy       = "seeder"
            });
            await ctx.SaveChangesAsync();
        }

        private static async Task SeedPassengerLeaderboardEntryAsync(ApplicationDbContext ctx, Guid userId)
        {
            if (await ctx.LeaderboardEntries.AnyAsync(l => l.Id == PassengerLBEntryId))
                return;

            ctx.LeaderboardEntries.Add(new LeaderboardEntry
            {
                Id              = PassengerLBEntryId,
                UserId          = userId,
                Username        = "john_traveler",
                TotalPoints     = 450,
                WeeklyPoints    = 200,
                MonthlyPoints   = 450,
                Rank            = 1,
                LeaderboardType = "Ship",
                CruiseSessionId = CruiseSessionCostaDiademaId,
                ShipName        = "Costa Diadema",
                LastUpdated     = DateTime.UtcNow,
                CreatedBy       = "seeder"
            });
            await ctx.SaveChangesAsync();
        }

        private static async Task SeedPassengerNotificationsAsync(ApplicationDbContext ctx, string userIdStr)
        {
            if (await ctx.Notifications.AnyAsync(n => n.Id == PassengerNotif1Id))
                return;

            ctx.Notifications.AddRange(
                new AppNotification
                {
                    Id                = PassengerNotif1Id,
                    UserId            = userIdStr,
                    Title             = "Welcome to The Field of Dreams!",
                    Message           = "Your Crete adventure begins. Explore, predict, and earn rewards.",
                    Type              = NotificationType.Info,
                    IsRead            = true,
                    RelatedEntityType = "System",
                    CreatedBy         = "seeder"
                },
                new AppNotification
                {
                    Id                = PassengerNotif2Id,
                    UserId            = userIdStr,
                    Title             = "150 points earned at Knossos!",
                    Message           = "Great job scanning at Knossos Palace. Keep exploring to earn more!",
                    Type              = NotificationType.Success,
                    IsRead            = false,
                    RelatedEntityType = "QRScan",
                    RelatedEntityId   = PassengerQRScan1Id.ToString(),
                    CreatedBy         = "seeder"
                }
            );
            await ctx.SaveChangesAsync();
        }

        // ── 14. Merchant role-specific data ───────────────────────────────────

        private static async Task SeedMerchantDataAsync(ApplicationDbContext ctx, Guid userId)
        {
            await SeedMerchantWalletAsync(ctx, userId);
            await SeedMerchantNotificationAsync(ctx, userId.ToString());
        }

        private static async Task SeedMerchantWalletAsync(ApplicationDbContext ctx, Guid userId)
        {
            if (await ctx.Wallets.AnyAsync(w => w.Id == MerchantWalletId))
                return;

            ctx.Wallets.Add(new Wallet
            {
                Id        = MerchantWalletId,
                UserId    = userId,
                Balance   = 0m,
                Points    = 0,
                CreatedBy = "seeder"
            });
            await ctx.SaveChangesAsync();
        }

        private static async Task SeedMerchantNotificationAsync(ApplicationDbContext ctx, string userIdStr)
        {
            if (await ctx.Notifications.AnyAsync(n => n.Id == MerchantNotif1Id))
                return;

            ctx.Notifications.Add(new AppNotification
            {
                Id                = MerchantNotif1Id,
                UserId            = userIdStr,
                Title             = "Your Merchant Dashboard is ready",
                Message           = "Validate passenger perks via QR Scanner. Manage live slots through the Telegram Bot.",
                Type              = NotificationType.Info,
                IsRead            = false,
                RelatedEntityType = "System",
                CreatedBy         = "seeder"
            });
            await ctx.SaveChangesAsync();
        }

        // ── 15. Cruise Sessions ───────────────────────────────────────────────
        // Represents real ship port-day visits used to test PostGIS port-arrival
        // detection (ST_DWithin) and the ship leaderboard feature.

        private static async Task SeedCruiseSessionsAsync(ApplicationDbContext ctx)
        {
            if (await ctx.CruiseSessions.AnyAsync(c => c.Id == CruiseSessionCostaDiademaId))
                return;

            ctx.CruiseSessions.AddRange(
                // Ship currently in port at Heraklion — missions already assigned
                new CruiseSession
                {
                    Id                   = CruiseSessionCostaDiademaId,
                    ShipName             = "Costa Diadema",
                    CruiseLine           = "Costa Cruises",
                    VoyageNumber         = "CD-2026-0615",
                    PortLocationId       = LocationPortId,
                    ArrivalTime          = new DateTime(2026, 6, 15, 6, 0, 0, DateTimeKind.Utc),
                    DepartureTime        = new DateTime(2026, 6, 15, 20, 0, 0, DateTimeKind.Utc),
                    EstimatedPassengers  = 3000,
                    Status               = CruiseSessionStatus.InPort,
                    LastKnownPosition    = Pt(35.3420, 25.1442), // docked at Heraklion port
                    DistanceToPortMeters = 0,
                    MissionsAssigned     = true,
                    CreatedBy            = "seeder"
                },
                // Ship approaching Chania — not yet in port, missions not assigned
                new CruiseSession
                {
                    Id                   = CruiseSessionMSCSeaviewId,
                    ShipName             = "MSC Seaview",
                    CruiseLine           = "MSC Cruises",
                    VoyageNumber         = "MSC-2026-0616",
                    PortLocationId       = LocationChaniaPortId,
                    ArrivalTime          = new DateTime(2026, 6, 16, 6, 0, 0, DateTimeKind.Utc),
                    EstimatedPassengers  = 4140,
                    Status               = CruiseSessionStatus.Approaching,
                    LastKnownPosition    = Pt(35.4800, 23.9500), // ~10 km west of Souda Bay
                    DistanceToPortMeters = 10_400,
                    MissionsAssigned     = false,
                    CreatedBy            = "seeder"
                }
            );

            await ctx.SaveChangesAsync();
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private static Guid WcGuid(int n) => new($"57432026-0000-0000-0000-{n:D12}");

        private static string BuildReferralCode(string email)
        {
            var prefix = email.Split('@')[0].ToUpper();
            if (prefix.Length > 6) prefix = prefix[..6];
            var rand = new Random().Next(1000, 9999);
            return $"REF{prefix}{rand}";
        }
    }
}
