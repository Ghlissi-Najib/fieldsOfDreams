using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Infrastructure.Identity;

namespace Thefieldofdreams.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public ApplicationDbContext() { }

        public DbSet<Prediction> Predictions => Set<Prediction>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Reward> Rewards => Set<Reward>();
        public DbSet<UserReward> UserRewards => Set<UserReward>();
        public DbSet<UserRouteCompletion> UserRouteCompletions => Set<UserRouteCompletion>();
        public DbSet<Campaign> Campaigns => Set<Campaign>();
        public DbSet<CampaignRule> CampaignRules => Set<CampaignRule>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<Sponsor> Sponsors => Set<Sponsor>();
        public DbSet<Wallet> Wallets => Set<Wallet>();
        public DbSet<QRCampaign> QRCampaigns => Set<QRCampaign>();
        public DbSet<QRScan> QRScans => Set<QRScan>();
        public DbSet<UserMission> UserMissions => Set<UserMission>();
        public DbSet<AppNotification> Notifications => Set<AppNotification>();
        public DbSet<LeaderboardEntry> LeaderboardEntries => Set<LeaderboardEntry>();
        public DbSet<QRCode> QRCodes => Set<QRCode>();
        public DbSet<Match> Matchs => Set<Match>();
        public DbSet<Referral> Referrals => Set<Referral>();
        public DbSet<TourismRoute> TourismRoutes => Set<TourismRoute>();
        public DbSet<CruiseSession> CruiseSessions => Set<CruiseSession>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ── PostGIS extension ─────────────────────────────────────────────
            builder.HasPostgresExtension("postgis");

            // ── Location spatial column + GIST index ──────────────────────────
            builder.Entity<Location>(e =>
            {
                e.Property(l => l.GeoLocation)
                    .HasColumnType("geography(Point,4326)");

                // GIST index powers ST_DWithin and ST_Within queries
                e.HasIndex(l => l.GeoLocation)
                    .HasMethod("gist")
                    .HasDatabaseName("ix_locations_geolocation");
            });

            // ── CruiseSession spatial column ──────────────────────────────────
            builder.Entity<CruiseSession>(e =>
            {
                e.Property(c => c.LastKnownPosition)
                    .HasColumnType("geography(Point,4326)");

                e.HasOne(c => c.PortLocation)
                    .WithMany(l => l.CruiseSessions)
                    .HasForeignKey(c => c.PortLocationId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ── User self-referential referral relationship ───────────────────
            // Map Referrer/ReferredUsers to the explicit ReferredByUserId column
            // so EF does not auto-create a separate ReferrerId shadow FK.
            builder.Entity<User>(e =>
            {
                e.HasOne(u => u.Referrer)
                    .WithMany(u => u.ReferredUsers)
                    .HasForeignKey(u => u.ReferredByUserId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ── LeaderboardEntry → CruiseSession (nullable) ───────────────────
            builder.Entity<LeaderboardEntry>(e =>
            {
                e.HasOne(l => l.CruiseSession)
                    .WithMany(c => c.ShipLeaderboard)
                    .HasForeignKey(l => l.CruiseSessionId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
