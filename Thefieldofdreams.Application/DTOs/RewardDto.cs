using System.ComponentModel.DataAnnotations;
using Thefieldofdreams.Domain.Entities;

namespace Thefieldofdreams.Application.DTOs
{
    public class RewardDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int PointsRequired { get; set; }
        public RewardType Type { get; set; }
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; }
        public int ClaimedCount { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsVIPOnly { get; set; }
    }

    public class CreateRewardRequestDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Range(0, int.MaxValue)]
        public int PointsRequired { get; set; }

        public RewardType Type { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; } = 0;

        public DateTime? ExpiryDate { get; set; }
        public bool IsVIPOnly { get; set; } = false;
    }

    public class UpdateRewardRequestDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Range(0, int.MaxValue)]
        public int PointsRequired { get; set; }

        public RewardType Type { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; } = 0;

        public DateTime? ExpiryDate { get; set; }
        public bool IsVIPOnly { get; set; } = false;
    }
}
