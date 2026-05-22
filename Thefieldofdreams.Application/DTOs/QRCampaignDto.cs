using System.ComponentModel.DataAnnotations;

namespace Thefieldofdreams.Application.DTOs
{
    public class QRCampaignDto
    {
        public Guid Id { get; set; }
        public string CampaignName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid CampaignId { get; set; }
        public Guid QRCodeId { get; set; }
        public int MaxScansPerUser { get; set; }
        public int TotalScanLimit { get; set; }
        public int CurrentScanCount { get; set; }
        public int BasePointsReward { get; set; }
        public int? BonusPointsForFirstScan { get; set; }
        public string? RewardCode { get; set; }
        public string? DiscountPercentage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public string? TargetUserGroup { get; set; }
        public int? MinimumUserLevel { get; set; }
        public int UniqueUsersScanned { get; set; }
        public int ConversionCount { get; set; }
        public decimal? ConversionRate { get; set; }
        public string? SuccessMessage { get; set; }
        public string? RedirectUrl { get; set; }
        public string? SponsorOfferCode { get; set; }
        public bool IsSponsorExclusive { get; set; }
    }

    public class CreateQRCampaignRequestDto
    {
        [Required]
        [StringLength(200)]
        public string CampaignName { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public Guid CampaignId { get; set; }

        [Required]
        public Guid QRCodeId { get; set; }

        [Range(1, int.MaxValue)]
        public int MaxScansPerUser { get; set; } = 1;

        [Range(0, int.MaxValue)]
        public int TotalScanLimit { get; set; } = 0;

        [Range(0, int.MaxValue)]
        public int BasePointsReward { get; set; } = 10;

        public int? BonusPointsForFirstScan { get; set; }

        [StringLength(100)]
        public string? RewardCode { get; set; }

        [StringLength(10)]
        public string? DiscountPercentage { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; } = true;

        [StringLength(50)]
        public string? TargetUserGroup { get; set; }

        public int? MinimumUserLevel { get; set; }

        [StringLength(500)]
        public string? SuccessMessage { get; set; }

        [StringLength(500)]
        public string? RedirectUrl { get; set; }

        [StringLength(100)]
        public string? SponsorOfferCode { get; set; }

        public bool IsSponsorExclusive { get; set; } = false;
    }

    public class UpdateQRCampaignRequestDto
    {
        [Required]
        [StringLength(200)]
        public string CampaignName { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Range(1, int.MaxValue)]
        public int MaxScansPerUser { get; set; } = 1;

        [Range(0, int.MaxValue)]
        public int TotalScanLimit { get; set; } = 0;

        [Range(0, int.MaxValue)]
        public int BasePointsReward { get; set; } = 10;

        public int? BonusPointsForFirstScan { get; set; }

        [StringLength(100)]
        public string? RewardCode { get; set; }

        [StringLength(10)]
        public string? DiscountPercentage { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; } = true;

        [StringLength(50)]
        public string? TargetUserGroup { get; set; }

        public int? MinimumUserLevel { get; set; }

        [StringLength(500)]
        public string? SuccessMessage { get; set; }

        [StringLength(500)]
        public string? RedirectUrl { get; set; }

        [StringLength(100)]
        public string? SponsorOfferCode { get; set; }

        public bool IsSponsorExclusive { get; set; } = false;
    }
}
