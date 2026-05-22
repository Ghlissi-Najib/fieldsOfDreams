namespace Thefieldofdreams.Domain.Entities
{
    public class QRCampaign : BaseEntity
    {
        public required string CampaignName { get; set; }
        public string? Description { get; set; }

        // Foreign keys
        public Guid CampaignId { get; set; }
        public Guid QRCodeId { get; set; }

        // Campaign specifics
        public int MaxScansPerUser { get; set; } = 1;
        public int TotalScanLimit { get; set; } = 0; // 0 = unlimited
        public int CurrentScanCount { get; set; } = 0;

        // Points & rewards
        public int BasePointsReward { get; set; } = 10;
        public int? BonusPointsForFirstScan { get; set; }
        public string? RewardCode { get; set; }
        public string? DiscountPercentage { get; set; }

        // Time constraints
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; } = true;

        // Targeting
        public string? TargetUserGroup { get; set; } // VIP, NewUsers, All
        public int? MinimumUserLevel { get; set; }

        // Tracking
        public int UniqueUsersScanned { get; set; } = 0;
        public int ConversionCount { get; set; } = 0; // Users who took action after scan
        public decimal? ConversionRate { get; set; }

        // Content
        public string? SuccessMessage { get; set; }
        public string? RedirectUrl { get; set; }

        // Sponsor specific
        public string? SponsorOfferCode { get; set; }
        public bool IsSponsorExclusive { get; set; } = false;

        // Navigation properties
        public virtual Campaign? Campaign { get; set; }
        public virtual QRCode? QRCode { get; set; }
        public virtual ICollection<QRScan> QRScans { get; set; } = new List<QRScan>();
    }
}