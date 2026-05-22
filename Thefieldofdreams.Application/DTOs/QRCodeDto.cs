using System.ComponentModel.DataAnnotations;
using Thefieldofdreams.Domain.Entities;

namespace Thefieldofdreams.Application.DTOs
{
    public class QRCodeDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public QRCodeType Type { get; set; }
        public Guid? SponsorId { get; set; }
        public Guid? LocationId { get; set; }
        public int PointsReward { get; set; }
        public int MaxScans { get; set; }
        public int CurrentScanCount { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsLimitedTime { get; set; }
    }

    public class CreateQRCodeRequestDto
    {
        [Required]
        [StringLength(500)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public QRCodeType Type { get; set; }

        public Guid? SponsorId { get; set; }
        public Guid? LocationId { get; set; }

        [Range(0, int.MaxValue)]
        public int PointsReward { get; set; } = 10;

        [Range(0, int.MaxValue)]
        public int MaxScans { get; set; } = 0;

        public DateTime? ExpiryDate { get; set; }
        public bool IsLimitedTime { get; set; } = false;
    }

    public class UpdateQRCodeRequestDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public QRCodeType Type { get; set; }

        public Guid? SponsorId { get; set; }
        public Guid? LocationId { get; set; }

        [Range(0, int.MaxValue)]
        public int PointsReward { get; set; } = 10;

        [Range(0, int.MaxValue)]
        public int MaxScans { get; set; } = 0;

        public DateTime? ExpiryDate { get; set; }
        public bool IsLimitedTime { get; set; } = false;
    }
}
