using System.ComponentModel.DataAnnotations;
using Thefieldofdreams.Domain.Entities;

namespace Thefieldofdreams.Application.DTOs
{
    public class UserRewardDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RewardId { get; set; }
        public DateTime ClaimedAt { get; set; }
        public DateTime? UsedAt { get; set; }
        public bool IsUsed { get; set; }
        public bool IsExpired { get; set; }
        public string RewardName { get; set; } = string.Empty;
        public string? RewardDescription { get; set; }
        public RewardType RewardType { get; set; }
        public int PointsSpent { get; set; }
        public string? UsageCode { get; set; }
        public string? DeliveryMethod { get; set; }
        public string? QrCodeValue { get; set; }
        public string? BarcodeValue { get; set; }
        public string? PartnerRedemptionCode { get; set; }
        public string? PartnerName { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? SatisfactionRating { get; set; }
        public string? FeedbackComment { get; set; }
        public string? AcquisitionChannel { get; set; }
    }

    public class CreateUserRewardRequestDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid RewardId { get; set; }

        [StringLength(50)]
        public string? DeliveryMethod { get; set; }

        [EmailAddress]
        [StringLength(200)]
        public string? DeliveryEmail { get; set; }

        [StringLength(100)]
        public string? SourceCampaign { get; set; }

        [StringLength(50)]
        public string? AcquisitionChannel { get; set; }
    }

    public class UpdateUserRewardRequestDto
    {
        public bool IsUsed { get; set; }

        [StringLength(200)]
        public string? UsageLocation { get; set; }

        [StringLength(200)]
        public string? UsageDeviceInfo { get; set; }

        [Range(1, 5)]
        public int? SatisfactionRating { get; set; }

        [StringLength(1000)]
        public string? FeedbackComment { get; set; }
    }
}
