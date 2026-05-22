namespace Thefieldofdreams.Domain.Entities
{
    public class UserReward : BaseEntity
    {
        // Foreign keys
        public Guid UserId { get; set; }
        public Guid RewardId { get; set; }

        // Claim data
        public DateTime ClaimedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UsedAt { get; set; }
        public bool IsUsed { get; set; } = false;
        public bool IsExpired { get; set; } = false;

        // Reward details at time of claim (snapshot)
        public string RewardName { get; set; }
        public string? RewardDescription { get; set; }
        public RewardType RewardType { get; set; }
        public int PointsSpent { get; set; }

        // Usage tracking
        public string? UsageCode { get; set; }
        public string? UsageLocation { get; set; }
        public string? UsageDeviceInfo { get; set; }

        // Physical/digital delivery
        public string? DeliveryMethod { get; set; } // Email, QR, InApp, Physical
        public string? DeliveryEmail { get; set; }
        public string? QrCodeValue { get; set; }
        public string? BarcodeValue { get; set; }

        // Unique codes for partners
        public string? PartnerRedemptionCode { get; set; }
        public string? PartnerName { get; set; }
        public DateTime? PartnerRedeemedAt { get; set; }

        // Expiry & extensions
        public DateTime? ExpiryDate { get; set; }
        public bool IsExtended { get; set; } = false;
        public DateTime? ExtendedUntil { get; set; }
        public string? ExtensionReason { get; set; }

        // Notifications
        public bool ExpiryNotificationSent { get; set; } = false;
        public DateTime? ExpiryNotificationSentAt { get; set; }
        public bool ReminderSent { get; set; } = false;

        // Feedback
        public int? SatisfactionRating { get; set; }
        public string? FeedbackComment { get; set; }

        // Analytics
        public string? SourceCampaign { get; set; } // Which campaign gave this reward
        public string? AcquisitionChannel { get; set; } // Prediction, QRScan, Mission, Referral

        // Navigation properties
        public virtual User? User { get; set; }
        public virtual Reward? Reward { get; set; }
    }
}