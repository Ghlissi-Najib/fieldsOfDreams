using System.ComponentModel.DataAnnotations;
using Thefieldofdreams.Domain.Entities;

namespace Thefieldofdreams.Application.DTOs
{
    public class CampaignDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid SponsorId { get; set; }
        public CampaignType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal? Budget { get; set; }
        public int TotalImpressions { get; set; }
        public int TotalClicks { get; set; }
        public int TotalConversions { get; set; }
        public CampaignStatus Status { get; set; }
        public int Priority { get; set; }
    }

    public class CreateCampaignRequestDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public Guid SponsorId { get; set; }

        public CampaignType Type { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public decimal? Budget { get; set; }

        public CampaignStatus Status { get; set; } = CampaignStatus.Draft;

        public int Priority { get; set; } = 0;
    }

    public class UpdateCampaignRequestDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public CampaignType Type { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public decimal? Budget { get; set; }

        public CampaignStatus Status { get; set; }

        public int Priority { get; set; } = 0;
    }
}
